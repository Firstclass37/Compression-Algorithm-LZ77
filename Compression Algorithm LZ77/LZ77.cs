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

            FirstStep();
                while (this.buffer.GetValue != string.Empty)
            {
                if (!dictionary.GetValue.Contains(buffer.GetValue[0])) StepWOMatch();
                else
                {
                    string matchString = GetMatchString();
                    int matchLenght = GetMatchLength(matchString);
                    int matchPosition = GetMahtPosition(matchString);
                    StepWMatchs(matchLenght,matchPosition);
                }


            }

            return GetResult();
        }

        private void StepWMatchs(int matchLength, int matchPosition)
        {

            if (this.bufferSize == matchLength && currentString != string.Empty)
            {
                resultTable.Add(new CompressionNode(matchPosition, matchLength, this.currentString[0]));
                dictionary.Add(buffer.GetValue + this.currentString[0]);
                buffer.Remove(0, 8);
                currentString = currentString.Remove(0, 1);                
            }
            else
            {
                resultTable.Add(new CompressionNode(matchPosition, matchLength, buffer.GetValue[matchLength]));
                dictionary.Add(buffer.GetValue.Substring(0, matchLength + 1));
                this.buffer.Remove(0, matchLength + 1);
            }

            if (this.currentString.Length > matchLength)
            {
                buffer.Add(this.currentString.Substring(0, matchLength));
                this.currentString = this.currentString.Remove(0, matchLength);
            }
            else
            {
                buffer.Add(this.currentString);
                this.currentString = string.Empty;
            }

        }




        private int GetMahtPosition(string matchString)
        {
            int tempIndex = dictionary.GetValue.LastIndexOf(matchString);
            string tempDictionary = dictionary.GetValue.Substring(tempIndex);
            return tempDictionary.Length;
        }

        private int GetMatchLength(string matchString)
        {
            int matchLenght = matchString.Length;
            string tempBuffer = buffer.GetValue.Remove(0,matchString.Length);

            while ( tempBuffer.Length >= matchString.Length && matchString == tempBuffer.Substring(0, matchString.Length) )
            {
                matchLenght += matchLenght;
                tempBuffer = tempBuffer.Remove(0,matchString.Length);
            }

            return matchLenght;


          

        }

        private string GetMatchString()
        {
            string matchString = string.Empty;
            int lenght = 1;
            while (lenght <= buffer.GetValue.Length && dictionary.GetValue.Contains(this.buffer.GetValue.Substring(0, lenght)))
            {
                matchString = this.buffer.GetValue.Substring(0, lenght);
                lenght++;
            }
            return matchString;

        }

        private void StepWOMatch()
        {
            this.dictionary.Add(buffer.GetValue[0]);
            resultTable.Add(new CompressionNode(0,0, buffer.GetValue[0]));
            this.buffer.Remove(0,1);
            if (this.currentString != string.Empty)
            {
                buffer.Add(currentString[0]);
                this.currentString = this.currentString.Remove(0, 1);

            }
        }

        public void FirstStep()
        {
            if (this.currentString.Length <= this.bufferSize)
            {
                this.buffer.Add(currentString);
                currentString = string.Empty;
            }
            else
            {
                string temp = currentString.Substring(0,this.bufferSize);
                this.buffer.Add(temp);
                this.currentString = this.currentString.Remove(0, this.bufferSize);
            }

        }



        private string GetResult()
        {
            string result = string.Empty;

            foreach (var item in resultTable)
                result += item.ToString();

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
