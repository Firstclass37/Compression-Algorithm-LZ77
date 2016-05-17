using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compression_Algorithm_LZ77
{
    class Buffer
    {
        private string buffer;
        private int maxSize;

        public string GetValue
        {
            get
            {
                return this.buffer;
            }

        }

        public Buffer(int size)
        {
            buffer = string.Empty;
            this.maxSize = size;
        }

        public void Add(string input)
        {
            if ((buffer.Length + input.Length) <= maxSize)
            {
                buffer = buffer + input;
            }
            else
            {
                int difference = (buffer.Length + input.Length) - maxSize;
                buffer = buffer.Remove(0, difference) + input;
            }
        }

        public void Add(char input)
        {
            if ((buffer.Length + 1) <= maxSize)
            {
                buffer = buffer + input.ToString();
            }
            else
            {
                buffer = buffer.Remove(0, 1) + input.ToString();
            }
        }

        public void Remove(int index, int count)
        {
            this.buffer = buffer.Remove(index,count); 
        }

        public bool IsFull()
        {
            if (buffer.Length < maxSize) return false;
            else return true;
        }
    }
}
