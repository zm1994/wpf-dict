/*
 * NonPredictiveModel.cs
 * 
 * @version
 * $Id: NonPredictiveModel.cs, Version 1.0 03/14/2015
 * 
 * @revision
 * $Log initial version $
 * 
 */

/**
 * 
 * This is the business logic for non-predictive mode of T9 application
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
    class NonPredictiveModel
    {
        // Stores the output string
        String result;
        // Stores the values for each button
        char[][] characterResult;
        // Checks which button was pressed last time
        String lastButtonPressed = "";
        // number of times same button is pressed
        int buttonPressed = 0;
        // Time at which button was pressed
        DateTime buttonPressedTime;
        // position in text for insert words which has been founded;
        public int LastPositionForInsertWord = 0;

        /// <summary>
        /// Resets the result
        /// </summary>
        public void reset()
        {
            result = "";
        }

        /// <summary>
        /// Initializes the values for each button
        /// </summary>
        public NonPredictiveModel()
        {
            characterResult = new char[11][];
            characterResult[0] = new char[] { '1' };
            characterResult[1] = new char[] { 'a', 'b', 'c', '2' };
            characterResult[2] = new char[] { 'd', 'e', 'f', '3' };
            characterResult[3] = new char[] { 'g', 'h', 'i', '4' };
            characterResult[4] = new char[] { 'j', 'k', 'l', '5' };
            characterResult[5] = new char[] { 'm', 'n', 'o', '6' };
            characterResult[6] = new char[] { 'p', 'q', 'r', 's', '7' };
            characterResult[7] = new char[] { 't', 'u', 'v', '8' };
            characterResult[8] = new char[] { 'w', 'x', 'y', 'z', '9' };
            characterResult[9] = new char[] { '0', '~'};
            characterResult[10] = new char[] { ' ' };
            buttonPressedTime = DateTime.Now;

        }

        /// <summary>
        /// Deletes the character from the output string
        /// </summary>
        public void Delete()
        {
            if (result != null)
            {
                int length = result.Length;
                if (length > 0)
                    result = result.Substring(0, length - 1);
            }
            
        }

        /// <summary>
        /// Enters a space in the output
        /// </summary>
        public void Spacebar()
        {
            result = result + " ";
        }
        /// <summary>
        /// Adds the character to the output string
        /// </summary>
        /// <param name="tag"></param>
        public void Add(String tag)
        {
            int length = 0;
            // If same button is pressed quickly, it will give the next value of the same button
            if (tag.Equals(lastButtonPressed) && (DateTime.Now - buttonPressedTime).TotalMilliseconds < 500)
            {
                buttonPressed++;
                length = result.Length;
                if (length > 0)
                    result = result.Substring(0, length - 1);
            }
            else
            {
                buttonPressed = 0;
            }

            lastButtonPressed = tag;
            
            int row;
            int column;
            // Takes the value based on which button and number of times that
            // button is pressed
            try
            {
                if (tag.Equals("spacebar"))
                {
                    LastPositionForInsertWord = length;
                    row = 9;
                }
                else if (tag.Equals("pound"))
                {
                    row = 10;
                }
                else
                {
                    row = Convert.ToInt32(tag) - 1;
                }
                
                column = buttonPressed % characterResult[row].Length;
                result += characterResult[row][column];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Data);
            }
            buttonPressedTime = DateTime.Now;
        }

        // returns the output string
        public String makeSentence()
        {
            return result;
        }

    }
}
