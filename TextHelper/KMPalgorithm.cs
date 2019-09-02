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
            int M = pattern.Length;
            int N = txt.Length;
            List<int> indexes = new List<int>();

            int[] lps = new int[M];
            int j = 0;

            computeLPSarray(pattern, M, lps);

            int i = 0;

            while (i < N)
            {
                if (pattern[j] == txt[i])
                {
                    j++;
                    i++;
                }
                if (j == M)
                {
                    indexes.Add(i - j); //Nalezení shody na indexu
                    j = lps[j - 1];
                }
                else if (i < N && pattern[j] != txt[i])
                {
                    if (j != 0)
                        j = lps[j - 1];
                    else
                        i++;
                }
            }
            return indexes;
        }

        private void computeLPSarray(string pattern, int M, int[] lps)
        {
            int len = 0;
            int i = 1;
            lps[0] = 0; // lps[0] je vždy rovno 0

            while (i < M)
            {
                if (pattern[i] == pattern[len])
                {
                    len++;
                    lps[i] = len;
                    i++;
                }
                else
                {
                    if (len != 0)
                    {
                        len = lps[len - 1];
                    }
                    else
                    {
                        lps[i] = len;
                        i++;
                    }
                }

            }
        }
    }
}
