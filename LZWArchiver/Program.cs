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
            //args = new string[] {"--compress", "dada.lzw", "Encode.txt", "Encode2.txt"};
            //args = new string[] {"--decompress", "dada.lzw"};//f - 101 0
            //new Encoder("wave1.ogg", "big.zip").Encode();
            //new Decoder("big.zip", "wave2.ogg").Decode();
            string path = Directory.GetCurrentDirectory()+"/";
            if (args[0] == "--compress")
            {
                string NameFileOutput = args[1]; //.lzw
                List<string> listOfNameFile = new List<string>();
                for (int i = 2; i < args.Length; i++)
                {
                    listOfNameFile.Add(args[i]);
                }
                WriteNameAndType.WriteNAT(path, listOfNameFile, encoding);
                new Encoder(path+"writetype.bin", path+NameFileOutput).Encode();
                File.Delete(path+"writetype.bin");
            }
            else if (args[0] == "--decompress")
            {
                string NameInpFile = args[1]; // .lzw
                new Decoder(path+NameInpFile, path+"decoding.bin").Decode();
                ReadNameAndType.ReadNAT(path, "decoding.bin", encoding);
                File.Delete(path+"decoding.bin");
            }
            else
            {
                throw new ArgumentException("Not indif operation");
            }
        }
    }
}
            
