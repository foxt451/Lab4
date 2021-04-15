using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZWArchiver
{
    abstract class CodeTable<KeyType, ValueType>
    {
        public int wordSizeInBits 
        {
            get;
            protected set;
        } = 8;
        protected int maxWordSize = 16;

        protected Dictionary<KeyType, ValueType> codeTable;

        public CodeTable()
        {
            InitCodeTable();
        }

        // we initialize our codeTable with codes for all 256 bytes
        // the codeTable will be expanded later, during the runtime
        public abstract void InitCodeTable();
        public bool ContainsKey(KeyType key) => codeTable.ContainsKey(key);
        public ValueType this[KeyType i]
        {
            get => codeTable[i];
        }

        // we may want to update the code size not immediately, but, for example, 1 iteration after
        // delay tells how many iterations later an update will take place

        // returns true if the table was reset
        // bool value needed for decoder

        private void ResetCodeTable()
        {
            InitCodeTable();
            wordSizeInBits = 8;
        }

        public bool UpdateWordSize(int wordSizeUpdateDelay=0, int tableResetDelay = 0)
        {
            // change bit number, if the table is filled
            // so that we have enough of bits to write all codes in the table
            if (codeTable.Count - wordSizeUpdateDelay >= (int)Math.Pow(2, wordSizeInBits))
            {
                wordSizeInBits++;
            }
            if (codeTable.Count - tableResetDelay >= (int)Math.Pow(2, maxWordSize))
            {
                // reset codetable
                ResetCodeTable();
                return true;
            }
            return false;
        }
    }
}
