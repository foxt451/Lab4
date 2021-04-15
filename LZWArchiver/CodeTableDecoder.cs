using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZWArchiver
{
    class CodeTableDecoder : CodeTable<int, List<byte>>
    {
        public override void InitCodeTable()
        {
            // we initialize our codeTable with codes for all 256 bytes
            // the codeTable will be expanded later, during the runtime
            for (int i = 0; i < 256; i++)
            {
                List<byte> entry = new() { (byte)i };
                codeTable.Add(i, entry);
            }
        }

        public void Add(List<byte> sequence)
        {
            codeTable.Add(codeTable.Count, sequence);
        }
    }
}
