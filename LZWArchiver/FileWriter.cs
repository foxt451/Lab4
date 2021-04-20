using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace LZWArchiver
{
    public class FileWriter : IDisposable
    {
        private BinaryWriter writer;

        public FileWriter(string filePath, FileMode filemode = FileMode.Create)
        {
            writer = new FileLoader().LoadFileBinaryWriter(filePath, filemode);
        }


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
        private bool disposedValue = false;

        public void WriteBits(int a, int numberOfBits)
        {
            if (disposedValue) return;

            while (numberOfBits != 0)
            {
                // if we have some bits left in previous, then filledBits will not be 0
                // (btw, the remaning bits are located on the left, like 110(00000) for three bits 110)
                int remainingBits = 8 - filledBits;
                if (numberOfBits >= remainingBits)
                {
                    previous |= (byte)(a >> (numberOfBits - remainingBits) & (0xFF >> (filledBits)));
                    // update 'numberOfBits', 'previous' and output the byte
                    numberOfBits -= remainingBits;
                    // Console.WriteLine($"Output byte {previous}");
                    writer.Write(previous);
                    previous = 0x00;
                    filledBits = 0;
                }
                else
                {
                    previous |= (byte)(a << (remainingBits - numberOfBits) & (0xFF >> (filledBits)));
                    filledBits += numberOfBits;
                    // Console.WriteLine($"This was not enough to output the whole byte... Completed by {numberOfBits} bits");
                    numberOfBits = 0;
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // write down previous, even if it's not filled
                    // in that case, we'll have some unnecessary zeroes at the end of the file
                    if (filledBits != 0)
                    {
                        writer.Write(previous);
                    }
                    writer.Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~FileWriter()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
