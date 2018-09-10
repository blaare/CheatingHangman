using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheatingHangman.src
{
    class Program
    {
        const string WORD_FILE_LOCATION = @"C:\Users\Timothy Wilson\Documents\GameDevelopment\1_CheatingHangman\CheatingHangman\CheatingHangman\assets\dictionary.txt";
        const int ALLOWED_GUESSES = 10;

        static void Main(string[] args)
        {
            bool readyForGame       = false,
                 displayCounter     = false,
                 completedGame      = false;

            WordBank wordBank       = new WordBank(WORD_FILE_LOCATION);

            int longestWordLength   = wordBank.FindLongestWord().Length,
                shortestWordLength  = wordBank.FindShortestWord().Length,
                incorrectGuesses    = 0,
                wordLength,
                familySelection;

            List<char> usedLetters  = new List<char>();
            
            Dictionary<int, List<string>> families;
            
            char[] correctGuesses;
            char guess;

            //Initialization Process

            //Read the file dictionary.txt, which contains the full contents of the Official Scrabble Player's Dictionary, 
            //  Second Edition. This word list has over 120,000 words. Research how to handle File I/O in C#.
            Console.WriteLine("==== Welcome to Cheating Hangman ====");
            do
            {
                //Input Process

                //Prompt the user for a word length, reprompting as necessary until they enter a number such that there 
                //  is at least one word that's exactly that long. That is, the game cannot be played with an integer 
                //  word length of -42 or 137, since no English words precisely that long.

                Console.WriteLine("Please input a word length from {0} to {1}",shortestWordLength,longestWordLength);
                int.TryParse(Console.ReadLine(), out int input);
                wordLength      = input;
                readyForGame    = 
                    (input >= shortestWordLength) && 
                    (input <= longestWordLength) && 
                    (wordBank.FindWordsOfLength(wordLength).Count() > 0);
                if (!readyForGame)
                    Console.WriteLine("No words found of length {0}", wordLength);
                
            } while (!readyForGame );

            //Prompt the user for the runtime option of the option to display a running total of the number of words 
            //  remaining in the word list. See the example program trace. (This completely ruins the illusion of a 
            //  fair game that you'll be cultivating, but it's quite useful for testing -- and grading!)
            Console.WriteLine("Would you like a running word(s) counter to be displayed?[Y/n]");
            string result = Console.ReadLine();
            displayCounter = result.ToLower() == "y";


            //Prepare for Guessing

            wordBank.Words = wordBank.FindWordsOfLength(wordLength);
            correctGuesses = new char[wordLength];

            //Guessing Process

            //Prompt the user for a number of incorrect guesses that they are allowed, which must be an integer greater 
            //  than zero. (The user will always win for games with 26 or more incorrect guesses.)
            do
            {
                DisplayNumberOfGuesses(ALLOWED_GUESSES - incorrectGuesses);
                if (displayCounter)
                    DisplayCounter(wordBank);
                DisplayWordBlanks(wordLength, correctGuesses);
                DisplayUsedLetters(usedLetters);
                guess = char.ToLower(GetUserGuess());

                if (usedLetters.IndexOf(guess) == -1)
                {
                    usedLetters.Add(guess);
                    usedLetters.Sort();

                    families            = wordBank.CreateWordFamilies(guess);
                    familySelection     = wordBank.PickAFamily(families);

                    //Family Selection Handling
                    if (familySelection == 0)
                    {
                        Console.WriteLine(0);
                        wordBank.Words = families[familySelection];
                    }
                    else if (familySelection == 1)
                    {
                        Console.WriteLine(1);
                        families = wordBank.SingleCharWord(families[familySelection], guess);
                        familySelection = wordBank.PickAFamily(families);
                        wordBank.Words = families[familySelection];
                        correctGuesses[familySelection] = guess;
                    }
                    else
                    {


                        completedGame = (wordLength != correctGuesses.Length) || (++incorrectGuesses == ALLOWED_GUESSES);
                    }
                }
                

            } while (!completedGame);

            if (incorrectGuesses == ALLOWED_GUESSES)
                DisplayLosingMessage();
            else
                DisplayWinningMessage();

            ExitProtocol();


        }

        /**
         * Function DisplayNumberOfGuesses
         * Goal:    to display the number of guesses left
         */ 
        public static void DisplayNumberOfGuesses(int remainingGuesses)
        {
            Console.WriteLine("You are allowed {0} more incorrect guesses.", remainingGuesses);
        }

        /**
         * Function DisplayCounter
         * Goal:    to display the remaining number of words in the bank
         */ 
        public static void DisplayCounter(WordBank wordBank)
        {
            Console.WriteLine("[ # of words possible: {0}]", wordBank.Words.Count);
        }

        /**
         * Function DisplayWordBlanks
         * Goal:    to display what is available for the word, for instance...
         *              if the word is of length 5:         _ _ _ _ _
         *              if the user has correct guesses:    f _ g _ _
         */ 
        public static void DisplayWordBlanks(int wordLength, char[] correctGuesses)
        {
            string wordBlanks = "";
            for(int i = 0; i < wordLength; i++)
            {
                wordBlanks += correctGuesses[i] != 0 ? correctGuesses[i] + " " : "_ ";
            }
            Console.WriteLine("Word: {0}", wordBlanks);
        }

        /**
         * Function DisplayUsedLetters
         * Goal:    to display the used letters
         */ 
        public static void DisplayUsedLetters(List<char> letters)
        {
            string usedLetters = "Used Letters: ";
            foreach(char c in letters)
            {
                if(c != 0)
                    usedLetters += c + " ";
            }
            Console.WriteLine(usedLetters);
        }

        /**
        * Function DisplayWinningMessage
        * Goal:    to display the winning message
        */
        public static void DisplayWinningMessage()
        {
            Console.WriteLine("You won! :)");
        }

        /**
         * Function DisplayLosingMessage
         * Goal:    to display the losing message
         */
        public static void DisplayLosingMessage()
        {
            Console.WriteLine("You Lost! :(");
        }

        /**
         * Function ExitProtocol
         * Goal:    to sleep for 1 second
         */ 
        public static void ExitProtocol()
        {
            System.Threading.Thread.Sleep(1000);
        }

        /**
         * Function GetUserGuess
         * Goal:    return the result of the users input
         * 
         */
        public static char GetUserGuess()
        {
            Console.Write("Enter Guess:");
            char.TryParse(Console.ReadLine(), out char result);
            return result;
        }




    }

    
}
