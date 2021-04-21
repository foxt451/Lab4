using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LZWArchiver
{
    public class FileLoader
    {
        public BinaryReader LoadFileBinaryReader(string filePath)
        {
            BinaryReader reader = new(new FileStream(filePath, FileMode.Open));
            return reader;
        }

        public BinaryReader LoadFileBinaryReader(string filePath, string dirName)
        {
            BinaryReader reader = new(new FileStream(dirName + "\\" + filePath, FileMode.Open));
            return reader;
        }

        public BinaryWriter LoadFileBinaryWriter(string filePath, FileMode filemode = FileMode.Create)
        {
            BinaryWriter writer = new(new FileStream(filePath, filemode));
            return writer;
        }

        public BinaryWriter LoadFileBinaryWriter(string filePath, string dirName, bool hasToOverWrite, FileMode filemode = FileMode.Create)
        {
            DirectoryInfo dir = new(dirName);
            if (dir.Exists && hasToOverWrite)
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo d in dir.GetDirectories())
                {
                    d.Delete(true);
                }
            } else if (!dir.Exists)
            {
                dir.Create();
            }
            BinaryWriter writer = new(new FileStream(dirName + "\\" + filePath, filemode));
            return writer;
        }
    }
}
