/*
 * BusinessModel.cs
 * 
 * @version
 * $Id: BusinessModel.cs, Version 1.0 03/14/2015
 * 
 * @revision
 * $Log initial version $
 * 
 */

/**
 * 
 * This is the business logic for predictive mode of T9 application
 * 
 * @author Ankit Bhankharia (atb5880)
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Project2
{
    class BusinessModel
    {
        // Stores the words in a sentence
        public List<String> wordsInSentence{ get; protected set; }
        // WordPredictor object
        WordPredictor predictor;
        // stores the position at which the word is to be picked
        // from the currentWordList
        int position = 0;
        // stores the word number on which user is currently working
        int wordPosition = 0;
        // Stores the button values that are pressed by user in a
        // sequence
        private String currentWord = "";
        // Stores the words depending on the user input
        private List<String> currentWordList;

        public BusinessModel(String file)
        {
            try
            {
                
                predictor = new WordPredictor(file);
                wordsInSentence = new List<string>();
                currentWordList = new List<string>();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found" + e.Data);
            }
            
        }

        public void ChangeWordInSentence(int indexWord, string updatedWord) 
        {
            wordsInSentence[indexWord] = updatedWord;
        }

        public void AddWordInList(string word) 
        {
            predictor.RewriteDictionary(word);
        }

        /// <summary>
        /// Resets the data when mode is changed
        /// </summary>

        public void reset()
        {
            position = 0;
            currentWord = "";
            wordsInSentence = new List<string>();
            currentWordList = null;
        }

        /// <summary>
        /// Deletes the character from the word
        /// </summary>
        public void Delete()
        {
            // Deletes the character from the word
            if (currentWord.Length > 0)
            {
                currentWord = currentWord.Substring(0, currentWord.Length - 1);
                if(currentWord.Length > 0)
                currentWordList = predictor.predictWords(currentWord);
            }
            // Deletes the word from the sentence            
            if (currentWord.Length == 0)
            {
                if (wordsInSentence.Count > 0)
                {
                    if (wordPosition < wordsInSentence.Count)
                    {
                        // Removing word from the sentence
                        wordsInSentence.RemoveAt(wordPosition);
                    }

                    if (wordPosition > 0)
                    {
                        wordPosition--;
                    }
                }
            }
        }
        /// <summary>
        /// It is used to cycle through the wordlist
        /// </summary>
        public void Spacebar()
        {
            if (currentWordList != null)
            {
                if (currentWordList.Count > position)
                {
                    position++;
                }
            }
        }
        /// <summary>
        /// Enters space in the sentence and adds the last word and
        /// its state so that when user comes back to that word
        /// he/she can continue from where he/she left
        /// </summary>
        public void Pound()
        {
            wordsInSentence.Add(" ");
            position = 0;
            currentWord = "";
            currentWordList = new List<string>();
            wordPosition++;
            if (wordsInSentence.Count > wordPosition)
            {
                wordPosition++;
            }
        }

        /// <summary>
        /// Adds a character to a word in a sentence
        /// </summary>
        /// <param name="tag"></param>
        public void Add(String tag)
        {
            currentWord += tag;
            currentWordList = predictor.predictWords(currentWord);
            if (currentWordList.Count != 0)
            {
                position = 0;
            }
        }

        /// <summary>
        /// Predcts the word depending on the buttons pressed
        /// </summary>
        /// <returns></returns>
        public List<String> makeSentence()
        {
            if (currentWord.Length > 0)
            {
                String word = "";
                // If word is not present in the dictionary,
                // it will change the word to '-'
                if (currentWordList.Count <= position)
                {
                    for (int i = 0; i < currentWord.Length; i++)
                    {
                        word += "-";
                    }
                }
                else
                {
                    word = currentWordList[position];
                }

                if (wordsInSentence.Count == wordPosition)
                {
                    // If its a new word in a sentence, it adds it to the sentence
                    wordsInSentence.Add(word);
                }
                else
                {
                    // If its a existing word in a sentence, replaces it
                    // with a new word
                    wordsInSentence[wordPosition] = word;
                }
            }
            return wordsInSentence;
        }

        /// <summary>
        /// Predicts the next 3 words from the word list
        /// </summary>
        /// <returns></returns>
        public String[] labelChange()
        {
            String[] label = new String[3];
            if (currentWordList != null && currentWord.Length > 0)
            {
                int length = currentWordList.Count;
                if (length > position + 1)
                {
                    // Stores 1st predicted word
                    label[0] = currentWordList[position + 1];
                }
                if (length > position + 2)
                {
                    // Stores 2nd predicted word
                    label[1] = currentWordList[position + 2];
                }
                if (length > position + 3)
                {
                    // Stores 3rd predicted word
                    label[2] = currentWordList[position + 3];
                }
            }
            return label;
        }
    }
}
