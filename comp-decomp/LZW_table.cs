using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace comp_decomp
{
    class LZW_table
    {
            public LZW_table(int numBytesPerCode)
            {
                maxCode = (1 << (8 * numBytesPerCode)) - 1;
            }

            public void AddCode(string s)
            {
                if (nextAvailableCode <= maxCode)
                {
                    if (s.Length != 1 && !table.ContainsKey(s))
                        table[s] = nextAvailableCode++;
                }
                else
                {
                    throw new Exception("LZW string table overflow");
                }
            }

            public int GetCode(string s)
            {
                if (s.Length == 1)
                    return (int)s[0];
                else
                    return table[s];
            }

            public bool Contains(string s)
            {
                return s.Length == 1 || table.ContainsKey(s);
            }

            private Dictionary<string, int> table = new Dictionary<string, int>();
            private int nextAvailableCode = 256;
            private int maxCode;
        }
    
}
