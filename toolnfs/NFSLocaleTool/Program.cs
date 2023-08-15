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
                Text_console();
                return;
            }
            string arg = args[0];
            switch (arg)
            {
                case "-help":
                    {
                        Text_console();        
                        return;
                    }

                case "-h":
                    {
                        Text_console();
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

        static void Text_console()
        {
            Console.WriteLine("Generate a list of characters:");
            Console.WriteLine("   -cl <outputcharstextfile>");
            Console.WriteLine("Export the game's locale histogram chunk, then compile a new histogram chunk using generated list of characters and game's histogram chunk:");
            Console.WriteLine("   -hg <inputhistogramchunk> <inputcharstext> <outputhistogramchunkfile>");
            Console.WriteLine("Extract the locale text from game's binary chunk using new histogram chunk:");
            Console.WriteLine("   -t <inputbinarychunk> <inputhistogramchunk> <outputtextfile>");
            Console.WriteLine("Compile a binary chunk with new edits using new histogram and edited text file:");
            Console.WriteLine("   -b <inputtextfile> <inputhistogramchunk> <inputidsfile> <outputbinarychunkfile>");

            Console.WriteLine("NFSLocaleToolHB.exe -cl chars_list.txt");
            Console.WriteLine("NFSLocaleToolHB.exe -hg histogram.chunk chars_list.txt newhistogram.chunk");
            Console.WriteLine("NFSLocaleToolHB.exe -t nfsunbound.chunk newhistogram.chunk nfsunbound.chunk.txt");
            Console.WriteLine("NFSLocaleToolHB.exe -b nfsunbound.chunk.txt newhistogram.chunk nfsunbound.chunk.ids newnfsunbound.chunk");
            return;
        }
    }
}
