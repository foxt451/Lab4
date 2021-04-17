using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mime;
using LZWArchiver;

namespace LZWArchiver
{
    static class Program
    {
        static void Main(string[] args)
        {
            
            string path = Directory.GetCurrentDirectory()+"/";
            if (args[0] == "--compress")
            {
                List<string> ListOfInputFile = new List<string>();//.txt and etc
                string NameFileOutput = args[1]; //.lzw
                for (int i = 2; i < args.Length; i++)
                {
                    ListOfInputFile.Add(args[i]);
                }
                FileWork file = new FileWork();
                
                file.WriteType(path, ListOfInputFile);
               
                new Encoder(path+ListOfInputFile[0], path+NameFileOutput).Encode();
                File.Delete(path+ListOfInputFile[0]);
            }
            //new Encoder("test.txt", "out.txt").Encode();
            else if (args[0] == "--decompress")
            {
                string NameInpFile = args[1]; // .lzw
                new Decoder(path+NameInpFile, path+"recovered.txt").Decode();
                FileWork file = new FileWork();
                file.ReadType(path+"recovered.txt"); // .exe and etc
                File.Delete(path+NameInpFile);
            }
            else
            {
                throw new ArgumentException("Not indif operation");
            }
        }
    }
}
            
