using System;
using System.Collections.Generic;

namespace LZWArchiver
{
    static class Program
    {
        static void Main(string[] args)
        {
            new Encoder("tst.txt", "out.zip").Encode();
            new Decoder("out.zip", "tstR.txt").Decode();
        }
    }
}
            
