using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compression_Algorithm_LZ77
{
    class CompressionNode
    { 
            public int I;
            public int J;
            public char Symbol;

            public CompressionNode(int i, int j, char symbol)
            {
                this.I = i;
                this.J = j;
                this.Symbol = symbol;
            }

        public override string ToString()
        {
            return string.Format("({0},{1}){2}", I, J, Symbol);
        }
    }
}
