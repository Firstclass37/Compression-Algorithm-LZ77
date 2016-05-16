using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Compression_Algorithm_LZ77
{
    class Program
    {
        static void Main(string[] args)
        {

            string inputString = string.Empty;
            do
            {
                LZ77 lz = new LZ77();
                Console.Clear();
                if (YNDialog("Change dictionary and buffer size?"))
                {
                    int dictionarySize = GetValueDialog("Input dictionary size (Lenght>50 ,default: 50)");
                    int bufferSize = GetValueDialog("Input bufferSize( lenght 8-16-32)");
                    
                   
                    Message(String.Format("dictionarySize = {0}  bufferSize = {1}",dictionarySize,bufferSize),ConsoleColor.Green);
                    lz.BufferSize = bufferSize;
                    lz.DictionarySize = dictionarySize;
                    Console.ReadKey();
                    Console.Clear();
                }

                string path = CommandDialog("input path");
                if (string.IsNullOrEmpty(path)) continue;
                string targetString = string.Empty;
                if (File.Exists(path)) targetString = GetTextFromPath(path);
                else
                {
                    Message("FileNotFound!!", ConsoleColor.Red);
                    Console.ReadKey();
                    continue;
                }
                string result = lz.Compression(targetString);
                Saveresult(path, result);
                Message("Complete!!!",ConsoleColor.Green);
                

            } while (YNDialog("Continue?"));
                    

        }

        static bool YNDialog(string question)
        {
            Console.Write(question + " " + "(y/n)");
            if (Console.ReadKey().KeyChar == 'y') return true;
            Console.Write("\n");
            return false;
        }

        static void Message(string message,ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        static string CommandDialog(string message)
        {
            Console.Write(message + " ");
            string result = Console.ReadLine();
            Console.Write("\n");
            return result;
        }

        static int GetValueDialog(string question)
        {
            int result;
            Console.Write(question + " ");
            int.TryParse(Console.ReadLine(),out result);
            return result;          
        }


        static string GetTextFromPath(string path)
        {            
           return  File.ReadAllText(path);                   
        }

        static void Saveresult(string path,string result)
        {
            FileInfo currentFile = new FileInfo(path);
            string saveName = "(comppressed)" + currentFile.Name;
            string savePath = currentFile.DirectoryName;
            string saveFuulName = savePath + saveName;
            File.AppendAllText(saveFuulName,result);
        }


    }
}
