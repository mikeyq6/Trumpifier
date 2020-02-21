using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Trumpifier
{
    public class InputData
    {
        public string InputFilename { get; private set; }
        private int MinWords { get; set; } = 10;

        public Dictionary<string, List<WordData>> Data { get; private set; }

        public InputData(string filename)
        {
            InputFilename = filename;
            parseInput();
        }

        private void parseInput()
        {
            Data = new Dictionary<string, List<WordData>>();
            try
            {
                string input = cleanInput(File.ReadAllText(InputFilename));
                string[] words = input.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    string word = words[i];
                    if (!Data.ContainsKey(word))
                    {
                        Data[word] = new List<WordData>();
                    }
                    if (i < words.Length - 3)
                    {
                        WordData wd = new WordData();
                        wd.Word = words[i + 1];
                        wd.NextWord = words[i + 2];
                        wd.NextNextWord = words[i + 3];
                        Data[word].Add(wd);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.InnerException);
            }
        }

        private string cleanInput(string input)
        {
            input = input.Replace('\n', ' ');

            return Regex.Replace(input, @"[^A-Za-z0-9 \.\-',!:;\#\@?\$%&]", "");

        }

        private bool isFirstWord(string word)
        {
            if (word == string.Empty)
                return false;
            return (word[0] >= 'A' && word[0] <= 'Z');
        }

        public string GetNextSentence()
        {
            StringBuilder sb = new StringBuilder();
            string next = string.Empty;
            WordData interim;
            Random rnd = new Random();
            int r = 0;
            int wordcount = 0;

            while (!next.EndsWith(".") || wordcount <= MinWords)
            {
                if (next == string.Empty)
                {
                    // Get first work
                    var itemlist = (from item in Data
                                    where isFirstWord(item.Key)
                                    select item);
                    r = rnd.Next(0, itemlist.Count());
                    next = itemlist.Skip(r).Take(1).First().Key;
                    sb.Append(next);
                    sb.Append(" ");
                }
                else
                {
                    // Get next word
                    var itemlist = Data[next];
                    r = rnd.Next(0, itemlist.Count());
                    interim = itemlist[r];
                    sb.Append(interim.Word);
                    sb.Append(" ");
                    if (!interim.Word.EndsWith(".") || wordcount <= MinWords)
                    {
                        next = interim.NextWord;
                        sb.Append(next);
                        sb.Append(" ");
                        wordcount++;

                        if (!interim.NextWord.EndsWith(".") || wordcount <= MinWords)
                        {
                            next = interim.NextNextWord;
                            sb.Append(next);
                            sb.Append(" ");
                            wordcount++;

                        }
                    }
                    else
                    {
                        next = interim.Word;
                    }
                }
                wordcount++;
            }

            return sb.ToString();
        }

        public class WordData
        {
            public string Word { get; set; }
            public string NextWord { get; set; }
            public string NextNextWord { get; set; }

            public bool IsEqual(string word)
            {
                return word == Word;
            }
        }
    }

}
