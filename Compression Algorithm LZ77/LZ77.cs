using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compression_Algorithm_LZ77
{
    class LZ77
    {
        private int dictionarySize;
        private int bufferSize;
        private Buffer buffer;
        private Dictionary dictionary;
        private List<CompressionNode> resultTable;
        private string currentString;
        
        public int DictionarySize
        {
            get
            {
                return dictionarySize;
            }
            set
            {
                dictionarySize = value > 50 ? value : 50;
            }

        }
        public int BufferSize
        {
            get
            {
                return bufferSize;
            }
            set
            {
                bufferSize = (value == 8 || value == 16 || value == 32) ? value : 8;
            }
        }

        public LZ77()
        {
            dictionarySize = 50;
            bufferSize = 8;
        }


        public string Compression(string input)
        {
            SetStartState(input);
            RefillBuffer();
            while (this.buffer.GetValue != string.Empty)
            {   
                int matchPosition;
                int matchLenght;
                FindMatch(out matchPosition,out matchLenght);
                AddResult(matchPosition,matchLenght,buffer.GetValue[matchLenght]);
                RefillDictionary(matchLenght+1);
                RefillBuffer();
            }
            return GetResult();
        }

        private void FindMatch(out int position, out int lenght)
        {
            position = 0;
            lenght = 0;
            string match = string.Empty;
            string tempMatch;

            for (int i = 1; i < bufferSize; i++)
            {
                tempMatch = buffer.GetValue.Substring(0, i);
                if (dictionary.GetValue.Contains(tempMatch))
                {
                    match = tempMatch;
                    lenght++;
                }
                else
                {
                    break;
                }
            }

            if (lenght > 1)
            {
                position = GetMahtPosition(match);
            }
            else
            {
                lenght = 0;
            }

        }

        private void RefillBuffer()
        {
            while (!buffer.IsFull() && currentString != string.Empty)
            {
                buffer.Add(currentString[0]);
                currentString = currentString.Remove(0, 1);
            }

        }

        private void RefillDictionary(int lenght)
        {
            dictionary.Add(buffer.GetValue.Substring(0,lenght));
            buffer.Remove(0,lenght);
        }

        private void AddResult(int position, int lenght,char c)
        {
            resultTable.Add(new CompressionNode(position,lenght,c));
        }

        private int GetMahtPosition(string matchString)
        {
            int tempIndex = dictionary.GetValue.LastIndexOf(matchString);
            string tempDictionary = dictionary.GetValue.Substring(tempIndex);
            return tempDictionary.Length;
        }


        private string GetResult()
        {
            string result = string.Empty;

            foreach (var item in resultTable)
            {
                if (item.I == 0) result += item.Symbol.ToString();
                else
                {
                    result += item.ToString();
                }
            }
            return result;
        }

        private void SetStartState(string input)
        {
            this.currentString = input;
            buffer = new Buffer(bufferSize);
            dictionary = new Dictionary(dictionarySize);
            resultTable = new List<CompressionNode>();
        }


    }
}
