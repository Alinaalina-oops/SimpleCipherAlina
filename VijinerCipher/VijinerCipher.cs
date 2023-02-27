using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace VijinerCipher
{
    public class VijinerCipher
    {
        public bool DeleteUnnesessarySymbols = false;
        public bool IngoreCase = false;
        public bool UpperCase = false;
        public bool LowerCase = false;
        private List<int> keys = null;
        private string Alphabet = string.Empty;

        public string Encrypt(string keyFilePath, string alphabetFilePath, string inputText)
        {
            StringBuilder result = new StringBuilder();
            GetKeyForEncrypt(keyFilePath);
            GetAlphabet(alphabetFilePath);
            return Crypt(inputText, result);
        }

        public string Decrypt(string keyFilePath, string alphabetFilePath, string inputText)
        {
            StringBuilder result = new StringBuilder();
            GetKeyForEncrypt(keyFilePath);
            GetAlphabet(alphabetFilePath);
            return Decrypt(inputText, result);
        }

        private string Crypt(string inputText, StringBuilder result)
        {
            int index = 0;
            for (int i = 0; i < inputText.Length; i++)
            {
                var currentChar = GetCurrentCharForKey(inputText[i]);
                if (keys.IndexOf(currentChar) != -1)
                {
                    //ci = (pi + ki) mod N, pi - символ исходного
                    //ki - символ ключа
                    int pi = (int)currentChar;
                    int ki = keys[i];
                    var resultChar = ((pi + ki) % (int)Alphabet.Length);
                    result.Append(Alphabet[resultChar]);

                    index++;
                    if (index >= keys.Count)
                    {
                        index = 0;
                    }
                }
                else
                {
                    if (!DeleteUnnesessarySymbols)
                    {
                        result.Append(currentChar);
                    }
                }
            }

            var resultString = result.ToString();
            if (UpperCase)
            {
                return resultString.ToUpper();
            }
            if (LowerCase)
            {
                return resultString.ToLower();
            }
            return resultString;
        }

        private string Decrypt(string inputText, StringBuilder result)
        {
            int index = 0;
            for (int i = 0; i < inputText.Length; i++)
            {
                var currentChar = GetCurrentCharForKey(inputText[i]);
                if (keys.IndexOf(currentChar) != -1)
                {
                    int pi = (int)currentChar;
                    int ki = keys[index];
                    int ci = (pi - ki);
                    if (ci < 0)
                    {
                        ci += Alphabet.Length;
                    }
                    result.Append(Alphabet[ci]);

                    index++;
                    if (index >= keys.Count)
                    {
                        index = 0;
                    }
                }
                else
                {
                    if (!DeleteUnnesessarySymbols)
                    {
                        result.Append(currentChar);
                    }
                }
            }

            var resultString = result.ToString();
            if (UpperCase)
            {
                return resultString.ToUpper();
            }
            if (LowerCase)
            {
                return resultString.ToLower();
            }
            return resultString;
        }
        private char GetCurrentCharForKey(char symbol)
        {
            if (IngoreCase)
            {
                return Char.ToLower(symbol);
            }
            else
            {
                return symbol;
            }
        }

        private void GetKeyForEncrypt(string keyFilePath)
        {
            var serializeKey = File.ReadAllText(keyFilePath).ToLower();
            var keysNotBeaty = JsonSerializer.Deserialize<int[]>(serializeKey);
            keys = new List<int>();

            foreach (var item in keysNotBeaty)
            {
                keys.Add(item);
            }
        }

        private StringBuilder BuildStringB(int length)
        {
            var result = new String('*', length);
            return new StringBuilder(result);
        }

        private void GetAlphabet(string alphabetFilePath)
        {
            
            Alphabet = alphabetFilePath;
        }
    }
}
