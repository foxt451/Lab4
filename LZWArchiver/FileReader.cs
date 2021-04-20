using System;
using System.IO;

namespace LZWArchiver
{

        public class FileReader : IDisposable
    {
        private BinaryReader reader;

        public FileReader(string filePath)
        {
            reader = new FileLoader().LoadFileBinaryReader(filePath);
        }


        // in cases when we want to write only 3 bits to file
        // or 12 bits.. we can't do it, because it can't be split into bytes
        // we could write, for example, three bits 110 as 00000110, but
        // it would consume a lot of memory
        // this class handles this

        // in case we have some remaining bits that don't make up a whole byte
        // we can store them in this byte, remembering how much we have of them in filledBits
        // on further writings this byte will eventually get complete and written down to the stream
        private bool disposedValue = false;
        private byte previous = 0;
        private int saveBits = 0;

        public void ResetSaved()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException("object is disposed");
            }
            previous = 0;
            saveBits = 0;
        }

        public int ReadBits(int numberOfBits)
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException("object is disposed");
            }

            int result = 0;
            if (saveBits > 0)
            {
                result |= previous;
                numberOfBits -= saveBits;
                if (numberOfBits >= 0)
                {
                    result = result << numberOfBits;
                    previous = 0x00;
                    saveBits = 0;
                }
                else if(numberOfBits < 0)
                {
                    result = result >> -numberOfBits;
                    previous |= (byte)(previous & (0xFF >> (8+numberOfBits)));
                    saveBits = -numberOfBits;
                }
            }
            while (numberOfBits > 0)
            {
                int a = reader.ReadByte();
                numberOfBits -= 8;
                if (numberOfBits>0)
                {
                    a = a << numberOfBits;
                }

                if (numberOfBits < 0)
                {
                    previous |= (byte)(a & (0xFF >> (8+numberOfBits)));
                    saveBits = -numberOfBits;
                    a = a >> (-numberOfBits);
                }
                result|=a;

            }

            return result;
        }

        public long GetBytesLeftInFile()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException("object is disposed");
            }
            return reader.BaseStream.Length - reader.BaseStream.Position;
        }

        public long GetBytesTotalInFile()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException("object is disposed");
            }
            return reader.BaseStream.Length;
        }

        public long Read64()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException("object is disposed");
            }
            return reader.ReadInt64();
        }

        public int Read32()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException("object is disposed");
            }
            return reader.ReadInt32();
        }

        public byte Read8()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException("object is disposed");
            }
            return reader.ReadByte();
        }

        public bool HasBits(int NumberOfBits)
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException("object is disposed");
            }
            long leftBytes = GetBytesLeftInFile();
            if (leftBytes*8 + saveBits >= NumberOfBits)
            {
                return true;
            }
            else
            {
                return false;
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
                    reader.Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
    
}