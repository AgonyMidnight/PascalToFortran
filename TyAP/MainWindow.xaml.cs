using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TyAP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        const int m1 = 19, m2 = 15;
        string [,] MyMatrix  = new string [m1,m2];
        int status = 0;
        int countIdentity = 10;
        int countNumber = 0;
        string Out; 

         char [] Book = new char[53] { 'q','Q','w','W','e','E','r','R','t','T','y','Y','u',
    'U','i','I','o','O','p','P','a','A','s','S','d','D','f','F','g','G','h','H','j','J',
    'k','K','l','L','z','Z','x','X','c','C','v','V','b','B','n','N','m','M', '_' };

        //словарь цифр
        char [] Number = new char[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        public MainWindow()
        {
            InitializeComponent();
            Begin(MyMatrix, dictionary);
            label1.Content = dictionary["program"];
        }

        private void Begin(string [,] Matrix, Dictionary<string, string> dic)
        {
            StreamReader table = new StreamReader("Resources\\table.txt");
            string temp = "";
            int str = 0;
            while (!table.EndOfStream)
            {
                temp = table.ReadLine();
                if ( temp != null) { 
                    string[] value = temp.Split('\t');
                
                    for (int i = 0; i < m2; i++)
                    {
                        Matrix[str, i] = value[i];
                    }
                    str++;
                }
            }
            StreamReader pairs = new StreamReader("Resources\\encoding.txt");
            while (!pairs.EndOfStream)
            {   
                temp = pairs.ReadLine();
                if (temp != null)
                {
                    string[] value = temp.Split(' ');
                    dic.Add(value[0], value[1]);
                }
            }
        }
        private void Add_word (string temp)
        {
            dictionary.Add(temp, countIdentity.ToString());
            countIdentity++;
            textBoxOutToken.Text = textBoxOutToken.Text + " " + dictionary[temp];
        }
    }
}
