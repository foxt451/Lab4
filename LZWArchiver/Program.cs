using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LZWArchiver
{
    static class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory()+"/";
            //new Encoder("wave1.ogg", "big.zip").Encode();
            //new Decoder("big.zip", "wave2.ogg").Decode();
            //FileInfo file = new FileInfo(@"D:\Новая папка (2)\Lab4\LZWArchiver\bin\Debug\net5.0\Encode.txt");
            //long sizeFile = file.Length;
            Encoding u = new ASCIIEncoding();
            byte[] fuck = u.GetBytes("MC");
            for (int i = 0; i < fuck.Length; i++)
            {
                Console.WriteLine(fuck[i]);
            }
        }
    }
}
            
