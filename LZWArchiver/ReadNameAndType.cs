using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LZWArchiver
{
    public class ReadNameAndType
    {
        public static void ReadNAT(string path, string name, Encoding encoding)
        {
            using (BinaryReader br = new(new FileStream(path + name, FileMode.Open)))
            {
                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    long numberBytes = 0;
                    string NameFile = String.Empty;
                    numberBytes = br.ReadInt64();
                    int bytesInName = br.ReadInt32();
                    List<byte> listByte = new List<byte>();
                    for (int i = 0; i < bytesInName; i++)
                    {
                        byte b = br.ReadByte();
                        listByte.Add(b);
                    }

                    NameFile = encoding.GetString(listByte.ToArray());

                    using (BinaryWriter writer = new(new FileStream(path + NameFile, FileMode.Create)))
                    {
                        for (int i = 0; i < numberBytes; i++)
                        {
                            writer.Write(br.ReadByte());
                        }
                    }
                }
            }

        }
        
    }
}