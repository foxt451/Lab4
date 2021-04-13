using System;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Text;

namespace LZWArchiver
{
    public class FileWork
    {
        public void WriteType(string path, string name)
        {
            using (StreamWriter sw = new StreamWriter(path+name, true, System.Text.Encoding.Default))
            {
                sw.WriteLine(name);
            }
        }

        public void ReadType(string path)
        {
            string filename = "";
            using (StreamReader rd = new StreamReader(path))
            {
                while (!rd.EndOfStream)
                {
                    filename = rd.ReadLine();
                }
            }
            var tempFile = Path.GetTempFileName();
            var linesToKeep = File.ReadLines(path).Where(l => l != filename);

            File.WriteAllLines(tempFile, linesToKeep);

            File.Delete(path);
            File.Move(tempFile, filename);
        }
    }
}