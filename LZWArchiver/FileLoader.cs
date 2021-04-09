using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LZWArchiver
{
    class FileLoader
    {
        public BinaryReader LoadFileBinaryReader(string filePath)
        {
            BinaryReader reader = new(new FileStream(filePath, FileMode.Open));
            return reader;
        }
    }
}
