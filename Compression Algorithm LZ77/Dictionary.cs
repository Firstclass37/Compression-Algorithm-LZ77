using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compression_Algorithm_LZ77
{
    class Dictionary
    {
        private string dictionary;
        private int maxSize;
        /// <summary>
        /// Property for get dictionary value as string. Only get 
        /// </summary>
        public string GetValue
        {
            get
            {
                return this.dictionary;
            }

        }

        public Dictionary(int size)
        {
            dictionary = string.Empty;
            this.maxSize = size;
        }
        /// <summary>
        /// Add input string in the dictionary
        /// </summary>
        /// <param name="input"></param>
        public void Add(string input)
        {
            if ((dictionary.Length + input.Length) <= maxSize)
            {
                dictionary = dictionary + input;
            }
            else
            {
                int difference = (dictionary.Length + input.Length) - maxSize;
                dictionary = dictionary.Remove(0, difference) + input;
            }
        }
        /// <summary>
        /// Add input char in the dictionary as string
        /// </summary>
        /// <param name="input"></param>
        public void Add(char input)
        {
            if ((dictionary.Length + 1) <= maxSize)
            {
                dictionary = dictionary + input.ToString();
            }
            else
            {
                dictionary = dictionary.Remove(0,1) + input.ToString();
            }
        }
    }
}
