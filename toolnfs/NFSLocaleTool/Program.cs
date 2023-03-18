using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NFSLocaleTool
{
    internal class Program
    {

        static void Main(string[] args)
        {
            NFSLocale nfs = new NFSLocale();
            if (args.Length == 0)
            {
                Console.WriteLine("Create CharsList:");
                Console.WriteLine("   -cl <outputcharstextfile>");
                Console.WriteLine("Create Histogram:");
                Console.WriteLine("   -hg <inputhistogramchunk> <inputcharstext> <outputhistogramchunkfile>");
                Console.WriteLine("Create Text:");
                Console.WriteLine("   -t <inputbinarychunk> <inputhistogramchunk> <outputtextfile>");
                Console.WriteLine("Create Binary:");
                Console.WriteLine("   -b <inputtextfile> <inputhistogramchunk> <inputidsfile> <outputbinarychunkfile>");

                Console.WriteLine("FrostbiteTool.exe -cl chars_list.txt");
                Console.WriteLine("FrostbiteTool.exe -h histogram.chunk chars_list.txt newhistogram.chunk");
                Console.WriteLine("FrostbiteTool.exe -t nfsunbound.chunk newhistogram.chunk nfsunbound.chunk.txt");
                Console.WriteLine("FrostbiteTool.exe -b nfsunbound.chunk.txt newhistogram.chunk nfsunbound.chunk.ids newnfsunbound.chunk");
                return;
            }
            string arg = args[0];
            switch (arg)
            {
                case "-help":
                    {
                        Console.WriteLine("Create CharsList:");
                        Console.WriteLine("   -cl <outputcharstextfile>");
                        Console.WriteLine("Create Histogram:");
                        Console.WriteLine("   -hg <inputhistogramchunk> <inputcharstext> <outputhistogramchunkfile>");
                        Console.WriteLine("Create Text:");
                        Console.WriteLine("   -t <inputbinarychunk> <inputhistogramchunk> <outputtextfile>");
                        Console.WriteLine("Create Binary:");
                        Console.WriteLine("   -b <inputtextfile> <inputhistogramchunk> <inputidsfile> <outputbinarychunkfile>");

                        Console.WriteLine("FrostbiteTool.exe -cl chars_list.txt");
                        Console.WriteLine("FrostbiteTool.exe -hg histogram.chunk chars_list.txt newhistogram.chunk");
                        Console.WriteLine("FrostbiteTool.exe -t nfsunbound.chunk newhistogram.chunk nfsunbound.chunk.txt");
                        Console.WriteLine("FrostbiteTool.exe -b nfsunbound.chunk.txt newhistogram.chunk nfsunbound.chunk.ids newnfsunbound.chunk");
                        return;
                    }

                case "-h":
                    {
                        Console.WriteLine("Create CharsList:");
                        Console.WriteLine("   -cl <outputcharstextfile>");
                        Console.WriteLine("Create Histogram:");
                        Console.WriteLine("   -hg <inputhistogramchunk> <inputcharstext> <outputhistogramchunkfile>");
                        Console.WriteLine("Create Text:");
                        Console.WriteLine("   -t <inputbinarychunk> <inputhistogramchunk> <outputtextfile>");
                        Console.WriteLine("Create Binary:");
                        Console.WriteLine("   -b <inputtextfile> <inputhistogramchunk> <inputidsfile> <outputbinarychunkfile>");

                        Console.WriteLine("FrostbiteTool.exe -cl chars_list.txt");
                        Console.WriteLine("FrostbiteTool.exe -hg histogram.chunk chars_list.txt newhistogram.chunk");
                        Console.WriteLine("FrostbiteTool.exe -t nfsunbound.chunk newhistogram.chunk nfsunbound.chunk.txt");
                        Console.WriteLine("FrostbiteTool.exe -b nfsunbound.chunk.txt newhistogram.chunk nfsunbound.chunk.ids newnfsunbound.chunk");
                        return;
                    }

                case "-cl":
                    {
                        nfs.CreateListChars(args[1]);
                        return;
                    }
                case "-hg":
                    {                      
                        nfs.HistogramWrite(args[2], args[1], args[3]);                     
                        return;
                    }
                case "-t":
                    {
                        nfs.Read(args[1], args[2]);
                        nfs.ExtractText(args[3]);
                        return;
                    }
                case "-b":
                    {
                        nfs.WriteFromText(args[1], args[2], args[4], args[3]);
                        return;
                    }
                default: {
                        
                        return;
                    }
            }
        }
    }
}
