using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LZWArchiver
{
    class Encoder
    {
        private Dictionary<List<byte>, int> codeTable = new(new ListComparer<byte>());
        private int wordSizeInBits = 8;
        private string inputFile, outputFile;
        public Encoder(string inputFile, string outputFile)
        {
            this.inputFile = inputFile;
            this.outputFile = outputFile;
            for (int i = 0; i < 256; i++)
            {
                List<byte> entry = new() { (byte)i };
                codeTable[entry] = i;
            }
        }
        public void Encode()
        {
            using (FileWriter output = new(outputFile))
            using (BinaryReader input = new FileLoader().LoadFileBinaryReader(inputFile))
            {
                List<byte> sequence = new();
                while (input.BaseStream.Position != input.BaseStream.Length)
                {
                    byte readByte = input.ReadByte();
                    sequence.Add(readByte);
                    if (!codeTable.ContainsKey(sequence))
                    {
                        // add new code to codeTable
                        codeTable.Add(new List<byte>(sequence), codeTable.Count);

                        // output the word
                        sequence.RemoveAt(sequence.Count - 1);
                        output.WriteBits(codeTable[sequence], wordSizeInBits);
                        sequence = new() { readByte };

                        // change bit number, if the table is filled
                        if (codeTable.Count > (int)Math.Pow(2, wordSizeInBits))
                        {
                            wordSizeInBits++;
                        }
                    }
                }
                if (sequence.Count != 0)
                {
                    // output the remaining sequence
                    output.WriteBits(codeTable[sequence], wordSizeInBits);
                }
            }
        }
    }
}
