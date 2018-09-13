using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheatingHangman.src
{
    class WordBank
    {
        public List<string> Words { get; set; }

        //possessionlessness is the word with the most repeats at (s)x8
        const int MAX_REPEATED_CHARS = 8;

        public WordBank(string wordBankLocation)
        {
            Words = new List<string>(System.IO.File.ReadAllLines(wordBankLocation));
        }
        
        /**
         * Function PrintWordBank
         * Goal:    to print the word bank
         * 
         */ 
        public void PrintWordBank()
        {
            foreach (string s in Words)
            {
                Console.WriteLine(s);
            }
        }
        
        /**
         * Function FindWordsOfLength
         * Goal:    to return all of the words of some length
         * Return:  All of the words of length
         */ 
        public List<string> FindWordsOfLength(int length)
        {
            List<string> newWords = new List<string>();
            foreach(string s in Words)
            {
                if (s.Length == length)
                    newWords.Add(s);
            }

            return newWords;
        }

        /**
         * Function FindLongestWord
         * Goal:    to find the longest word in the wordbank
         * Return:  the longest word
         */
        public string FindLongestWord()
        {
            string longestString = "";
            foreach(string s in Words)
            {
                if (longestString.Length < s.Length)
                    longestString = s;
            }
            return longestString;
        }

        /**
         * Function FindShortestWord
         * Goal:    to find the shortest word in the wordbank\
         * Return:  the shortest word
         */ 
        public string FindShortestWord()
        {
            string shortestString = Words.First<string>();
            foreach(string s in Words)
            {
                if (shortestString.Length > s.Length)
                    shortestString = s;
            }
            return shortestString;
        }

        /**
         * Function CreateWordFamilies
         * Goal:    to create the word families, 
         *          a.) 0 letters available
         *          b.) 1...N letters available in the word
         */ 
        public Dictionary<int, List<string>> CreateWordFamilies(char c)
        {
            Dictionary<int, List<string>> subFamilies  = new Dictionary<int, List<string>>();
            int wordLength = Words.First().Length;

            for (int i = 0; i < wordLength+1; i++)
            { 
                subFamilies.Add(i, new List<string>());     
            }
            foreach (string word in Words)
            {
                subFamilies[NumberOfOccurrences(word, c)].Add(word);
            }
            return subFamilies;
        }

        /**
         * Function PickAFamily
         * Goal:    to pick the largest family every time
         * 
         */ 
        public static int PickAFamily(Dictionary<int, List<string>> families)
        {
            int biggestFamily = 0;
            for(int i = 0; i < families.Count; i++)
            {
                if(families[biggestFamily].Count < families[i].Count)
                {
                    biggestFamily = i;
                }
            }
            return biggestFamily;
        }

        public static string PickAFamily(Dictionary<string, List<string>> families)
        {
            string biggestFamily = families.Keys.First();
            foreach(KeyValuePair<string, List<string>> entry in families)
            {
                if(families[biggestFamily].Count < families[entry.Key].Count)
                {
                    biggestFamily = entry.Key;
                }
            }
            return biggestFamily;
        }

        /**
         * Function NumberOfOccurrences
         * Goal:    to find the number of occurrences of char c in string s
         * Return:  the number of occurrences
         */ 
        public static int NumberOfOccurrences(string s, char c)
        {
            return s.Count(f => f == c);
        }

        /**
         * Function SingleCharWord
         * Goal:    to find the most common location of a letter in a word.
         */ 
        public static Dictionary<int, List<string>> SingleCharWord(List<string> family, char c)
        {
            int wordLength = family.First().Length;
            Dictionary<int, List<string>> subFamilies = new Dictionary<int, List<string>>();
            for(int i = 0; i < wordLength; i++)
            {
                subFamilies.Add(i, new List<string>());
            }
            foreach(string s in family)
            {
                subFamilies[s.IndexOf(c)].Add(s);
            }

            return subFamilies;
        }

        public static Dictionary<string, List<string>> TwoPlusCharWord(List<string> family, char c)
        {
            int wordLength = family.First().Length;
            string key;
            Dictionary<string, List<string>> subFamilies = new Dictionary<string, List<string>>();
            foreach(string s in family)
            {
                key = BreakIntoLocations(s, c);
                if (!subFamilies.ContainsKey(key))
                {
                    subFamilies.Add(key, new List<string>());
                }
                subFamilies[key].Add(s);
            }
            return subFamilies;
        }

        public static string BreakIntoLocations(string s, char guess)
        {
            string locations = "";
            foreach(char c in s)
            {
                locations += c == guess ? 1 : 0;
            }

            return locations;
        }
    }
}
