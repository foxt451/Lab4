﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LZWArchiver
{
    public class Decoder
    {
        // codeTable stores corresponding codes for (possibly repeated) sequences of bytes
        private Dictionary<int, List<byte>> codeTable = new Dictionary<int, List<byte>>();

        // variable-length codes for words (sequences of bytes) are used
        // if, at first, the length of the codes is 8 bits per code
        // then, each time the size of the dictionary exceeds 2^wordSizeInBits
        // we add 1 additional bit to the size of the codes
        private int wordSizeInBits = 8;

        private string inputFile, outputFile;
        public Decoder(string inputFile, string outputFile)
        {
            this.inputFile = inputFile;
            this.outputFile = outputFile;

            // we initialize our codeTable with codes for all 256 bytes
            // the codeTable will be expanded later, during the runtime
            for (int i = 0; i < 256; i++)
            {
                List<byte> entry = new List<byte>() { (byte)i };
                codeTable[i] = entry;
                //codeTable[i].AddRange(Convert.ToString(i, 2).PadRight(8, '0').ToCharArray()
                    //.Select(c => (byte) Char.GetNumericValue(c)));

            }
        }

        public void Decode()
        {
            // FileWriter provides us with a possibility to write not only whole bytes,
            // but, also, any number of bits
            using (FileWriter output = new (outputFile))
            using (FileReader input = new FileReader(inputFile))
            {
                // current sequence of read bytes
                int readByte = input.ReadBits(wordSizeInBits);
                List<byte> sequence = codeTable[readByte];
                //var oBuf = new List<byte>(windw);
                foreach (var currentByte in sequence)
                {
                    output.WriteBits(currentByte, 8);
                }

                // read until end of file
                List<byte> previousSequence = sequence;
                while (input.HasBits(wordSizeInBits))
                {
                    readByte = input.ReadBits(wordSizeInBits);
                    sequence = codeTable[readByte];
                    foreach (var seq in sequence)
                    {
                        output.WriteBits(seq, wordSizeInBits);
                    }
                    List<byte> newSequence = new List<byte>(previousSequence);
                    newSequence.Add(sequence[0]);
                    //foreach (var seq in codeTable[readByte])
                    //{
                        //sequence.Add(seq);
                    //}
                    // if the newly obtained sequence doesn't have its own code yet
                    if (!codeTable.ContainsKey(readByte))
                    {
                        // add new code to codeTable
                        codeTable.Add(new List<byte>(sequence), codeTable.Count);

                        // output the word (without the last read byte)
                        sequence.RemoveAt(0);
                        output.WriteBits(codeTable[sequence], wordSizeInBits);
                        // set the sequence to contain only the last read byte
                        sequence = new () {readByte};

                        // change bit number, if the table is filled
                        // so that we have enough of bits to write all codes in the table
                        if (codeTable.Count > (int) Math.Pow(2, wordSizeInBits))
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
                    //sequence = new List<byte>();
                }
            }
        }
    }
}