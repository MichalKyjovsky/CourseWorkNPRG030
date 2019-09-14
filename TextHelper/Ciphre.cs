using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextHelper
{
    public class Ciphre
    {
        private string pomPasswd = "";
        public string helpflPasswd = "";
        private string hexalPattern = "0123456789ABCDEF";

        public string securePasswd(string passwd)
        {
            if (passwd.Length > helpflPasswd.Length)
            {
                string newHelpflPasswd = "";
                for (int i = 0; i < passwd.Length; i++)
                {
                    newHelpflPasswd += helpflPasswd[i % helpflPasswd.Length];
                }
                helpflPasswd = newHelpflPasswd;
            }
            else if (passwd.Length < helpflPasswd.Length)
            {
                string newHelpflPasswd = "";
                newHelpflPasswd = helpflPasswd.Substring(0, passwd.Length);
                helpflPasswd = newHelpflPasswd;
            }

            for (int i = 0; i < passwd.Length; i++)
            {
                if ((((int)(passwd[i]) + (int)(helpflPasswd[i])) - 96) > 122)
                    pomPasswd += (char)(((int)(passwd[i]) + (int)(helpflPasswd[i]) - 96) - 26);
                else
                    pomPasswd += (char)(((int)(passwd[i]) + (int)(helpflPasswd[i]) - 96));
            }
            return pomPasswd;
        }

        public string toHexal(string prepPasswd)
        {
            string resPasswd = "0x";

            foreach (char letter in pomPasswd)
            {
                for (int i = 1; i < 16; i++)
                {
                    if (((int)letter == 0) || ((((int)(letter)) % i) == 0))
                        resPasswd += hexalPattern[i];
                    if (i % 6 == 0)
                        resPasswd += "0x";
                }
            }
            return resPasswd;
        }
    }
}
