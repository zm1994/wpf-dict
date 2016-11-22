/*
 * MainWindow.xaml.cs
 * 
 * @version
 * $Id: MainWindowxaml.cs, Version 1.0 03/14/2015
 * 
 * @revision
 * $Log initial version $
 * 
 */

/**
 * 
 * This is the view of T9 application that for both
 * predictive and non-predictive mode
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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Controllers object
        EventController controller;

        /// <summary>
        /// Initializes the controller
        /// </summary>
        public MainWindow()
        {
            this.controller = new EventController(@"..\..\words.txt");
            InitializeComponent();
        }

        /// <summary>
        /// Checks the Tag of the button pressed and passes it to
        /// the controller. Also takes the next 3 predicted words
        /// from the controller.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            String buttonName = (String)button.Tag;
            Task<string> task = Task.Run(() => controller.ButtonClick(buttonName));
            DisplayTextBox.Text = await task;
            // Empties the predicted words
            predict1.Content = "";
            predict2.Content = "";
            predict3.Content = "";
            String[] label = controller.LabelUpdate();
            // Refills the predicted words
            if (label != null && DisplayTextBox.Text != "")
            {
                // First Predicted word
                if (label.Length >= 1)
                {
                    predict1.Content = label[0];
                }
                // Second Predicted word
                if (label.Length >= 2)
                {
                    predict2.Content = label[1];
                }
                // Third Predicted word
                if (label.Length >= 3)
                {
                    predict3.Content = label[2];
                }
            }
        }

        /// <summary>
        /// Changes Predictive mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsPredictiveON(object sender, RoutedEventArgs e)
        {
            DisplayTextBox.Text = "";
            controller.changePredictionMode();
        }

        private void predict1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Label lab = (Label)sender;
            DisplayTextBox.Text = controller.InsertWordInLastPosition(lab.Content.ToString());
        }

        private async void AddToDictionary(object sender, RoutedEventArgs e)
        {
            btnAddToDictionary.IsEnabled = false;
            
                    if (DisplayTextBox.Text.Length > 0)
                    {
                        controller.AddWordToDictionary(DisplayTextBox.Text);
                        MessageBox.Show("Word was successful added");
                    }

            btnAddToDictionary.IsEnabled = true;
        }
    }
}
