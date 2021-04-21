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
        private CodeTableEncoder codeTable = new CodeTableEncoder();

        // variable-length codes for words (sequences of bytes) are used
        // if, at first, the length of the codes is 8 bits per code
        // then, each time the size of the dictionary exceeds 2^wordSizeInBits
        // we add 1 additional bit to the size of the codes
        private readonly string outputFile;
        private readonly string[] inputFiles;
        private readonly Encoding encoding;

        
        public Encoder(string[] inputFiles, string outputFile, Encoding encoding)
        {
            this.inputFiles = inputFiles;
            this.outputFile = outputFile;
            this.encoding = encoding;
        }
        public void Encode()
        {
            // FileWriter provides us with a possibility to write not only whole bytes,
            // but, also, any number of bits
            using (FileWriter output = new(outputFile, FileMode.Create))
            {
                foreach (string inputLocation in inputFiles)
                {
                    codeTable = new();
                    output.ResetSaved();
                    bool isDir = false;
                    List<string> inputFiles = new();
                    DirectoryInfo dir = null;
                    // it's a folder
                    if (!inputLocation.Contains("."))
                    {
                        isDir = true;
                        dir = new(inputLocation);
                        // init inputFiles
                        foreach (FileInfo dirFile in dir.GetFiles())
                        {
                            inputFiles.Add(dirFile.Name);
                        }
                        WriteNameAndType.WriteDir(dir, encoding, output);
                    }
                    // not a folder
                    else
                    {
                        inputFiles.Add(inputLocation);
                    }

                    foreach (string encodedFile in inputFiles)
                    {
                        codeTable = new();
                        output.ResetSaved();
                        FileInfo file = new(encodedFile);
                        using (FileReader input = isDir ? new(encodedFile, dir.Name) : new(encodedFile))
                        {
                            if (isDir)
                            {
                                WriteNameAndType.WriteNAT(dir.FullName + "\\" + encodedFile, encoding, output);
                            } else
                            {
                                WriteNameAndType.WriteNAT(encodedFile, encoding, output);
                            }
                            // current sequence of read bytes
                            List<byte> sequence = new();
                            // read until end of file
                            int bytesRead = 0;
                            while (input.HasBits(8))
                            {
                                byte readByte = (byte)input.ReadBits(8);
                                // debug
                                bytesRead++;
                                if (bytesRead % (1024 * 1024) == 0)
                                {
                                    Console.WriteLine($"Read {bytesRead / (1024 * 1024)} MBs ({(float)bytesRead / input.GetBytesTotalInFile() * 100})%");
                                }
                                // add the read byte to the sequence
                                sequence.Add(readByte);
                                // if the newly obtained sequence doesn't have its own code yet
                                if (!codeTable.ContainsKey(sequence))
                                {
                                    // add new code to codeTable
                                    codeTable.Add(new List<byte>(sequence));

                                    // output the word (without the last read byte)
                                    sequence.RemoveAt(sequence.Count - 1);
                                    output.WriteBits(codeTable[sequence], codeTable.wordSizeInBits);
                                    // set the sequence to contain only the last read byte
                                    sequence = new() { readByte };

                                    codeTable.UpdateWordSize(1, 1);
                                }
                            }

                            // when we have reached EOF, we still might have some remaining sequence of bytes 
                            if (sequence.Count != 0)
                            {
                                // output the remaining sequence
                                output.WriteBits(codeTable[sequence], codeTable.wordSizeInBits);
                            }

                            Console.WriteLine("Encoded!");
                        }
                    }
                }
            }
        }
    }
}
