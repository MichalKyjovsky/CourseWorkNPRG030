using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextHelper
{
    public class KMPsearchPattern
    {
        public List<int> search(string pattern, string txt)
        {
           
            List<int> indexes = new List<int>();

            int[] lps = new int[pattern.Length];
            int j = 0;

            computeLPSarray(pattern, pattern.Length, lps);

            int i = 0;

            while (i < txt.Length)
            {
                if (pattern[j] == txt[i])
                {
                    j++;
                    i++;
                }
                if (j == pattern.Length)
                {
                    indexes.Add(i - j); //Nalezení shody na indexu
                    j = lps[j - 1];
                }
                else if (i < txt.Length && pattern[j] != txt[i])
                {
                    if (j != 0)
                        j = lps[j - 1];
                    else
                        i++;
                }
            }
            return indexes;
        }

        private void computeLPSarray(string pattern, int patternLenth, int[] lps)
        {
            int length = 0;
            int i = 1;
            lps[0] = 0; // lps[0] je vždy rovno 0

            while (i < patternLenth)
            {
                if (pattern[i] == pattern[length])
                {
                    length++;
                    lps[i] = length;
                    i++;
                }
                else
                {
                    if (length != 0)
                    {
                        length = lps[length - 1];
                    }
                    else
                    {
                        lps[i] = length;
                        i++;
                    }
                }
            }
        }
    }
}
