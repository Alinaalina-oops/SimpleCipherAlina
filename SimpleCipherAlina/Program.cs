using System;
using System.IO;

namespace SimpleCipherAlina
{
    class Program
    {
        public static bool KeyFileExists = false;
        public static bool FileInputExists = false;
        public static bool FileOutputExists = false;
        private static ReplaceCipher cipher = new ReplaceCipher();

        public static void Main(string[] args)
        {
            try
            {
                switch (args[0])
                {
                    case "encrypt":
                        Encrypt(args);
                        break;
                    case "decrypt":
                        Decrypt(args);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникла непредвиденная ошибка:" + ex.Message);
            }
        }

        private static void Decrypt(string[] args)
        {
            try
            {
                var parameters = ForEachByArgs(args);

                var inputOutputData = CheckEmptyParams(parameters.Item2, parameters.Item3);

                var resultOfDecrypt = cipher.Decrypt(parameters.Item1, inputOutputData.Item1);

                WriteResult(resultOfDecrypt, inputOutputData.Item2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        private static void Encrypt(string[] args)
        {
            try
            {
                var parameters = ForEachByArgs(args);

                var inputOutputData = CheckEmptyParams(parameters.Item2, parameters.Item3);

                var resultOfEncrypt = cipher.Encrypt(parameters.Item1, inputOutputData.Item1);
                
                WriteResult(resultOfEncrypt, inputOutputData.Item2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        private static void WriteResult(string resultOfEncrypt, string item2)
        {
            if (FileOutputExists)
            {
                File.WriteAllText(item2, resultOfEncrypt);
            }
            else
            {
                Console.WriteLine(resultOfEncrypt);
            }
        }

        private static (string, string) CheckEmptyParams(string input, string output)
        {
            string resultInput = null;
            if (FileInputExists)
            {
                resultInput = File.ReadAllText(input);
            }
            else
            {
                Console.WriteLine("Введите шифруемый текст");
                resultInput = Console.ReadLine();
            }

            return (resultInput, output);
        }

        private static (string, string, string) ForEachByArgs(string[] args)
        {
            string fileInput = null;
            string fileOutput = null;
            string fileWithKey = null;

            for (int i = 1; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-k":
                        KeyFileExists = true;
                        i++;
                        fileWithKey = args[i];
                        break;
                    case "-i":
                        FileInputExists = true;
                        i++;
                        fileInput = args[i];
                        break;
                    case "-o":
                        FileOutputExists = true;
                        i++;
                        fileOutput = args[i];
                        break;
                    case "-r":
                        cipher.DeleteUnnesessarySymbols = true;
                        break;
                    case "-c":
                        cipher.IngoreCase = true;
                        break;
                    case "-u":
                        cipher.UpperCase = true;
                        break;
                    case "-l":
                        cipher.LowerCase = true;
                        break;
                    default:
                        var message = $"Нарушены правила ввода, неизвестная команда = {args[i]}";
                        throw new Exception(message);
                }
            }

            return (fileWithKey, fileInput, fileOutput);
        }
    }
}
