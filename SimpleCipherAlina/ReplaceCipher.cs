using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace SimpleCipherAlina
{
    public class ReplaceCipher
    {
        public bool DeleteUnnesessarySymbols = false;
        public bool IngoreCase = false;
        public bool UpperCase = false;
        public bool LowerCase = false;
        private Dictionary<char, char> keys = null;

        public string Encrypt(string keyFilePath, string inputText)
        {
            StringBuilder result = new StringBuilder();
            //получение ключа для шифрования
            GetKeyForEncrypt(keyFilePath);
            return Crypt(inputText, result);
        }

        public string Decrypt(string keyFilePath, string inputText)
        {
            StringBuilder result = new StringBuilder();
            GetKeyForDecrypt(keyFilePath);
            return Crypt(inputText, result);
        }

        private string Crypt(string inputText, StringBuilder result)
        {
            for (int i = 0; i < inputText.Length; i++)
            {
                var currentChar = GetCurrentCharForKey(inputText[i]);
                if (keys.ContainsKey(currentChar))
                {
                    var charFromDictionary = keys[currentChar];
                    if (inputText[i] == currentChar)
                    {
                        result.Append(charF romDictionary);
                    }
                    else
                    {
                        result.Append(Char.ToUpper(charFromDictionary));
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
            var keysNotBeaty = JsonSerializer.Deserialize<Dictionary<string, string>>(serializeKey);
            keys = new Dictionary<char, char>();

            foreach (var item in keysNotBeaty)
            {
                keys.Add(item.Key[0], item.Value[0]);
            }
        }

        private void GetKeyForDecrypt(string keyFilePath)
        {
            var serializeKey = File.ReadAllText(keyFilePath).ToLower();
            var keysNotBeaty = JsonSerializer.Deserialize<Dictionary<string, string>>(serializeKey);
            keys = new Dictionary<char, char>();

            foreach (var item in keysNotBeaty)
            {
                keys.Add(item.Value[0], item.Key[0]);
            }
        }
    }
}
