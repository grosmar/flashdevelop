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
            List<String> matchedItems = new List<String>();
            Comparer searchMatch = new Comparer(AdvancedSearchMatch);
            foreach (String item in source)
            {
                if (searchMatch(item, searchText, pathSeparator))
                {
                    matchedItems.Add(item);
                    if (limit > 0 && i++ > limit) break;
                }
            }
            return matchedItems;
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
        static public bool AdvancedSearchMatch(String file, String searchText, String pathSeparator)
        {
            int i = 0;
            int matchStart = 0;
            int matchLength = 0;
            int index;
            int lastIndex = 0;
            int lastSequenceIndex = 0;
            int length = searchText.Length;
            String text = Path.GetFileName(file);
            String subText = "";
            char sequenceStartChar = ' ';

            if (file.Length < length) return false;

            while (i < length)
            {
                // hold the searchable subsequence
                subText = searchText.Substring(matchStart, matchLength + 1);

                // if this is not the first sequence and we just started the new we must check against upper case, unless it cannot match
                if (matchLength == 0 && i > 0)
                {
                    index = text.IndexOf(subText.ToUpper(), lastSequenceIndex, StringComparison.Ordinal);
                }
                else
                {
                    index = text.IndexOf(subText, lastSequenceIndex, StringComparison.OrdinalIgnoreCase);
                }

                if (index == -1)
                {
                    // if failed to match at least a character then it cannot match
                    if (matchLength == 0)
                    {
                        return false;
                    }

                    // if this is the first sequence and it was not started
                    if ( matchStart == 0 && !Char.IsUpper(sequenceStartChar) && !Char.IsUpper(text[lastIndex + matchLength - 1]) )
                    {
                        return false;
                    }

                    //jump to the end of last match and start a new sequence
                    matchStart = matchStart + matchLength;
                    lastSequenceIndex = lastIndex + 1;
                    matchLength = 0;

                    // if we want to jump over the full text then it won't match
                    if (matchStart >= text.Length)
                    {
                        return false;
                    }
                }
                else
                {
                    if ( matchLength == 0 )
                    {
                        sequenceStartChar = text[index];
                    }
                    lastIndex = index;
                    matchLength++;
                    i++;
                }
            }
            return true;
        }

    }
}
