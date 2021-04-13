using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using LZWArchiver;

namespace LZWArchiver
{
    static class Program
    {
        static void Main(string[] args)
        {
            string path = @"D:\Новая папка (2)\Lab4\LZWArchiver\bin\Debug\net5.0\";
            string commands = Console.ReadLine();
            string[] command = commands.Split(' ');
            List<string> ListOfInputFile = new List<string>();//.txt and etc
            if (command[0] == "--compress")
            {
                string NameFileOutput = command[1]; //.lzw
                for (int i = 2; i < command.Length; i++)
                {
                    ListOfInputFile.Add(command[i]);
                }
                FileWork file = new FileWork();
                for (int i = 0; i < ListOfInputFile.Count; i++)
                {
                    file.WriteType(path, ListOfInputFile[i]);
                }
                new Encoder(path+ListOfInputFile[0], path+NameFileOutput).Encode();
                //File.Delete(path+ListOfInputFile[0]);
            }
            //new Encoder("test.txt", "out.txt").Encode();
            else if (command[0] == "--decompress")
            {
                string NameInpFile = command[1]; // .lzw
                new Decoder(path+NameInpFile, "recovered.txt").Decode();
                FileWork file = new FileWork();
                file.ReadType(path + "recovered.txt"); // .exe and etc
            }
            else
            {
                throw new ArgumentException("Not indif operation");
            }
        }
    }
}
            
