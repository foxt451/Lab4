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

        public string ReadType(string path)
        {
            string filename = "";
            using (StreamReader rd = new StreamReader(path))
            {
                rd.ReadLine();
                filename = rd.ReadLine();
            }
            FileStream file = new FileStream(path + "/rating.csv", FileMode.OpenOrCreate);
            File.WriteAllLines(file, 
                File.ReadLines(file).Where(l => l != filename).ToList());

            return filename;
        }
    }
}