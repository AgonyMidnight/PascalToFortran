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
        int countIdentity = 20;
        int countNumber = 0;
        int countStringConst = 0;
        int status = 0;
        string buff = "";
        string parsBuff = "";
        string move = "";
        string firstParsMove = "";
        string secondParsMove = "";
        bool dveTochki = false;
        string Out; 

         char [] Book = new char[53] { 'q','Q','w','W','e','E','r','R','t','T','y','Y','u',
    'U','i','I','o','O','p','P','a','A','s','S','d','D','f','F','g','G','h','H','j','J',
    'k','K','l','L','z','Z','x','X','c','C','v','V','b','B','n','N','m','M', '_' };

        //словарь цифр
        char [] Number = new char[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        public MainWindow()
        {
            InitializeComponent();
            Begin();
            




        }

        private void Begin()
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
                        MyMatrix[str, i] = value[i];
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
                    dictionary.Add(value[0], value[1]);
                }
            }
        }
        private void Add_word (string temp)
        {
            string temp2 = "I" + countIdentity.ToString();
            dictionary.Add(temp, temp2);
            countIdentity++;
            textBoxOutToken.Text = textBoxOutToken.Text + " " + dictionary[temp];
        }
        private void Add_Number (string temp)
        {
            dictionary.Add(temp, "N" + countIdentity.ToString());
            countNumber++;
            textBoxOutToken.Text = textBoxOutToken.Text + " " + dictionary[temp];
        }
        private void Add_String_Const (string temp)
        {
            dictionary.Add(temp, "S" + countIdentity.ToString());
            countStringConst++;
            textBoxOutToken.Text = textBoxOutToken.Text + " " + dictionary[temp];
        }
        private void getP_Two (string parsbuff)
        {
            bool find = false;
            foreach (KeyValuePair<string, string> keyValue in dictionary.ToArray())
            {
                if (keyValue.Key == parsbuff)
                {
                    textBoxOutToken.Text = textBoxOutToken.Text + " " + keyValue.Value;
                    find = true;
                }
                if (find == false)
                {
                    getP_One(parsbuff);
                    break;
                }
            }
        }
        private void getP_One(string parsbuff)
        {
            bool find = false; ;
            foreach (KeyValuePair<string, string> keyValue in dictionary.ToArray())
            {
                if (keyValue.Key == parsbuff)
                {
                    textBoxOutToken.Text = textBoxOutToken.Text + " " + keyValue.Value;
                    find = true;
                }
                if (find == false)
                {
                    Add_word(parsbuff);
                    break;
                }
              
            }

        }
        void getP_Three (string parsbuff)
        {
            dictionary.Add(parsbuff, "N" + countIdentity.ToString());
            countNumber++;
            textBoxOutToken.Text = textBoxOutToken.Text + " " + dictionary[parsbuff];
        }
        private void getP_Four(string parsbuff)
        {
            foreach (KeyValuePair<string, string> keyValue in dictionary.ToArray())
            {
                if (keyValue.Key == parsbuff)
                {
                    textBoxOutToken.Text = textBoxOutToken.Text + " " + keyValue.Value;
                    break;
                }
            }
        }
        private void getP_Five (string parsbuff)
        {
            foreach (KeyValuePair<string, string> keyValue in dictionary.ToArray())
            {
                if (keyValue.Key == parsbuff)
                {
                    textBoxOutToken.Text = textBoxOutToken.Text + " " + keyValue.Value;
                    break;
                }
            }
        }
        private string getP_Six(string parsBuff, char element)
        {
            return parsBuff + element;
        }
        private int getP_Seven(string buff, int lengthStr)
        {
            string temp = "";
            for (int i = lengthStr; buff[i] != '\''; i++)
            {
                temp = temp + buff[i];
                lengthStr++;
            }
            Add_String_Const(temp);
            return lengthStr;
        }

        string getFindDecision(int AnyLexem, int status)
        {
            return MyMatrix[status, AnyLexem];
        }

        void setF()
        {
            MessageBox.Show("error");
        }
        ///**************************************************************************************************///
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] arrayBox = textBoxInput.Text.Split('\n');
            for (int i = 0; i < arrayBox.Length; i++)
            {   
                buff = arrayBox[i];
                buff = buff.Replace("\r", "");
                //textBoxOutToken.Text = textBoxOutToken.Text + buff + "\r\n";
                status = 0;
                for (int j = 0; j < buff.Length ; j++) // || parsBuff != ""
                {
                    move = getFindDecision(WhatIsIt(buff[j]), status);
                    if ((move.Length <= 2) && (move != "F") && (move != "Z"))
                    {
                        status = Convert.ToInt32(move);
                        parsBuff = parsBuff + buff[j];
                        continue;
                    }
                    if (move == "F")
                    {
                        setF();
                    }
                    if (move == "Z")
                    {
                        MessageBox.Show("All good!");
                    }
                    if (move.Length > 2)
                    {
                        if (move[2] == ',')
                        {
                            firstParsMove = "";
                            secondParsMove = "";
                            firstParsMove = firstParsMove + move[0] + move[1];
                            status = Convert.ToInt32(firstParsMove);
                            for (int k = 3; k < move.Length; k++)
                            {
                                secondParsMove = secondParsMove + move[k];
                            }
                            //parsBuff = parsBuff + buff[i];
                        }
                        else if (move[1] == ',')
                        {
                            firstParsMove = firstParsMove + move[0];
                            status = Convert.ToInt32(firstParsMove);
                            for (int k = 2; k < move.Length; k++)
                            {
                                secondParsMove = secondParsMove + move[k];
                            }
                            //parsBuff = parsBuff + buff[i];
                        }
                        if (secondParsMove == "P1")
                        {
                            getP_One(parsBuff);
                            //status = 0;
                            parsBuff = ""; move = ""; firstParsMove = ""; secondParsMove = "";
                        }
                        else if (secondParsMove == "P2")
                        {
                            getP_Two(parsBuff);
                            //status = 0;
                            parsBuff = "";
                            parsBuff += buff[j]; move = ""; firstParsMove = ""; secondParsMove = "";
                            // expir
                            j--;
                            if (status == 0) { parsBuff = ""; }
                        }
                        else if (secondParsMove == "P3")
                        {
                            /////ПРОВЕРИТЬ!!!!!111111111!!!!!!!!!!!!!!!ВАЖНААА!ааАААаааа!!11111ОДИН!!!АДЫН!!!!
                            if (firstParsMove == "15")
                            {   //исправлени косяка с 7.
                                parsBuff.Substring(0, parsBuff.Length - 1);//????
                                dveTochki = true;
                            }

                            getP_Three(parsBuff);
                            //status = 0;
                            parsBuff = "";
                            parsBuff += buff[j]; move = "";
                            //туточки?

                            if (firstParsMove == "11" || status == 14 || status == 7)
                            {   //исправление косяка с двумя подряд разделителями
                                j++;
                            }

                            firstParsMove = ""; secondParsMove = "";
                            if (status == 16) j++; //от сглаза двух палочек
                            --j; continue;
                        }
                        else if (secondParsMove == "P4")
                        {
                            if (firstParsMove == "0" && dveTochki == true)
                            {
                                parsBuff += '.';
                                dveTochki = false;
                                j++;
                            }

                            getP_Four(parsBuff);
                            //status = 0;
                            parsBuff = ""; move = ""; firstParsMove = ""; secondParsMove = "";
                            if (status == 14 || status == 7) j++;   //исправление косяка с := после пробела
                            --j; continue;

                        }
                        else if (secondParsMove == "P5")
                        {
                            getP_Five(parsBuff);
                            //status = 0;
                            parsBuff = "";
                            parsBuff += buff[j]; move = ""; firstParsMove = ""; secondParsMove = "";
                            if (!(status == 1 || status == 2 || status == 3))  //траблы с двойной буквой
                                --j;
                            continue;
                            //тут?) при 3
                        }
                        else if (secondParsMove == "P6")
                        {
                            parsBuff = getP_Six(parsBuff, buff[j]);
                            move = ""; firstParsMove = ""; secondParsMove = "";
                        }
                        else if (secondParsMove == "P7")
                        {
                            i = getP_Seven(buff, j);
                            //status = 0;
                            parsBuff = ""; move = ""; firstParsMove = ""; secondParsMove = "";
                            //--i; 
                            continue;
                        }
                    }
                }
            }
        }

        private int WhatIsIt(char parsbuff)
        {
            if (parsbuff == 'e' || parsbuff == 'E') { return 7; } //экспериментальная функция
            for (int i = 0; i < 52; i++)
            {
                if (parsbuff == Book[i])
                {
                    return 0;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                if (parsbuff == Number[i])
                {
                    return 1;
                }
            }
            if (parsbuff == '.') { return 2; }
            if (parsbuff == '<') { return 3; }
            if (parsbuff == '>') { return 4; }
            if (parsbuff == '*' || parsbuff == '^') { return 5; }
            if (parsbuff == '=') { return 6; }
            //if (parsbuff == 'e') { return 7; }
            if (parsbuff == '\'') { return 8; }
            if (parsbuff == '/') { return 9; }
            if (parsbuff == ' ' || parsbuff == ',' || parsbuff == ';' || parsbuff == '('
                || parsbuff == ')' || parsbuff == '[' || parsbuff == ']') { return 10; }
            if (parsbuff == '\n') { return 11; }
            if (parsbuff == '\0') { return 12; }
            if (parsbuff == ':') { return 13; }
            if (parsbuff == '+' || parsbuff == '-') { return 14; }
            else {
                MessageBox.Show("Разделитель не найден!");
                return 80;
            }
        }

    }
}
