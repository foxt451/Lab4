using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace LZWArchiver
{
    public class WriteNameAndType
    {
        public static void WriteNAT(string path, List<string> listOfName, Encoding encoding)
        {
            for (int i = 0; i < listOfName.Count; i++)
            {
                FileInfo file = new FileInfo(path + listOfName[i]);
                long sizeFile = file.Length;
                using (BinaryReader copyFile = new(new FileStream(path + listOfName[i], FileMode.Open)))
                {
                    using (BinaryWriter writer = new(new FileStream(path + "writetype.bin", FileMode.Append)))
                    {
                        writer.Write(sizeFile);
                        byte[] NameFile = encoding.GetBytes(listOfName[i]);
                        writer.Write(NameFile.Length);
                        for (int j = 0; j < NameFile.Length; j++)
                        {
                            writer.Write(NameFile[j]);
                        }

                        while (copyFile.BaseStream.Position != copyFile.BaseStream.Length)
                        {
                            writer.Write(copyFile.ReadByte());
                        }
                    }
                }
                //File.Delete(path + listOfName[i]);
            }
        }
    }
}