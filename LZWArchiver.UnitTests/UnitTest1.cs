using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LZWArchiver.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        // basic writing
        [TestMethod]
        public void BasicWriting()
        {
            using (var writer = new FileWriter("test.bin"))
            {
                // 1 byte together: 33
                writer.WriteBits(16, 7);
                writer.WriteBits(1, 1);

                // 6 bits together: 010100(xx)
                writer.WriteBits(2, 3);
                writer.WriteBits(1, 1);
                writer.WriteBits(4, 2);

                // fills the previous byte with 00 to 01010000: 80
                // 5 bits are left: 10000
                writer.WriteBits(16, 7);
                // fills the previous byte with 100 to 10000100: 132
                writer.WriteBits(4, 3);
            }
        }
    }
}
