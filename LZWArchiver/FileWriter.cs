using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZWArchiver
{
    class FileWriter
    {
        // in cases when we want to write only 3 bits to file
        // or 12 bits.. we can't do it, because it can't be split into bytes
        // we could write, for example, three bits 110 as 00000110, but
        // it would consume a lot of memory
        // this class handles this

        // in case we have some remaining bits that don't make up a whole byte
        // we can store them in this byte, remembering how much we have of them in filledBits
        // on further writings this byte will eventually get complete and written down to the stream
        byte previous = 0x00;
        int filledBits = 0;
        public void WriteBits(int a, int numberOfBits)
        {

            while (numberOfBits != 0)
            {
                // if we have some bits left in previous, then filledBits will not be 0
                // (btw, the remaning bits are located on the left, like 110(00000) for three bits 110)
                int remainingBits = 8 - filledBits;
                if (numberOfBits >= remainingBits)
                {
                    previous |= (byte)(a >> (numberOfBits - remainingBits) & (0xFF >> (filledBits)));
                    // update 'a', 'previous' and output the byte
                    a >>= (numberOfBits - remainingBits);
                    numberOfBits -= remainingBits;
                    Console.WriteLine($"Output byte {previous}");
                    previous = 0x00;
                    filledBits = 0;
                }
                else
                {
                    previous |= (byte)(a << (remainingBits - numberOfBits) & (0xFF >> (filledBits)));
                    filledBits += numberOfBits;
                    Console.WriteLine($"This was not enough to output the whole byte... Completed by {numberOfBits} bits");
                    numberOfBits = 0;
                }
            }
        }
    }
}
