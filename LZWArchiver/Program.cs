using System;

namespace LZWArchiver
{
    static class Program
    {
        static void Main(string[] args)
        {
            Encoder encoder = new("test.bin", "asA");
            FileWriter writer = new FileWriter();
            writer.WriteBits(16, 7);
            writer.WriteBits(1, 1);
            writer.WriteBits(2, 3);
            writer.WriteBits(1, 1);
            writer.WriteBits(4, 2);
            writer.WriteBits(16, 7);
            // bug
            writer.WriteBits(4, 3);
        }
    }
}
            
