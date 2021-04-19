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
            //args = new string[] {"--compress", "dada.lzw", "encode.png"};
            args = new string[] {"--decompress", "dada.lzw"};//f - 101 0
            //new Encoder("wave1.ogg", "big.zip").Encode();
            //new Decoder("big.zip", "wave2.ogg").Decode();
            string path = Directory.GetCurrentDirectory()+"/";
            if (args[0] == "--compress")
            {
                List<string> ListOfInputFile = new List<string>();//.txt and etc
                string NameFileOutput = args[1]; //.lzw
                for (int i = 2; i < args.Length; i++)
                {
                    new WriteNameAndType(path, args[i], encoding);
                    new Encoder(path+args[i], path+NameFileOutput).Encode();
                }
            }
            //new Encoder("test.txt", "out.txt").Encode();
            else if (args[0] == "--decompress")
            {
                string NameInpFile = args[1]; // .lzw
                new Decoder(path+NameInpFile, path+"test.bin").Decode();
                //FileWork file = new FileWork();
                //file.ReadType(path); // .exe and etc
                new ReadNameAndType(path, "test.bin", encoding);
                File.Delete(path+"test.bin");
            }
            else
            {
                throw new ArgumentException("Not indif operation");
            }
        }
    }
}
            
