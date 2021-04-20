using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace LZWArchiver
{
    public class WriteNameAndType
    {
        public static void WriteNAT(string pathInput, string pathOutput, Encoding encoding, FileWriter writer)
        {
            FileInfo file = new(pathInput);
            long sizeFile = file.Length;
            writer.Write64(sizeFile);
            byte[] NameFile = encoding.GetBytes(file.Name);
            writer.Write32(NameFile.Length);
            for (int j = 0; j < NameFile.Length; j++)
            {
                writer.WriteBits(NameFile[j], 8);
            }
        }
    }
}