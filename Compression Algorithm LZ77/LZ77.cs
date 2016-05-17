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
        private int minMatchSize;
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
                dictionarySize = value > 100 ? value : 100;
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
                bufferSize = value >8  && value < 64 ? value : 8;
            }
        }
        public int MinMatchLenght
        {
            get { return minMatchSize; }
            set { minMatchSize = value > 2 && value < bufferSize ? value : 2; }
        }

        public LZ77()
        {
            dictionarySize = 50;
            bufferSize = 8;
            minMatchSize = 2;
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
            
            for (int i = 1; i < buffer.GetValue.Length; i++)
            {
                string tempMatch = buffer.GetValue.Substring(0, i);
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

            if (lenght >= minMatchSize)
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


        public string Decompression(string targetString)
        {
            SetStartState(targetString);

            

            while (currentString.Contains("("))
            {
                int indexLeft = currentString.IndexOf("(");
                int indexRight = currentString.IndexOf(")");
                string tempMatch = currentString.Substring(indexLeft,indexRight - indexLeft + 1);
                string[] par = tempMatch.Remove(0, 1).Remove(tempMatch.Length - 2, 1).Split(',');
                int pos = int.Parse(par[0]);
                int len = int.Parse(par[1]);
                string matchString = GetMatch(indexLeft-pos,len);
                currentString = currentString.Remove(indexLeft, indexRight - indexLeft + 1).Insert(indexLeft,matchString);
            }

            return currentString;

        }

        private string GetMatch(int pos, int len)
        {
            string result = currentString.Substring(pos,len);
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
