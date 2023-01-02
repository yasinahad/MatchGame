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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MatchGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
            }
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>() //Create a list of eight pairs of emoji
            {
                "🦁","🦁",
                "🐯","🐯",
                "🦊","🦊",
                "🐭","🐭",
                "🦒","🦒",
                "🐻","🐻",
                "🦓","🦓",
                "🐷","🐷",
            };

            Random random = new Random();  //Create a new random number generator

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())  //Find every TextBlock in the main grid and repeat the following statements for each of them
            {
                if (textBlock.Name != "timeTextBlock")  //This if statement inside the foreach loop is used to skips the TextBlock with the name timeTextBlock.
                {
                    textBlock.Visibility = Visibility.Visible; 
                    int index = random.Next(animalEmoji.Count);  //Pick a random number between 0 and the number of emoji left in the list and call it “index”
                    string nextEmoji = animalEmoji[index];  //Use the random number called “index” to get a random emoji from the list
                    textBlock.Text = nextEmoji;  //Update the TextBlock with the random emoji from the list
                    animalEmoji.RemoveAt(index); //Remove the random emoji from the list
                }
                //Start the timer and reset the fields
                timer.Start();
                tenthsOfSecondsElapsed = 0;
                matchesFound = 0;
            }
        }

        TextBlock lastTextBlockClicked;
        bool findingMatch = false;  //It keeps track of whether or not the player just clicked on the first animal in a pair and is now trying to find its match.

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (findingMatch == false)  //The player just clicked the first animal in a pair, so it makes that animal invisible and keeps track of its TextBlock in case it needs to make it visible again.
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if (textBlock.Text == lastTextBlockClicked.Text)  //The player found a match! So it makes the second animal in the pair invisible (and unclickable) too, and resets findingMatch so the next animal clicked on is the first one in a pair again
            {
                matchesFound++;  //Increases matchesFound by one every time the player successfully finds a match.
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            else  //The player clicked on an animal that doesn’t match, so it makes the first animal that was clicked visible again and resets findingMatch.
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // This resets the game if all 8 matched pairs have been found(otherwise it does nothing because the game is still running)
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
