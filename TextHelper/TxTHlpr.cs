using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Net;
using System.Xml;
using System.Drawing.Printing;
using Newtonsoft.Json;

namespace TextHelper
{
    public partial class TxTHlpr : Form
    {
        public static int pocitadlo;
        public TxTHlpr()
        {
            InitializeComponent();
            this.ActiveControl = TextBoxInterface;
            
            /* Odstranění orámečkování z obou panelů nástrojů,
            aby nevznikal přechod mezi form1 a vnitřním prostředím */
            toolStrip1.Renderer = new ToolStripStripeRemoval();
            toolStrip2.Renderer = new ToolStripStripeRemoval();
        }

        //Pojmenování forms1 => viditelnost názvu aplikace při jejím běhu//
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "TextHelper";
        }

        public int line = 0; // proměnná pro úchovu počtu řádků
        public int characters = 0; // počet znaků na řádku, dále vázán k proměnné line
        public static string passwd = "";

        /* Třída pro funkce odstraňující přechody mezi komponentami
         * panelů nástrojů a okolním rámem editoru*/
        public class ToolStripStripeRemoval : ToolStripSystemRenderer
        {
            public ToolStripStripeRemoval() { }

            protected override void
                OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {

            }
        }

        /*Tlačítko nové stránky. Bez uložení vymaže obsah textového pole, 
        možná inovace => dialogové okno zdali nechceme nejprve soubor uložit*/
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            TextBoxInterface.Clear();
        }

        //Tlačítko pro otevření nového či načtení existujícího souboru//
        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Open a file..";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                TextBoxInterface.Clear();
                using (StreamReader sr = new StreamReader(openFile.FileName))
                {
                    TextBoxInterface.Text = sr.ReadToEnd();
                    sr.Close();
                }
            }
        }

            private void FileSaving()
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Title = "Save file as..";
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter txtO = new StreamWriter(saveFile.FileName);
                    txtO.Write(TextBoxInterface.Text);
                    txtO.Close();
                }
            }

        //Tlačítko uložení souboru popř. uložení jako..//
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            FileSaving();
        }

        private void PrintingAFile()
        {
            PrintDocument doc = new PrintDocument();
            PrintDialog pd = new PrintDialog();
            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.Document = doc;
            pd.Document = doc;
            doc.PrintPage += new PrintPageEventHandler(doc_PrintPage);
            if (pd.ShowDialog() == DialogResult.OK)
            {
                doc.Print();
            }
        }

        //Tlačítko tisku souboru, vyvolá diaologové okno pro možnosti tisku//
        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            PrintingAFile();
        }

        /*Tlačítko na vystřižení označené části textu, po označení textu
         * a stisku tlačítka se text z z textového pole vystřihne a je možné
         * jej dále buďto kopírovat nebo zahodit */
        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            TextBoxInterface.Cut();
        }

        //Tlačítko, které zkopíruje označený text, nemaže jej jako cut viz výše//
        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            TextBoxInterface.Copy();
        }

        //Po stisku následujícího tlačítka se zkopírovaná hodnota vloží na pozici kurzoru//
        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            TextBoxInterface.Paste();
        }

        //Tlačítko zpětné akce//
        private void UndoButton_Click(object sender, EventArgs e)
        {
            TextBoxInterface.Undo();
        }

        //Tlačítko dopředné akce//
        private void RedoButton_Click(object sender, EventArgs e)
        {
            TextBoxInterface.Redo();
        }

        /*Tlačítko označí veškerý text v textové poli, 
         * poté jsou možné libovolné operace s označeným textem*/
        private void selectAllButton_Click(object sender, EventArgs e)
        {
            TextBoxInterface.SelectAll();
        }

        /*Otevření nabídky File ve které se skrývají tlačítka, která
         jsou buď již známá a jen se odvoláme na jejich volací funkci,
         ale také nová tlačítka jako náhled, ukončení aplikace,
         či uložit soubor jako. Viz následující 4 metody*/
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newToolStripButton.PerformClick();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openToolStripButton.PerformClick();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToolStripButton.PerformClick();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /*Následují opět tlačítka z panelu nástrojů s vypsanou funkcí,
         * jsou zde funkce pracující s textem tj, Copy, Paste atp. Tlačítka
         jsou k nalezenív rozklikávacím menu Edit. Je to následujících 6 metod.*/
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UndoButton.PerformClick();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RedoButton.PerformClick();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cutToolStripButton.PerformClick();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyToolStripButton.PerformClick();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pasteToolStripButton.PerformClick();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectAllButton.PerformClick();
        }


        //Label pro zobrazení řádků a znaků na řádce //
        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        //Metoda pro počítání řádků a znaků na řádce//
        private void timer1_Tick(object sender, EventArgs e)
        {
            line = 1 + TextBoxInterface.GetLineFromCharIndex(TextBoxInterface.GetFirstCharIndexOfCurrentLine());
            characters = TextBoxInterface.SelectionStart - TextBoxInterface.GetFirstCharIndexOfCurrentLine();
            toolStripStatusLabel1.Text = "line: " + line.ToString() + " | chars on line: " + characters.ToString();
        }

        /*Následující 4 metody představují tlačítka z rozklikávacího menu
         Search. Toto menu obsahuje tlačítka Přesun kurzoru na začátek/konec,
         nalezení textového řetězce a funkci pro nalezení všech výskytů slov,
         které pak nahradí slovem od uživatele */
        private void jumpToTopToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TextBoxInterface.SelectionStart = 0;
            TextBoxInterface.ScrollToCaret();
        }

        private void jumpToBottomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBoxInterface.SelectionStart = TextBoxInterface.Text.Length;
            TextBoxInterface.ScrollToCaret();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            searchBox.Focus();

        }

        private void findAndReplaceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            findBox.Focus();
        }

        /* Metoda, která umožnuje tisk z náhledu. Nutné dodělání CRLF 
        a zobrazení popřípadě tisk více stránek*/
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(TextBoxInterface.Text, new Font("Tie New Roman", 14, FontStyle.Bold), Brushes.Black, new PointF(100, 100));
        }

        /*Metoda pro tisk obsahu textového pole vyvolána tlačítkem tiskárna,
         nikoliv tlačítkem náhled (pracuje s jiným dialogovým oknem)*/
        private void doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            int x = 10;
            int y = 0;
            int charpos = 0;

            while (charpos < TextBoxInterface.Text.Length)
            {
                if (TextBoxInterface.Text[charpos] == '\n')
                {
                    charpos++;
                    y += 20;
                    x = 10;
                }
                else if (TextBoxInterface.Text[charpos] == '\r')
                {
                    charpos++;
                }
                else
                {
                    TextBoxInterface.Select(charpos, 1);
                    e.Graphics.DrawString(TextBoxInterface.SelectedText, TextBoxInterface.SelectionFont, new SolidBrush(TextBoxInterface.SelectionColor), new PointF(x, y));
                    x = x + 8;
                    charpos++;
                }
            }
        }

        //Obsah vyhledávacího pole FIND, jeho reakce na událost//
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        //Obsah vyhledávacího pole REPLACE, jeho reakce na událost//
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        /*Tlačítko které po zadání hledaného znaku popř. slova do 
         textového pole FIND nahradí daný znak popř. slovo
         znakem či slovem zadaným do textového pole REPLACE*/
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (findBox.Text == "" || replaceBox.Text == "")
            {
                DialogResult dialog = MessageBox.Show("Fill your search request!", "Continue", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (dialog == DialogResult.Yes)
                {
                    return;
                }
            }
            else if(TextBoxInterface.Text == "")
            {
                DialogResult dialog = MessageBox.Show("There is nothing to look for!", "Continue", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (dialog == DialogResult.Yes)
                {
                    return;
                }
            }
            else
            {
                TextBoxInterface.SelectionStart = 0;
                TextBoxInterface.SelectionLength = TextBoxInterface.Text.Length;
                TextBoxInterface.SelectionBackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(245)))), ((int)(((byte)(238)))));

                if (findBox.Text != null && !string.IsNullOrWhiteSpace(findBox.Text) && replaceBox.Text != null && !string.IsNullOrWhiteSpace(replaceBox.Text))
                {
                    TextBoxInterface.Text = TextBoxInterface.Text.Replace(findBox.Text, replaceBox.Text);
                }
            }
        }

        /*Tlačítko zavolá metodu pro vyhledávání témat na anglické wikipedii,
         není nutné zadávat do textového pole URL, ani to není možné, 
         stačí zadat hledaný výraz, daná funkce pak vyhledá danou stránku 
         na Wikipedii, dle XML ji zformátuje a vrátí do hlavního textového pole 
         zarovnaný plain text, který je možné dále upravovat*/
        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox4.Text == "")
            {
                DialogResult dialog = MessageBox.Show("Fill your search request", "Continue", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (dialog == DialogResult.Yes)
                {
                    return;
                }
            }else
            {  
            TextBoxInterface.Clear();
            WebClient client = new WebClient();
                using (Stream stream = client.OpenRead("https://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&explaintext=&titles=" + textBox4.Text + "&redirects=true"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    JsonSerializer ser = new JsonSerializer();
                    Result result = ser.Deserialize<Result>(new JsonTextReader(reader));


                    foreach (Page page in result.query.pages.Values)
                    {
                        TextBoxInterface.Text = page.extract;

                        if (TextBoxInterface.Text == "")
                        {
                            TextBoxInterface.Text = "Page you are looking for may does not exist.\nCheck correctness of your request.";
                        }
                    }
                }
                }
        }

        /* Tlačítko volby fontu, velikosti písma a 
        jeho modifikace ze základní sady MS, po stisku
        vyvolá okno kde je možné si příslušné parametry 
        zvolit dle potřeby neboť editor v defaultu začíná s 
        Fontem "ARIAL" velikosti 14 což nemusí uživateli vyhovovat*/
        private void bFont_Click(object sender, EventArgs e)
        {
            DialogResult fontResult = fontDialog1.ShowDialog();
            if (fontResult == DialogResult.OK)
                TextBoxInterface.Font = fontDialog1.Font;
        }

        // Tlačítko náhledu před tiskem z File Menu// 
        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        //Dotázání na ukončení aplikace// 
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Do you really want to close the program?", "Exit", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            { }
            else if (dialog == DialogResult.No)
                e.Cancel = true;
        }

        private string charConventer(string buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                switch (buffer[i])
                {
                    case 'ě':
                        passwd += 'e';
                        break;
                    case 'š':
                        passwd += 's';
                        break;
                    case 'č':
                        passwd += 'c';
                        break;
                    case 'ř':
                        passwd += 'r';
                        break;
                    case 'ž':
                        passwd += 'z';
                        break;
                    case 'ý':
                        passwd += 'y';
                        break;
                    case 'á':
                        passwd += 'a';
                        break;
                    case 'í':
                        passwd += 'i';
                        break;
                    case 'é':
                        passwd += 'e';
                        break;
                    default:
                        passwd += buffer[i];
                        break;
                }
            }
            return passwd;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (passwd2encdTextbox.Text == "")
            {
                DialogResult dialog = MessageBox.Show("Input your password!", "Continue", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (dialog == DialogResult.Yes)
                {
                    return;
                }
            }
            else
            {
                string password = "";
                string key = "";

                Ciphre ciphre;

                if (pocitadlo % 2 == 0)
                {
                    passwd2encdLabel.Text = "Helpful password";
                    encodeButton.Text = "Encode";
                    password = charConventer(passwd2encdTextbox.Text.ToLower().Trim());
                }
                else
                {
                    ciphre = new Ciphre();
                    key = charConventer(passwd2encdTextbox.Text.ToLower().Trim());

                    passwd2encdLabel.Text = "Password to encode";
                    encodeButton.Text = "Submit";
                    ciphre.helpflPasswd = key;
                    TextBoxInterface.Text = ciphre.toHexal(ciphre.securePasswd(passwd));
                    passwd = "";
                }
                passwd2encdTextbox.Text = "";
                pocitadlo++;
            }
        }
        private void Search_button_Click(object sender, EventArgs e)
        {
            if (searchBox.Text == "")
            {
                DialogResult dialog = MessageBox.Show("Fill your search request", "Continue", MessageBoxButtons.OK,MessageBoxIcon.Error);
                if (dialog == DialogResult.Yes)
                {
                    return;
                }

            }
            else
            {
                TextBoxInterface.SelectionStart = 0;
                TextBoxInterface.SelectionLength = TextBoxInterface.Text.Length;
                TextBoxInterface.SelectionBackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(245)))), ((int)(((byte)(238)))));

                KMPsearchPattern kMPsearchPattern = new KMPsearchPattern();
                List<int> indexy = kMPsearchPattern.search(searchBox.Text.ToLower(), TextBoxInterface.Text.ToLower());
                string pattern = searchBox.Text;

                foreach (int item in indexy)
                {
                    TextBoxInterface.SelectionStart = item;
                    TextBoxInterface.SelectionLength = pattern.Length;
                    TextBoxInterface.SelectionColor = Color.Black;
                    TextBoxInterface.SelectionBackColor = Color.DodgerBlue;
                }
            }
        }

        private void PrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintingAFile();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileSaving();
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
