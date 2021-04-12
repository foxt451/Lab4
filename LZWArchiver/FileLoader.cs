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

        public BinaryWriter LoadFileBinaryWriter(string filePath)
        {
            BinaryWriter writer = new(new FileStream(filePath, FileMode.Create));
            return writer;
        }
    }
}
