using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZWArchiver
{
    class CodeTableEncoder : CodeTable<List<byte>, int>
    {

        public override void InitCodeTable()
        {
            codeTable = new(new ListComparer<byte>());
            for (int i = 0; i < 256; i++)
            {
                List<byte> entry = new() { (byte)i };
                codeTable[entry] = i;
            }
        }

        public void Add(List<byte> sequence)
        {
            codeTable.Add(sequence, codeTable.Count);
        }
    }
}
