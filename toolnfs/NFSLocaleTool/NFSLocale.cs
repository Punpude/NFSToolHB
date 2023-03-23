using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NFSLocaleTool
{
    internal class NFSLocale
    {
        //all offsets without 8 bytes
        public uint Signature = 0x039000;
        public char end_line = '¬';
        public UInt16[] ArrayToHistogram { get; set; }
        public string[] DefArrayFile { get; set; }
        public int DefLengthFile { get; set; }
        public byte[] DefArrayFromStrings { get; set; }
        public UInt16[] ArrayOriginList { get; set; }
        public UInt16[] ArrayCharsList { get; set; }        
        public int FileSize { get; set; }
        public int EntriesNum { get; set; }
        public int TableOffset { get; set; }
        public int DataOffset { get; set; }
        public int DataOffSize { get; set; }
        public string Type { get; set; }
        List<Entry> Entries { get; set; }       
        List<Entry> ToEndFile { get; set; }       
        public void ExtractText(string outputFile)
        {
            List<string> text = new List<string>();
            foreach (Entry entry in Entries)
            {                
                text.Add($"{entry.String}");
            }
            File.WriteAllLines(outputFile, text.ToArray());
        }

        public void WriteFromText(string textFile, string hgfile, string outputChunk, string idsFile)
        {
            HistogramReadChars(hgfile);
            DefaultListChars();

            string[] text = File.ReadAllLines(textFile);
            string[] ids_ = File.ReadAllLines(idsFile);
                           
            if (text.Length != ids_.Length)
            {
                Console.WriteLine($"Несоответствие кол-ва строк в файле {Path.GetFileName(textFile)} к ID в файле {Path.GetFileName(idsFile)}");
                Console.WriteLine($"{text.Length}/{ids_.Length}");
                return;
            }
            uint[] ids = new uint[text.Length];
            uint[] offsets = new uint[text.Length];
            for(int i = 0; i < ids_.Length; i++)
            {
                ids[i] = UInt32.Parse(ids_[i], System.Globalization.NumberStyles.HexNumber);
            }
            using(BinaryWriter writer = new BinaryWriter(File.Create(outputChunk)))
            {
                writer.Write(Signature);
                writer.Write(0); //skip filesize
                writer.Write(text.Length); //listSize
                writer.Write(140); //table offset dataOffset
                int textStart = (text.Length * 8) + 0x8C;//calculate text offset stringsOffset 8-9
                writer.Write(textStart);
                writer.Write(Encoding.UTF8.GetBytes("Default"));

                writer.BaseStream.Position = textStart + 8; // start text
                for(int i = 0; i < text.Length; i++)
                {
                    offsets[i] = (uint)((writer.BaseStream.Position - textStart) - 8);
                    writer.Write(NFSEncoder(text[i].Replace("\"\"", "\"")));
                    writer.Write(new byte()); //null term
                }
                writer.BaseStream.Position = 0x94;
                for (int i = 0; i < text.Length; i++)
                {
                    writer.Write(ids[i]);
                    writer.Write(offsets[i]);
                }

                writer.BaseStream.Position = 4;
                writer.Write((uint)(writer.BaseStream.Length - 8));
            }
        }

        public void CreateListChars(string outputfile)
        {            
            byte[] defList = Resource1.charslist;
            UInt16[] defListUI = new UInt16[(defList.Length)/2];
            using (BinaryWriter writer = new BinaryWriter(File.Create(outputfile)))
            {               
                for (int i = 0; i < defList.Length; i+=2)
                {
                    ushort _a = (ushort)((defList[i]) | (defList[i + 1]) << 8);
                    defListUI[i/2] = _a;
                    writer.Write(defListUI[i/2]);                
                }                
            }        
        }

        public void HistogramWrite(string hgfile, string file, string outputfile)
        {
            HistogramReadChars(file);
            DefaultListChars();

            int countDefList = File.ReadAllLines(hgfile).Length;
            //Console.WriteLine($"{countDefList}");
            ArrayCharsList = new UInt16[DataOffSize/2];
            using (BinaryReader charsReader = new BinaryReader(File.OpenRead(hgfile)))
            {             
                int countArrayCharsList = 0;
                UInt16 tempInt;
                charsReader.BaseStream.Position = 2;
                for (int i = 1; i < (countDefList*3-1); i++)
                {
                    tempInt = charsReader.ReadUInt16();                  
                    if ((tempInt != 10) & (tempInt != 13) & (tempInt != Convert.ToUInt16(end_line)))
                    {
                        ArrayCharsList[countArrayCharsList] = tempInt;
                        countArrayCharsList++;                                         
                    }
                }             
            }

            if (ArrayOriginList.Contains(Convert.ToUInt16(end_line)))
            {
                Console.WriteLine($"Error, replace ¬(00AC) with 0000(utf-16) in original histogram.");
                Environment.Exit(1);
            }

            //256 dataoffsize
            UInt16[] arrayRemoveSimilar = new UInt16[DataOffSize/2];           
            int countRemove = 0;
            for (var i = 0; i < ArrayCharsList.Length; i++)
            {                
                if (ArrayOriginList.Contains(ArrayCharsList[i]))
                {
                    continue;
                }                  
                else
                {
                    arrayRemoveSimilar[countRemove] = ArrayCharsList[i];
                    countRemove++;                  
                }               
            }

            

            foreach (UInt16 val in arrayRemoveSimilar)
            {
                int count_int = 0;
                if (val != 0)
                {
                    for (var i = 0; i < arrayRemoveSimilar.Length; i++)
                    {
                        if (arrayRemoveSimilar[i] == val)
                        {
                            count_int++;
                        }
                    }
                    if (count_int > 1)
                    {
                        Console.WriteLine($"Error, the same chars are in the chars_list.");
                        Environment.Exit(1);
                    }
                }                
            }
                    

            ArrayToHistogram = new UInt16[DataOffSize/2];           
            int countToHistogram = 0;
            countRemove = 0;          
            UInt16 zero = 0;
            for (var i = 0; i < ArrayOriginList.Length; i++)
            {
                if (ArrayOriginList[i] == zero)
                {
                    if (countRemove < arrayRemoveSimilar.Length)
                    {
                        ArrayToHistogram[countToHistogram] = arrayRemoveSimilar[countRemove];
                        countToHistogram++;
                        countRemove++;
                    }
                }
                else
                {
                    ArrayToHistogram[countToHistogram] = ArrayOriginList[i];
                    countToHistogram++;
                }
            }

            using (BinaryWriter hwriter = new BinaryWriter(File.Create(outputfile)))
            {
                hwriter.Write(Signature + 1);
                hwriter.Write(DataOffset);
                hwriter.Write(DataOffSize);
                hwriter.BaseStream.Position = 0x10c;              
                foreach (UInt16 val in ArrayToHistogram)
                {
                    hwriter.Write(val);
                }
                var hisEndId = from p in ToEndFile select p.Id;
                foreach (UInt16 val in hisEndId)
                {
                    hwriter.Write(val);
                }
            }
        }

        
        public void DefaultListChars()
        {
            string defList = NFSLocaleTool.Resource1.default_list_chunk_number;
            DefArrayFile = defList.Split('\n');
            DefLengthFile = DefArrayFile.Length;
            
            DefArrayFromStrings = new byte[DefLengthFile];
            for (int i = 0; i < DefLengthFile; i++)
            {
                DefArrayFromStrings[i] = byte.Parse(DefArrayFile[i], System.Globalization.NumberStyles.HexNumber);               
            }           
        }
        

        public void HistogramReadChars(string file)
        {
            using (BinaryReader hCharReader = new BinaryReader(File.OpenRead(file)))
            {
                if (hCharReader.ReadUInt32() != Signature + 1)
                    throw new Exception("Unknown file");
                DataOffset = hCharReader.ReadInt32(); //270600
                DataOffSize = hCharReader.ReadInt32(); //256
                
                hCharReader.BaseStream.Position = 0x10c;

                UInt16 tempOriginListReader;
                ArrayOriginList = new UInt16[DataOffSize/2];
                for (int i = 0; i < (DataOffSize/2); i++)
                {
                    tempOriginListReader = hCharReader.ReadUInt16();
                    ArrayOriginList[i] = tempOriginListReader;                  
                }

                ToEndFile = new List<Entry>();
                for (int i = 0; i < (DataOffset - (DataOffSize + 260)) / 2; i++)
                {
                    ToEndFile.Add(new Entry()
                    {
                        Id = hCharReader.ReadUInt16()
                    });
                }

            }
        }

        
        public void Read(string file, string hgfile)
        {
            HistogramReadChars(hgfile);
            DefaultListChars();

            using (BinaryReader reader = new BinaryReader(File.OpenRead(file)))
            {
                if (reader.ReadUInt32() != Signature)
                    throw new Exception("Unknown file");
                FileSize = reader.ReadInt32();
                EntriesNum = reader.ReadInt32();
                TableOffset = reader.ReadInt32();
                DataOffset = reader.ReadInt32();
                Type = Utils.ReadString(reader, Encoding.UTF8);

                Entries = new List<Entry>();
                reader.BaseStream.Position = TableOffset + 8;
                for(int i = 0; i < EntriesNum; i++)
                {
                    Entries.Add(new Entry()
                    {
                        Id = reader.ReadUInt32(),
                        Offset = reader.ReadInt32() + DataOffset + 8
                    });
                }
                for(int i = 0; i < EntriesNum; i++)
                {
                    Entry entry = Entries[i];
                    reader.BaseStream.Position = entry.Offset;
                    entry.String = NFSDecoder(Utils.ReadNullTerminatedArray(reader));
                    Entries[i] = entry;
                }

                File.WriteAllLines(file + ".ids", Entries.Select(i => i.Id.ToString("X8")));
            }
        }

        private byte[] NFSEncoder(string text)
        {
            List<byte> result = new List<byte>();
            
            for(int i = 0; i < text.Length; i++)
            {
                int index = Array.IndexOf(ArrayOriginList, text[i]);
                if (index != -1)
                {
                    result.Add(DefArrayFromStrings[index]);
                    continue;
                }
                if (text[i] == end_line)
                {
                    result.Add(0x0a);
                    continue;
                }
                result.Add((byte)text[i]);
            }
            return result.ToArray();
        }

        private string NFSDecoder(byte[] data)
        {
            string result = "";
                       
            for (int i = 0; i < data.Length; i++)
            {
                int index = Array.IndexOf(DefArrayFromStrings, data[i]);
                if (index != -1)
                {
                    result += (char)ArrayOriginList[index];
                    continue;
                }
                if (data[i] == 10)
                {
                    result += end_line;
                    continue;
                }
                result += (char)data[i];                             
            }                       
            return result;          
        }                           
    }                               
                                    
    internal class Entry            
    {
        public uint Id { get; set; }
        public int Offset { get; set; }
        public byte[] StringArray { get; set; }
        public string String { get; set; }
    }
}
