using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LZWArchiver
{
    public class ReadNameAndType
    {
        public static (long size, string filename) ReadNAT(FileReader input, Encoding encoding)
        {
            long numberBytes = input.Read64();
            int bytesInName = input.Read32(); 
            List<byte> listByte = new List<byte>(); 
            for (int i = 0; i < bytesInName; i++) 
            { 
                byte b = input.Read8(); 
                listByte.Add(b);
            }
            string nameFile = encoding.GetString(listByte.ToArray());
            return (numberBytes, nameFile);
        }

    }
}
