using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace LZWArchiver
{
    public class WriteNameAndType
    {
        public static void WriteNAT(string pathInput, Encoding encoding, FileWriter writer)
        {
            // 0 if it's a file
            writer.WriteBits(0, 8);
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

        public static void WriteDir(DirectoryInfo dir, Encoding encoding, FileWriter writer)
        {
            int numberOfFiles = dir.GetFiles().Length;
            string dirName = dir.Name;
            // 1 if it's a dir
            writer.WriteBits(1, 8);
            // write number of files
            writer.Write32(numberOfFiles);
            // write the length of dir name
            byte[] NameDir = encoding.GetBytes(dirName);
            writer.Write32(NameDir.Length);
            // write dir name
            for (int j = 0; j < NameDir.Length; j++)
            {
                writer.WriteBits(NameDir[j], 8);
            }
        }
    }
}