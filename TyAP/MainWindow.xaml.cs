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
        public MainWindow()
        {
            InitializeComponent();
            Begin(MyMatrix, dictionary);
            label1.Content = MyMatrix[1, 1].ToString();
        }
        private void Begin(string [,] Matrix, Dictionary<string, string> dic)
        {
            StreamReader table = new StreamReader("table.txt");
            StreamReader pairs = new StreamReader("encoding.txt");
            string temp = "";
            int str = 0;
            while (temp != null)
            {
                temp = table.ReadLine();
                string[] value = temp.Split(' ');
                for (int i = 0; i < m2; i++)
                {
                    Matrix[str, i] = value[i];
                }
                str++;
            }
            temp = "";
            while (temp != null)
            {
                temp = table.ReadLine();
                string[] value = temp.Split(' ');
                dic.Add(value[0], value[1]);
            }
        }
    }
}
