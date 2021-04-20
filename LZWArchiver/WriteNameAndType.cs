using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace LZWArchiver
{
    public class WriteNameAndType
    {
        public static void WriteNAT(string path, string name, string nameOutput, Encoding encoding)
        {
            FileInfo file = new FileInfo(path + name);
                long sizeFile = file.Length;
                using (BinaryWriter writer = new(new FileStream(path + nameOutput, FileMode.Append)))
                    {
                        writer.Write(sizeFile);
                        byte[] NameFile = encoding.GetBytes(name);
                        writer.Write(NameFile.Length);
                        for (int j = 0; j < NameFile.Length; j++)
                        {
                            writer.Write(NameFile[j]);
                        }
                    }
                }
                //File.Delete(path + listOfName[i]);
            }
        }