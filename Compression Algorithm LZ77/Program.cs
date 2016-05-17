using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Compression_Algorithm_LZ77
{

    enum Action
    {
        Compress,
        Decompress,

        Unknown
    }

    class Program
    {
        static void Main(string[] args)
        {

            string inputString = string.Empty;
            do
            {
                LZ77 lz = new LZ77();
                Console.Clear();
                if (YNDialog("Do you want change dictionary size, buffer size  and min match lenght?"))
                {
                    lz.DictionarySize = GetValueDialog("Input dictionary size (min 100 max 15000)");
                    lz.BufferSize = GetValueDialog("Input buffer Size( min 8 max 64)");   
                    lz.MinMatchLenght = GetValueDialog("Input min match lenght (  min 2 , < buffer size)");
                    Message(String.Format("dictionarySize = {0}  bufferSize = {1}  min match lenght = {2}",lz.DictionarySize,lz.BufferSize,lz.MinMatchLenght),ConsoleColor.Green);
                    Console.ReadKey();
                    Console.Clear();
                }

                Action currentAction = ActionChoiceDialog();
                string path;  
                switch (currentAction)
                {
                    case Action.Compress: path = CommandDialog("path for compress: ");
                        break;
                    case Action.Decompress: path = CommandDialog("path for decompress: ");
                        break;
                    default: Message("Unknown opertion",ConsoleColor.Red);
                        continue;
                }
                if (string.IsNullOrEmpty(path)) continue;


                string targetString;
                if (File.Exists(path))
                {
                    targetString = GetTextFromPath(path);
                }
                else
                {
                    Message("FileNotFound!!", ConsoleColor.Red);
                    Console.ReadKey();
                    continue;
                }

                Message(string.Format("target string lenght: {0}",targetString.Length),ConsoleColor.Green);

                string result;
                switch (currentAction)
                {
                    case Action.Compress:
                        Message("In process...", ConsoleColor.DarkYellow);
                        result = lz.Compression(targetString);  
                        Saveresult(path,result,currentAction);     
                        break;
                    case Action.Decompress:
                        Message("In process...", ConsoleColor.DarkYellow);
                        result = lz.Decompression(targetString);
                        Saveresult(path, result, currentAction);
                        break;
                    default:
                        Message("Unknown opertion", ConsoleColor.Red);
                        continue;
                }

                Message(string.Format("result string lenght: {0}", result.Length), ConsoleColor.Green);
                Message("Complete!!!",ConsoleColor.Green);
                

            } while (YNDialog("Continue?"));
                    

        }

        private static Action ActionChoiceDialog()
        {
            Console.Write("Chose the action: 1-Compress 2-Decompres");
            var command = Console.ReadKey().KeyChar;
            Console.WriteLine();
            switch (command)
            {
                case '1':
                    return Action.Compress;
                case '2':
                    return Action.Decompress;
                default:
                    return Action.Unknown;
            }
            

        }

        private static bool YNDialog(string question)
        {
            Console.Write(question + " " + "(y/n)");
            if (Console.ReadKey().KeyChar == 'y')
            {
                Console.WriteLine();
                return true;
            }
            Console.WriteLine();
            return false;
        }

        private static void Message(string message,ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static string CommandDialog(string message)
        {
            Console.Write(message + " ");
            string result = Console.ReadLine();
            Console.WriteLine();
            return result;
        }

        private static int GetValueDialog(string question)
        {
            int result;
            Console.Write(question + " ");
            int.TryParse(Console.ReadLine(),out result);
            Console.WriteLine();
            return result;          
        }

        private static string GetTextFromPath(string path)
        {            
           return  File.ReadAllText(path);                   
        }

        private static void Saveresult(string path,string result,Action action)
        {
            FileInfo currentFile = new FileInfo(path);
            string saveName = "(comppressed)" + currentFile.Name;
            switch (action)
            {
                 case Action.Compress:
                    saveName = "(comppressed)" + currentFile.Name;
                    break;
                case Action.Decompress:
                    saveName = "(decomppressed)" + currentFile.Name;
                    break;
            }
            string savePath = currentFile.DirectoryName + "\\";
            string saveFuulName = savePath + saveName;
            File.AppendAllText(saveFuulName,result);
        }


    }
}
