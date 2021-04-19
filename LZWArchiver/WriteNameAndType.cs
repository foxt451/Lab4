using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace LZWArchiver
{
    public class WriteNameAndType
    {
        public WriteNameAndType(string path, string name)
        {
            FileInfo file = new FileInfo(path+name);
            long sizeFile = file.Length;
            using (BinaryReader copyFile = new(new FileStream(path + name, FileMode.Open)))
            {
                using (BinaryWriter writer = new(new FileStream(path+name+"1", FileMode.Create)))
                {
                    writer.Write(sizeFile);
                    Encoding u = new ASCIIEncoding();
                    byte[] NameFile = u.GetBytes(name);
                    for (int i = 0; i < NameFile.Length; i++)
                    {
                        writer.Write(NameFile[i]);
                    }
                    writer.Write(0);
                    while (copyFile.BaseStream.Position != copyFile.BaseStream.Length)
                    {
                        writer.Write(copyFile.ReadByte());
                    }
                }
            }
            File.Delete(path+name);
            File.Move(path+name+"1", name);
        }
    }
}