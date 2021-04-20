using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace LZWArchiver
{
    static class Program
    {
        static void Main(string[] args)
        {
            Encoding encoding = new UnicodeEncoding();
            if (args[0] == "--compress")
            {
                string nameFileOutput = args[1]; //.lzw
                new Encoder(args[2..], nameFileOutput, encoding).Encode();
            }
            else if (args[0] == "--decompress")
            {
                string NameInpFile = args[1]; // .lzw
                new Decoder(NameInpFile, encoding).Decode();
            }
            else
            {
                Console.WriteLine("Undefined operation");
                return;
            }
        }
    }
}
            
