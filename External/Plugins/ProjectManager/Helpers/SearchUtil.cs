using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectManager.Helpers
{
    public class SearchUtil
    {
        public delegate Boolean Comparer(String value1, String value2, String value3);

        public static List<String> GetMatchedItems(List<String> source, String searchText, String pathSeparator, Int32 limit)
        {
            Int32 i = 0;
            //List<String> matchedItems = new List<String>();

            List<SearchResult> matchedItems = new List<SearchResult>();
            int sequenceCaseMatchCount;

            foreach (String item in source)
            {
                sequenceCaseMatchCount = AdvancedSearchMatch(item, searchText, pathSeparator);
                if (sequenceCaseMatchCount > -1)
                {
                    matchedItems.Add(new SearchResult(item, sequenceCaseMatchCount));
                    if (limit > 0 && i++ > limit) break;
                }
            }


            return matchedItems.OrderByDescending(matchedItem => matchedItem.sequenceCaseMatchCount).Select(matchedItem => matchedItem.file).ToList();
        }

        /* Intelligent search to match camelCase patterns,
            ie. when you have an ExampleClass the following matches will happen:
            ec          true
            EC          true
            amC         true
            ampleclass  true
            amplass     false
            elass       false
            Exampless   false
        */
        static public int AdvancedSearchMatch(String file, String searchText, String pathSeparator)
        {
            int i = 0;
            int matchStart = 0;
            int matchLength = 0;
            int index = -1;
            int lastIndex = 0;
            int lastSequenceIndex = 0;
            int length = searchText.Length;
            int sequenceCaseMatchCount = 0;
            String text = Path.GetFileName(file);
            String subText = "";
            char sequenceStartChar = ' ';
            char lastSubText;

            if (file.Length < length) return sequenceCaseMatchCount;

            while (i < length)
            {
                // hold the searchable subsequence
                subText = searchText.Substring(matchStart, matchLength + 1);

                lastSubText = subText.Last();
                // if this is the start of the sequence we try to check agains uppercase
                if (matchLength == 0 || Char.IsUpper(lastSubText))
                {
                    index = text.IndexOf(Char.ToUpper(lastSubText).ToString(), lastSequenceIndex, StringComparison.Ordinal);

                    /* if we've found the match and the case was uppercase
                     * we add one to the sequenceCaseMatchCount, 
                     * and we reset the sequence because if it's uppercase it's a new start
                     */
                    if (index > -1 && lastSubText == text[index])
                    {
                        matchStart = matchStart + matchLength;
                        matchLength = 1;
                        i++;
                        lastSequenceIndex = index;
                        sequenceCaseMatchCount++;
                        lastIndex = index;
                        continue;
                    }
                }

                // if this is not the start of the sequence or we failed to find the result of the first sequence we try to check case insensitive
                if (matchLength > 0 || (i == 0 && index == -1))
                {
                    index = text.IndexOf(subText, lastSequenceIndex, StringComparison.OrdinalIgnoreCase);
                }

                if (index == -1)
                {
                    // if failed to match at least a character then it cannot match
                    if (matchLength == 0)
                    {
                        return -1;
                    }

                    // if this is the first sequence and it was not started
                    if (matchStart == 0 && !Char.IsUpper(sequenceStartChar) && !Char.IsUpper(text[lastIndex + matchLength - 1]))
                    {
                        return -1;
                    }

                    //jump to the end of last match and start a new sequence
                    matchStart = matchStart + matchLength;
                    lastSequenceIndex = lastIndex + 1;
                    matchLength = 0;

                    // if we want to jump over the full text then it won't match
                    if (matchStart >= text.Length)
                    {
                        return -1;
                    }
                }
                else
                {
                    if (matchLength == 0)
                    {
                        sequenceStartChar = text[index];
                    }
                    lastIndex = index;
                    matchLength++;
                    i++;
                }
            }
            return sequenceCaseMatchCount;
        }
    }

    struct SearchResult
    {
        public string file;
        public int sequenceCaseMatchCount;

        public SearchResult(string file, int sequenceCaseMatchCount)
        {
            this.file = file;
            this.sequenceCaseMatchCount = sequenceCaseMatchCount;
        }
    }
}
