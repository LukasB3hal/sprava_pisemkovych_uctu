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
namespace sprava_pisemkovych_uctu_win_forms
{
    public partial class main : Form
    {

        List<string> students = new List<string>();
        List<int> accounts = new List<int>();



        public main()
        {
            InitializeComponent();
        }


        private void button2_Click(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Nastavený filtr uživatelovi nedovolí poslat jiný typ souboru, než .txt
            // Zapsnáno ve tvaru: {To, co uživatel vidí v průzkumníku, jako typ vybíraného souboru}|{To, co filtruje systém}
            openFileDialog1.Filter = "*.txt|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bool semicolon_error = false;
                string[] array;
                string line;
                StreamReader sr = new StreamReader(openFileDialog1.FileName);
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    if (line != "" && line != ";")
                    {
                        array = line.Split(';');
                        if (array.Length > 1)
                        {
                            students.Add(array[0]);
                            if (int.TryParse(array[1], out int parsed))
                            {
                                accounts.Add(parsed);
                            }
                            else
                            {
                                accounts.Add(int.MinValue);
                            }
                        }
                        else
                        {
                            semicolon_error = true;
                            students.Add(array[0]);
                            accounts.Add(int.MinValue);
                        }
                    }
                }

                int accounts_count = accounts.Count();
                int last_created_account = 0;
                int account_to_create;
                // Dopočítávání čísel účtů
                if (accounts_count > 0)
                {
                    for (int i = 0; i < accounts_count; i++)
                    {
                        if (accounts[i] == int.MinValue)
                        {
                            account_to_create = last_created_account + 1;
                            while (accounts.Contains(account_to_create)){
                                account_to_create++;
                            }
                            accounts[i] = account_to_create;
                            last_created_account = account_to_create;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Načetl/a jste prázdný .txt soubor.");
                }
                sr.Close();
                if (semicolon_error)
                {
                    MessageBox.Show("Na některém řádku chyběl středník. Mohl proto být vytvořen špatný uživatelský účet se špatně pojmenovaným adresářem studenta.");
                }
            }
            else
            {
                MessageBox.Show("Nevybral/a jste žádný/správný soubor.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                int accounts_count = accounts.Count();
                for (int i = 0; i < accounts_count; i++)
                {
                    Directory.CreateDirectory(folderBrowserDialog1.SelectedPath + @"\P" + accounts[i] + @"\" + students[i]);
                }
            }
        }
    }
}
