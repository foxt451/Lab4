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
        // codeTable stores corresponding codes for (possibly repeated) sequences of bytes
        private Dictionary<List<byte>, int> codeTable = new(new ListComparer<byte>());

        // variable-length codes for words (sequences of bytes) are used
        // if, at first, the length of the codes is 8 bits per code
        // then, each time the size of the dictionary exceeds 2^wordSizeInBits
        // we add 1 additional bit to the size of the codes
        private int wordSizeInBits = 8;

        private string inputFile, outputFile;
        public Encoder(string inputFile, string outputFile)
        {
            this.inputFile = inputFile;
            this.outputFile = outputFile;

            // we initialize our codeTable with codes for all 256 bytes
            // the codeTable will be expanded later, during the runtime
            for (int i = 0; i < 256; i++)
            {
                List<byte> entry = new() { (byte)i };
                codeTable[entry] = i;
            }
        }
        public void Encode()
        {
            // FileWriter provides us with a possibility to write not only whole bytes,
            // but, also, any number of bits
            using (FileWriter output = new(outputFile))
            using (BinaryReader input = new FileLoader().LoadFileBinaryReader(inputFile))
            {
                // current sequence of read bytes
                List<byte> sequence = new();
                // read until end of file
                while (input.BaseStream.Position != input.BaseStream.Length)
                {
                    byte readByte = input.ReadByte();
                    // add the read byte to the sequence
                    sequence.Add(readByte);
                    // if the newly obtained sequence doesn't have its own code yet
                    if (!codeTable.ContainsKey(sequence))
                    {
                        // add new code to codeTable
                        codeTable.Add(new List<byte>(sequence), codeTable.Count);

                        // output the word (without the last read byte)
                        sequence.RemoveAt(sequence.Count - 1);
                        output.WriteBits(codeTable[sequence], wordSizeInBits);
                        // set the sequence to contain only the last read byte
                        sequence = new() { readByte };

                        // change bit number, if the table is filled
                        // so that we have enough of bits to write all codes in the table
                        if (codeTable.Count > (int)Math.Pow(2, wordSizeInBits))
                        {
                            wordSizeInBits++;
                        }
                    }
                }

                // when we have reached EOF, we still might have some remaining sequence of bytes 
                if (sequence.Count != 0)
                {
                    // output the remaining sequence
                    output.WriteBits(codeTable[sequence], wordSizeInBits);
                }
            }
        }
    }
}
