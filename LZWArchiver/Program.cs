using System;
using System.Collections.Generic;

namespace LZWArchiver
{
    static class Program
    {
        static void Main(string[] args)
        {
            new Encoder("big.bmp", "big.zip").Encode();
            new Decoder("big.zip", "bigR.bmp").Decode();
        }
    }
}
            
