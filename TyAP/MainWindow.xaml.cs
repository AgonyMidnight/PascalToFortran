using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections;
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
        const int s0m1 = 19, s0m2 = 25;
        string [,] MyMatrix  = new string [m1,m2];
        string[,] StateNull = new string[s0m1, s0m2];
        int countIdentity = 20;
        int countNumber = 0;
        int countFunction = 0;
        int countGoTo = 0;
        List<string> arrayNumbers = new List<string>();
        bool flagFunction = false;
        bool flagGoTo = false;
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



        class Stack
        {
            public List<string> StackTokens = new List<string>();   //Стек
            public int countStack = 0;                              //Размер стека   

            public int state = 0;                                   //Состояние
            public List<string> OutString = new List<string>();     //Выходная строка           
            public int countM = 1;      //счетчик меток
            public int countNum = 1;    //счетчик числен
            public int countFunc = 1;   //счетчик функций


            public Stack(){

            }

            //Указанный элемент (Вершина стека) в выхоодную строку
            public void pop(string temp = null) 
            {
                OutString.Add(temp ?? StackTokens[countStack - 1]);
            }
      
            public void setState(int s)
            {
                state = s;
            }

            //Указанные (из входной строки) в вершину стека
            public void push(string temp) 
            {
                StackTokens.Add(temp);
                countStack++;
            }

            //Удалить последний в стеке элемент
            public void getOut ()  
            {
                StackTokens.RemoveAt(countStack - 1);
                countStack--;
            }

            //Заменить элемент в вершине стека на Указанный
            public void swap(string temp)  
            {
                StackTokens[countStack - 1] = temp;
            }
        }

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
            StreamReader TableStateNull = new StreamReader("Resources\\StateNull.txt");
            str = 0;
            while (!TableStateNull.EndOfStream)
            {
                temp = TableStateNull.ReadLine();
                if (temp != null)
                {
                    string[] value = temp.Split('\t');
                    for (int i = 0; i < s0m2; i++)
                    {
                       StateNull[str, i] = value[i];
                    }
                    str++;
                }
            }
        }
        private void Add_word (string temp)
        {
            if (flagFunction) {
                string temp2 = "F" + countFunction.ToString();
                dictionary.Add(temp, temp2);
                countIdentity++;
                textBoxOutToken.Text = textBoxOutToken.Text + " " + dictionary[temp];
                textBoxUseToken.Text = textBoxUseToken.Text + temp + "     " + dictionary[temp] + "\r\n";
                flagFunction = false;
            }
            else if (flagGoTo)
            {
                string temp2 = "G" + countGoTo.ToString();
                dictionary.Add(temp, temp2);
                countIdentity++;
                textBoxOutToken.Text = textBoxOutToken.Text + " " + dictionary[temp];
                textBoxUseToken.Text = textBoxUseToken.Text + temp + "     " + dictionary[temp] + "\r\n";
                flagGoTo = false;
            }
            else
            {
                string temp2 = "I" + countIdentity.ToString();
                dictionary.Add(temp, temp2);
                countIdentity++;
                textBoxOutToken.Text = textBoxOutToken.Text + " " + dictionary[temp];
                textBoxUseToken.Text = textBoxUseToken.Text + temp + "     " + dictionary[temp] + "\r\n";
            }
        }
        private void Add_Number (string temp)
        {
            dictionary.Add(temp, "N" + countNumber.ToString());
            countNumber++;
            textBoxOutToken.Text = textBoxOutToken.Text + " " + dictionary[temp];
            textBoxUseToken.Text = textBoxUseToken.Text + temp + "     " + dictionary[temp] + "\r\n";
        }
        private void Add_String_Const (string temp)
        {
            dictionary.Add(temp, "S" + countStringConst.ToString());
            countStringConst++;
            textBoxOutToken.Text = textBoxOutToken.Text + " " + dictionary[temp];
            textBoxUseToken.Text = textBoxUseToken.Text + temp + "     " + dictionary[temp] + "\r\n";
        }
        private void getP_Two (string parsbuff)
        {
            bool find = false;
            foreach (KeyValuePair<string, string> keyValue in dictionary.ToArray())
            {
                if (keyValue.Key == parsbuff)
                {
                    textBoxOutToken.Text = textBoxOutToken.Text + " " + keyValue.Value;
                    textBoxUseToken.Text = textBoxUseToken.Text + parsbuff + "     " + dictionary[parsbuff] + "\r\n";
                    find = true;
                    break;
                }
                
            }
            if (find == false)
            {
                getP_One(parsbuff);
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
                    textBoxUseToken.Text = textBoxUseToken.Text + parsbuff + "     " + dictionary[parsbuff] + "\r\n";
                    find = true;
                    if (parsbuff == "procedure" || parsbuff == "function") { flagFunction = true; }
                    if (parsbuff == "GOTO" || parsbuff == "goto") { flagGoTo = true; }
                    break;
                }
               
              
            } 
            if (find == false){
                    Add_word(parsbuff);
                   
            }

        }
        void getP_Three (string parsbuff)
        {
            //dictionary.Add(parsbuff, "N" + countIdentity.ToString());
            arrayNumbers.Add(parsbuff);
            textBoxOutToken.Text = textBoxOutToken.Text + " " + "N" + countNumber.ToString();
            textBoxUseToken.Text = textBoxUseToken.Text + parsbuff + "     " + "N" + countNumber.ToString() + "\r\n";
            countNumber++;
        }
        private void getP_Four(string parsbuff)
        {
            foreach (KeyValuePair<string, string> keyValue in dictionary.ToArray())
            {
                if (keyValue.Key == parsbuff)
                {
                    textBoxOutToken.Text = textBoxOutToken.Text + " " + keyValue.Value;
                    textBoxUseToken.Text = textBoxUseToken.Text + parsbuff + "     " + dictionary[parsbuff] + "\r\n";
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
                    textBoxUseToken.Text = textBoxUseToken.Text + parsbuff + "     " + dictionary[parsbuff] + "\r\n";
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
            for (int i = lengthStr+1; buff[i] != '\''; i++)
            {
                temp = temp + buff[i];
                lengthStr++;
            }
            Add_String_Const(temp);
            return lengthStr+1;
        }

        string getFindDecision(int AnyLexem, int status)
        {
            return MyMatrix[status, AnyLexem];
        }

        string getFindMoveForToken(int Token, int state)
        {
            return StateNull[state, Token];
        }

        void setF()
        {
            MessageBox.Show("error");
        }
        /*******************************        1 LABA                                    ************************************************/
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] arrayBox = textBoxInput.Text.Split('\n');
            for (int i = 0; i < arrayBox.Length; i++)
            {   
                buff = arrayBox[i];
                buff = buff.Replace("\r", "");
                buff += "\n";
                //textBoxOutToken.Text = textBoxOutToken.Text + buff + "\r\n";
                status = 0;
                int mystr = 0;
                for (int j = 0; j < buff.Length ; j++) // || parsBuff != ""
                {
                    
                    if(j == 7)
                    {
                        ;
                    }
                    move = getFindDecision(WhatIsIt(buff[j]), status);
                    if ((move.Length <= 2) && (move != "F") && (move != "Z"))
                    {
                        status = Convert.ToInt32(move);
                        parsBuff = parsBuff + buff[j];
                        if(status == 11)
                        {
                            j--;
                        }
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

                        if (firstParsMove == "9")
                        {
                            mystr = j;
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
                            if (status != 14) j--;
                            if (status == 0) { parsBuff = ""; }
                        }
                        else if (secondParsMove == "P3")
                        {
                            /////ПРОВЕРИТЬ!!!!!111111111!!!!!!!!!!!!!!!ВАЖНААА!ааАААаааа!!11111ОДИН!!!АДЫН!!!!
                            if (firstParsMove == "15")
                            {   //исправлени косяка с 7.
                                parsBuff = parsBuff.Substring(0, parsBuff.Length - 1);//????
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
                            if (status == 11) j--;
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
                            if (status == 14 || status == 7 || status == 0) j++;   //исправление косяка с := после пробела
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
                            if (firstParsMove == "9")
                            {
                                mystr = j;
                            }
                            move = ""; firstParsMove = ""; secondParsMove = "";
                        }
                        else if (secondParsMove == "P7")
                        {
                            j = getP_Seven(buff, mystr);
                            //status = 0;
                            parsBuff = ""; move = ""; firstParsMove = ""; secondParsMove = "";
                            //--i; 
                            
                            continue;
                        }
                        
                    }
                }
                textBoxOutToken.Text = textBoxOutToken.Text + "\r\n";
                parsBuff = "";
            }
            buttonLR2.Visibility = Visibility;
        }

        private void TextBoxInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        int getState(string token)
        {
            if (token == "R15")
            {
                return 1;
            }
            if (token == "iA")
            {
                return 2;
            }
            if (token[0] == 'F')
            {
                return 3;
            }
            if (token == "W03")
            {
                return 4;
            }
            if (token[0] == 'I' && token[1] == 'F' && token[2] == 'M') 
            {
                return 5;
            }
            if (token[0] == 'I' && token[1] == 'F' && token[2] == 'M') //+1
            {
                return 6;
            }
            if (token[0] == 'P' && token[1] == 'R' && token[2] == 'O')
            {
                return 7;
            }
            if (token[0] == 'D' && token[1] == 'C' && token[2] == 'L')
            {
                return 8;
            }
            if (token == "W14" || token == "W15" || token == "W16" || token == "W17" || token == "W18")
            {
                return 9;
            }
            if (token == "W02") //end
            {
                return 10;
            }
            if (token == "O00")
            {
                return 11;
            }
            if (token == "W12")
            {
                return 12;
            }
            if (token == "W11")
            {
                return 13;
            }
            if (token == "W13")
            {
                return 14;
            }
            if (token == "O06" || token == "O07" || token == "O08" || token == "O09" || token == "O10" || token == "O11")
            {
                return 15;
            }
            if (token == "O01" || token == "O02")
            {
                return 16;
            }
            if (token == "O03" || token == "O04")
            {
                return 17;
            }
            if (token == "O16")
            {
                return 18;
            }
            else { return 777; }
           
        }
        /*******************************        2 LABA                                    ************************************************/
        private void Button_ClickLR1(object sender, RoutedEventArgs e)
        {
            Stack stack = new Stack();
          
            string[] arrayBox = textBoxOutToken.Text.Split('\n');
            for (int i = 0; i < arrayBox.Length; i++)
            {
                int state = 0;
                buff = arrayBox[i];
                string[] token = buff.Split(' ');
                for (int j = 0; j < token.Length; j++) {
                    if (token[j] == "" || token[j] == "\n" || token[j] == " " || token[j] == "\r") continue;
                    if(stack.StackTokens.Count == 0) { state = 0; }
                    else
                    {
                        getState(stack.StackTokens.Last());
                    }
                    string move = getFindMoveForToken(WhatIsToken(token[j]), state);
                    string[] ArrayMove = move.Split(',');
                    if (ArrayMove.Length == 1)
                    {
                        if (ArrayMove[0] == "Push")
                        {
                            stack.push(token[j]);
                        }
                    }
                    if (ArrayMove.Length == 2)
                    {

                    }
                    if (ArrayMove.Length == 3)
                    {

                    }
                }
            }
        
        // || parsBuff != ""

            /*string g = StateNull[1, 3];
            stack.push(g);
            stack.pop();
            stack.OutString.ForEach(delegate (string value)
           {
               textBoxOPZ.Text = textBoxOPZ.Text + value + "   ";
           });*/

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private int WhatIsToken (string token)
        {
            if ( token[0] == 'N' || token[0]== 'S')
            {
                return 0;
            }
            if (token[0] == 'I' )
            {
                return 1;
            }
            if (token == "R15")
            {
                return 2;
            }
            if (token == "R16")
            {
                return 3;
            }
            if (token == "R08")
            {
                return 4;
            }
            if (token == "R03")
            {
                return 5;
            }
            if (token == "R06")
            {
                return 6;
            }
            if (token == "W03")
            {
                return 7;
            }
            if (token == "W05")
            {
                return 8;
            }
            if (token == "W04")
            {
                return 9;
            }
            if (token == "W08")
            {
                return 10;
            }
            if (token == "W06")
            {
                return 11;
            }
            if (token == "W02")
            {
                return 12;
            }
            if (token == "W14" || token == "W15" || token == "W16" || token == "W17" || token == "W18" ) 
            {
                return 13;
            }
            if (token == "O00")
            {
                return 14;
            }
            if (token == "W12")
            {
                return 15;
            }
            if (token == "W11")
            {
                return 16;
            }
            if (token == "W13")
            {
                return 17;
            }
            if (token == "O06" || token == "O07" || token == "O08" || token == "O09" || token == "O10" || token == "O11")
            {
                return 18;
            }
            if (token == "O01" || token == "O02")
            {
                return 19;
            }
            if (token == "O03" || token == "O04")
            {
                return 20;
            }
            if (token == "O16")
            {
                return 21;
            }
            if (token == "R04")
            {
                return 22;
            }
            if (token == "R05")
            {
                return 23;
            }
            if (token == "R07")
            {
                return 24;
            }
            else { return 777; 
            }

        }
        private int WhatIsIt(char parsbuff)
        {
            if (parsbuff == 'e' || parsbuff == 'E') { return 7; } //экспериментальная функция
            for (int i = 0; i < 53; i++)
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
