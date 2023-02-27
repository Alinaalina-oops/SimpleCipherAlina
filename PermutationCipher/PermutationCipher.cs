using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace PermutationCipher
{
    public class PermutationCipher
    {
        public bool DeleteUnnesessarySymbols = false;
        public bool IngoreCase = false;
        public bool UpperCase = false;
        public bool LowerCase = false;
        private List<int> keys = null;

        public string Encrypt(string keyFilePath, string inputText)
        {
            StringBuilder result = BuildStringB(inputText.Length);
            GetKeyForEncrypt(keyFilePath);
            return Crypt(inputText, result, true);
        }

        public string Decrypt(string keyFilePath, string inputText)
        {
            StringBuilder result = BuildStringB(inputText.Length);
            GetKeyForDecrypt(keyFilePath);
            return Crypt(inputText, result, false);
        }

        private string Crypt(string inputText, StringBuilder result, bool isEncrypt)
         {
            if (isEncrypt)
            {
                for (int i = 0; i < inputText.Length; i += keys.Count)
                {
                    var localKeys = GetLocalArrayKey(i, inputText.Length - i);
                    int iLocal = i;
                    foreach (var item in localKeys)
                    {
                        result[i + item] = inputText[iLocal];
                        iLocal++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < inputText.Length; i += keys.Count)
                {
                    var localKeys = GetLocalArrayKey(i, inputText.Length - i);
                    int iLocal = i;
                    foreach (var item in localKeys)
                    {
                        result[iLocal] = inputText[i + item];
                        iLocal++;
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

        private int[] GetLocalArrayKey(int currentIndex, int length)
        {
            if (keys.Count <= length)
            {
                return keys.ToArray();
            }
            var newKeysArr = keys.Take(length).ToList();
            var tempList = new List<int>(newKeysArr);
            tempList.Sort();
            var result = new List<int>();
            foreach (var item in newKeysArr)
            {
                var resItem = tempList.IndexOf(item);
                result.Add(resItem);
            }
            return result.ToArray();
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

        private void GetKeyForDecrypt(string keyFilePath)
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
    }
}
