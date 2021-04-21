using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LZWArchiver
{
    public class ReadNameAndType
    {
        // if a dir - length is the number of files, a file - length in bytes
        public static (long length, string name, bool isDir) ReadNAT(FileReader input, Encoding encoding)
        {
            byte flag = (byte) input.ReadBits(8);
            if (flag == 0)
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
                return (numberBytes, nameFile, false);
            } else
            {
                int numberOfFiles = input.Read32();
                int bytesInName = input.Read32();
                List<byte> listByte = new List<byte>();
                for (int i = 0; i < bytesInName; i++)
                {
                    byte b = input.Read8();
                    listByte.Add(b);
                }
                string nameDir = encoding.GetString(listByte.ToArray());
                return (numberOfFiles, nameDir, true);
            }
        }

    }
}
