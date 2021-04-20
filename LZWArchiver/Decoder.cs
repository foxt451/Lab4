using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LZWArchiver
{
    public class Decoder
    {
        // codeTable stores corresponding codes for (possibly repeated) sequences of bytes
        private CodeTableDecoder codeTable = new();
        // variable-length codes for words (sequences of bytes) are used
        // if, at first, the length of the codes is 8 bits per code
        // then, each time the size of the dictionary exceeds 2^wordSizeInBits
        // we add 1 additional bit to the size of the codes

        private string inputFile, outputFile;
        private Encoding encoding;

        public Decoder(string inputFile, string outputFile, Encoding encoding)
        {
            this.inputFile = inputFile;
            this.outputFile = outputFile;
            this.encoding = encoding;
        }

        public void Decode()
        {
            // FileWriter provides us with a possibility to write not only whole bytes,
            // but, also, any number of bits
            using (FileReader input = new FileReader(inputFile))
            {
                while(input.HasBits(8))
                {
                    codeTable = new();
                    (long numberBytes, string nameFile) = ReadNameAndType.ReadNAT(input, encoding);
                    outputFile = nameFile;
                    //Console.WriteLine(numberBytes + " "+nameFile);

                    using (FileWriter output = new(outputFile))
                    {
                        // current sequence of read bytes
                            int readByte = input.ReadBits(codeTable.wordSizeInBits);
                            List<byte> sequence = codeTable[readByte];
                            //var oBuf = new List<byte>(windw);
                            foreach (var currentByte in sequence)
                            {
                                output.WriteBits(currentByte, 8);
                            }

                            List<byte> previousSequence = sequence;
                            int decodedBytes = 1;
                            // read until end of file
                            codeTable.UpdateWordSize(0, 0);
                            while (input.HasBits(codeTable.wordSizeInBits) && decodedBytes < numberBytes)
                            {
                                bool reset = codeTable.UpdateWordSize(0, 0);
                                readByte = input.ReadBits(codeTable.wordSizeInBits);
                                if (!codeTable.ContainsKey(readByte))
                                {
                                    sequence = new(previousSequence)
                                    {
                                        previousSequence[0]
                                    };
                                }
                                else
                                {
                                    sequence = codeTable[readByte];
                                }

                                foreach (var seq in sequence)
                                {
                                    output.WriteBits(seq, 8);

                                    // debug
                                    decodedBytes++;
                                    Console.WriteLine(decodedBytes);
                                    if (decodedBytes % (1024 * 1024) == 0)
                                    {
                                        Console.WriteLine($"Decoded {decodedBytes / (1024 * 1024)} MBs");
                                    }
                                }

                                if (!reset)
                                {
                                    List<byte> newSequence = new List<byte>(previousSequence);
                                    newSequence.Add(sequence[0]);
                                    codeTable.Add(newSequence);
                                }

                                previousSequence = sequence;
                                //foreach (var seq in codeTable[readByte])
                                //{
                                //sequence.Add(seq);
                                //}
                                // if the newly obtained sequence doesn't have its own code yet
                            }
                        }
                    

                    Console.WriteLine("Decoding complete!");

                    // when we have reached EOF, we still might have some remaining sequence of bytes
                }
            }
        }
    }
}