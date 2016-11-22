/*
 * WordPredictor.cs
 * 
 * @version
 * $Id: WordPredictor.cs, Version 1.0 03/14/2015
 * 
 * @revision
 * $Log initial version $
 * 
 */

/**
 * 
 * This is the stores the dictionary for predictive mode
 * 
 * @author Ankit Bhankharia (atb5880)
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{

    /// <summary>
    /// Sorting logic for predicted words
    /// </summary>
    class Sorting : IComparer<String>
    {
        public int Compare(string s1, string s2)
        {
            // Sorts based on the length of the word
            if (s1.Length > s2.Length)
            {
                return 1;
            }
            else if (s2.Length > s1.Length)
            {
                return -1;
            }
            return 0;
        }
    }

    /// <summary>
    /// It will predict all the possible words from the dictionary
    /// based on the buttons pressed
    /// </summary>

    class WordPredictor
    {
        string fileDictionary;
        String[] wordDictionary;
        // Stores all the words with the corresponding key value
        public Dictionary<String, List<String>> predictedWords =
            new Dictionary<String, List<String>>();
        // Dictionary to store value for every button
        public static Dictionary<string, string> predictDictionary =
            new Dictionary<string, string>()
        {
            {"a", "2"},
            {"b", "2"},
            {"c", "2"},
            {"d", "3"},
            {"e", "3"},
            {"f", "3"},
            {"g", "4"},
            {"h", "4"},
            {"i", "4"},
            {"j", "5"},
            {"k", "5"},
            {"l", "5"},
            {"m", "6"},
            {"n", "6"},
            {"o", "6"},
            {"p", "7"},
            {"q", "7"},
            {"r", "7"},
            {"s", "7"},
            {"t", "8"},
            {"u", "8"},
            {"v", "8"},
            {"w", "9"},
            {"x", "9"},
            {"y", "9"},
            {"z", "9"},
        };

        /// <summary>
        /// Finds all the words that can be formed based on
        /// the user input
        /// </summary>
        /// <param name="find"></param>
        /// <returns></returns>
        public List<String> predictWords(String find)
        {
            
            List<String> temp = new List<string>();
            int length = find.Length;
            foreach (KeyValuePair<string, List<string>> pointer in predictedWords)
            {
                if (pointer.Key.Length >= length && pointer.Key.Substring(0, length).Equals(find))
                {
                    temp.AddRange(pointer.Value);
                }
            }
            // To sort words based on their length
            temp.Sort(new Sorting());

            return temp;
        }
        /// <summary>
        /// For every word in the dictionary, it will assign a
        /// appropriate key to it
        /// </summary>
        /// <param name="fileName"></param>
        public WordPredictor(String fileName)
        {
            fileDictionary = fileName;
            ReadDictionary(fileDictionary);
        }

        private async void ReadDictionary(string fileDict) 
        {
            Task<string[]> task = Task.Run(() => System.IO.File.ReadAllLines(fileDictionary));
            wordDictionary = await task;

            for (int i = 0; i < wordDictionary.Length; i++)
            {
                AddWordToDictionaryKey(wordDictionary[i]);
            }
        }

        public void AddWordToDictionaryKey(string word) 
        {
            String key = findKeyForAWord(word);
            // If key is present, it will add the word to next
            // location in the same key
            if (predictedWords.ContainsKey(key))
            {
                predictedWords[key].Add(word);
            }
            // If key is not present, it will add a new key
            else
            {
                predictedWords.Add(key, new List<String>());
                predictedWords[key].Add(word);
            }
        }

        public void RewriteDictionary(string word) 
        {
            var lst = wordDictionary.ToList();
            lst.Add(word);
            lst.Sort();
            System.IO.File.WriteAllLines(fileDictionary, lst.ToArray());
        }

        /// <summary>
        /// Finds the key for every word in the dictionary
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public String findKeyForAWord(String word)
        {
            String key = "";
            for (int i = 0; i < word.Length; i++)
            {
                String temp = word.Substring(i, 1);
                key += predictDictionary[temp];
            }
            return key;
        }
    }
}
