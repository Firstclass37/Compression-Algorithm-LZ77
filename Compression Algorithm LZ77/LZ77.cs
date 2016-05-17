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

        /// <summary>
        /// Method compress target string and return new compressed string
        /// </summary>
        /// <param name="inputString"></param>
        public string Compression(string inputString)
        {
            SetStartState(inputString);
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
        /// <summary>
        /// Find match and return position and lenght
        /// </summary>
        /// <param name="position"></param>
        /// <param name="lenght"></param>
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
        /// <summary>
        /// Method add symbols in buffer from current string
        /// </summary>
        private void RefillBuffer()
        {
            while (!buffer.IsFull() && currentString != string.Empty)
            {
                buffer.Add(currentString[0]);
                currentString = currentString.Remove(0, 1);
            }

        }
        /// <summary>
        /// Method refill dictionary from buffer for input lenght
        /// </summary>
        /// <param name="lenght"></param>
        private void RefillDictionary(int lenght)
        {
            dictionary.Add(buffer.GetValue.Substring(0,lenght));
            buffer.Remove(0,lenght);
        }
        /// <summary>
        /// Add current result in result table
        /// </summary>
        /// <param name="position"></param>
        /// <param name="lenght"></param>
        /// <param name="c"></param>
        private void AddResult(int position, int lenght,char c)
        {
            resultTable.Add(new CompressionNode(position,lenght,c));
        }
        /// <summary>
        /// Method return a position for input match
        /// </summary>
        /// <param name="matchString"></param>
        /// <returns></returns>
        private int GetMahtPosition(string matchString)
        {
            int tempIndex = dictionary.GetValue.LastIndexOf(matchString);
            string tempDictionary = dictionary.GetValue.Substring(tempIndex);
            return tempDictionary.Length;
        }
        /// <summary>
        /// Convert results table in a string and return result as string
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Method decompress input string and return new decompressed string
        /// </summary>
        /// <param name="targetString"></param>
        /// <returns></returns>
        public string Decompression(string targetString)
        {
            SetStartState(targetString);

            

            while (currentString.Contains("["))
            {
                int indexLeft = currentString.IndexOf("[");
                int indexRight = currentString.IndexOf("]");
                string tempMatch = currentString.Substring(indexLeft,indexRight - indexLeft + 1);
                string[] par = tempMatch.Remove(0, 1).Remove(tempMatch.Length - 2, 1).Split(',');
                int pos = int.Parse(par[0]);
                int len = int.Parse(par[1]);
                string matchString = GetMatch(indexLeft-pos,len);
                currentString = currentString.Remove(indexLeft, indexRight - indexLeft + 1).Insert(indexLeft,matchString);
            }

            return currentString;

        }
        /// <summary>
        /// Method return match string from input position and have input leght
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        private string GetMatch(int pos, int len)
        {
            string result = currentString.Substring(pos,len);
            return result;
        }


        /// <summary>
        /// Method set start setting
        /// </summary>
        /// <param name="input"></param>
        private void SetStartState(string input)
        {
            this.currentString = input;
            buffer = new Buffer(bufferSize);
            dictionary = new Dictionary(dictionarySize);
            resultTable = new List<CompressionNode>();
        }


    }
}
