using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Net;
using System.Text;

namespace LZWArchiver
{
    public class FileWork
    {
        public void WriteType(string path, List<string> NameOfFile)
        {
            for (int i = 0; i < NameOfFile.Count; i++)
            {
                string[] tempStringOutFile = File.ReadAllLines(path + NameOfFile[i]);
                using (StreamWriter sw = new StreamWriter(path + "MergeFile.txt", true, System.Text.Encoding.Default))
                {
                    for (int j = 0; j < tempStringOutFile.Length; j++)
                    {
                        sw.WriteLine(tempStringOutFile[j]);
                    }

                    sw.WriteLine("!" + NameOfFile[i]);
                }

                File.Delete(path + NameOfFile[i]);
            }
        }

        public void ReadType(string path)
        {
            string filename = "";
            using (StreamReader rd = new StreamReader(path + "recovered.txt"))
            {
                while (!rd.EndOfStream)
                {
                    string tempLine = rd.ReadLine();
                    if (tempLine.Length != 0 && tempLine[0] == '!')
                    {
                        for (int j = 1; j < tempLine.Length; j++)
                        {
                            filename += tempLine[j];
                        }

                        File.Move(path + "tempFile.txt", filename);
                        filename = "";
                    }
                    else
                    {
                        using (StreamWriter sw = new StreamWriter(path + "tempFile.txt", true,
                            System.Text.Encoding.Default))
                        {
                            sw.WriteLine(tempLine);
                        }
                    }
                }
            }
        }
    }
}