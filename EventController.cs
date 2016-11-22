/*
 * EventController.cs
 * 
 * @version
 * $Id: EventController.cs, Version 1.0 03/14/2015
 * 
 * @revision
 * $Log initial version $
 * 
 */

/**
 * 
 * This is the controller of T9 application that takes the data from
 * the View and passes it to the Model.
 * Also returns the data to the View that it gets from the Model
 * 
 * @author Ankit Bhankharia (atb5880)
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project2
{
    class EventController
    {
        // Checks if predictive mode is on
        Boolean isPredictive;
        // object of predictive business model
        BusinessModel businessModel;
        // object of non-predictive business model
        NonPredictiveModel nonPredictiveBusinessModel;
        

        /// <summary>
        /// Initiates Predictive business model and
        /// non predictive business model
        /// </summary>
        /// <param name="file"></param>
        public EventController(String file)
        {
            businessModel = new BusinessModel(file);
            nonPredictiveBusinessModel = new NonPredictiveModel();
            isPredictive = false;
        }
        
        /// <summary>
        /// Changes Prediction mode
        /// </summary>
        public void changePredictionMode()
        {
            // Resets all the data when mode is changed
            businessModel.reset();
            nonPredictiveBusinessModel.reset();
            if (isPredictive == true)
            {
                isPredictive = false;
            }
            else
            {
                isPredictive = true;
            }
        }

        /// <summary>
        ///  Takes next 3 predicted values from the predictive business model
        /// </summary>
        /// <returns></returns>
        public String[] LabelUpdate()
        {
            if (isPredictive)
            {
                String[] label = businessModel.labelChange();
                return label;
            }
            return null;
        }

        /// <summary>
        /// Calls predictive or non-predictive function
        /// depending on which mode is on
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public String ButtonClick(String tag)
        {
            String text;
            if (isPredictive)
            {
                text = PredictiveMode(tag);
            }
            else
            {
                text = NonPredictiveMode(tag);
            }
            return text;
        }

        /// <summary>
        /// Decides which method to call from predictive business model
        /// depending on which button is pressed
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public String PredictiveMode(String tag)
        {
            String text = "";
            switch (tag)
            {
                case "delete":
                    businessModel.Delete();
                    break;
                case "spacebar":
                    businessModel.Spacebar();
                    break;
                case "pound":
                    businessModel.Pound();
                    break;
                case "1":
                    break;
                default:
                    businessModel.Add(tag);
                    break;
            }
            
            List<string> temp = businessModel.makeSentence();
            foreach (String test in temp)
            {
                text += test;
            }
            return text;
        }

        /// <summary>
        /// Decides which method to call from non predictive business model
        /// depending on which button is pressed
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public String NonPredictiveMode(String tag)
        {
            switch (tag)
            {
                case "delete":
                    nonPredictiveBusinessModel.Delete();
                    break;
                default:
                    nonPredictiveBusinessModel.Add(tag);
                    break;
            }
            return nonPredictiveBusinessModel.makeSentence();
        }

        public string InsertWordInLastPosition(string tag) 
        {
            String text = "";
            int lastPos = 0;
            if (businessModel.wordsInSentence.Count >= 0)
                lastPos = businessModel.wordsInSentence.Count - 1;

            businessModel.ChangeWordInSentence(lastPos, tag);
            foreach (String test in businessModel.wordsInSentence)
            {
                text += test;
            }
            //text += tag;

            return text;
        }

        public void AddWordToDictionary(string word) 
        {
            businessModel.AddWordInList(word);
        }
    }
}
