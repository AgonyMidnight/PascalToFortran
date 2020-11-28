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
    public partial class MainWindow : Window
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        const int m1 = 19, m2 = 15;
        const int s0m1 = 25, s0m2 = 32;
        const int s1m1 = 2, s1m2 = 6;
        string [,] MyMatrix  = new string [m1,m2];
        string[,] StateNull = new string[s0m1, s0m2];
        string[,] StateOne = new string[s1m1, s1m2];
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


        char[] Book = new char[53] { 'q','Q','w','W','e','E','r','R','t','T','y','Y','u',
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

            public void plusToLast(int last, int preLast = 0, int prePreLast = 0)
            {
                string[] arr = StackTokens.Last().Split('_');
                if(last > 0)
                {
                    arr[arr.Length - 1] = (Int32.Parse(arr[arr.Length - 1]) + last).ToString();
                }
                if (preLast > 0)
                {
                    arr[arr.Length - 2] = (Int32.Parse(arr[arr.Length - 2]) + preLast).ToString();
                }
                if (prePreLast > 0)
                {
                    arr[arr.Length - 3] = (Int32.Parse(arr[arr.Length - 3]) + prePreLast).ToString();
                }
                swap(String.Join("_", arr));
            }

            public int getLastInt()
            {
                string[] arr = StackTokens.Last().Split('_');
                return Int32.Parse(arr[arr.Length - 1]);
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
            str = 0;
            StreamReader TableStateOne = new StreamReader("Resources\\StateOne.txt");
            while (!TableStateOne.EndOfStream)
            {
                temp = TableStateOne.ReadLine();
                if (temp != null)
                {
                    string[] value = temp.Split('\t');
                    for (int i = 0; i < s1m2; i++)
                    {
                        StateOne[str, i] = value[i];
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
            
            try
            {
                dictionary.Add(temp, "S" + countStringConst.ToString());
                countStringConst++;
                textBoxOutToken.Text = textBoxOutToken.Text + " " + dictionary[temp];
                textBoxUseToken.Text = textBoxUseToken.Text + temp + "     " + dictionary[temp] + "\r\n";
            } catch (Exception e)
            {
                textBoxOutToken.Text = textBoxOutToken.Text + " " + dictionary[temp];
                textBoxUseToken.Text = textBoxUseToken.Text + temp + "     " + dictionary[temp] + "\r\n";
            }
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
                    if (parsbuff == "procedure" || parsbuff == "function") { flagFunction = true; }
                    if (parsbuff == "GOTO" || parsbuff == "goto") { flagGoTo = true; }
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

        string getFindMoveForToken(int whatLastInTheStack, int whatInTheStr, int state)
        {
            if (state == 0) return StateNull[whatLastInTheStack, whatInTheStr]; //большая таблица
            if (state == 1) return StateOne[whatLastInTheStack, whatInTheStr];  //маленькая таблица
            else return "Ошибка, состояние может быть только 0 или 1";
        }

        void setF()
        {
            MessageBox.Show("error");
        }
        /*******************************        1 LABA                                    ************************************************/
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            textBoxOutToken.Text = "";
            textBoxUseToken.Text = "";
            textBoxOPZ.Text = "";
         

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
                            if (status == 12 || status == 13 || status == 7) j++;
                        }
                        else if (secondParsMove == "P3")
                        {
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
                            if (status == 16 || status == 12 || status == 13) j++; //от сглаза двух палочек
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
                            if (!(status == 1 || status == 2 || status == 3 )) --j; //траблы с двойной буквой     
                             if(status == 7)       // ибо траблы с + (превращался в ++)                                                                  НОВОЕ!!!
                            {
                                j++;
                                //parsBuff = "";    //ЛИШНЕЕ
                            }
                            continue;
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
        int whatLastInTheStack(string token, int state)
        {
            //ТАБЛИЦА ПО  ВЕРТИКАЛИ
            if (state == 0) {
                if (token.IndexOf("FUNC_") >= 0) return 24;   //function


                if (token == "" || token == "W01") return 0;   // в стеке пусто или begin
                if (token == "R15") return 1;   //(
                if (token[0] == 'А' && token[1] == 'Э' && token[2] == 'М') return 2;   //Array i
                if (token[0] == 'F') return 3;   //Function i
                if (token == "IF") return 4;   //if

                if (token.IndexOf("IF_Mi_") >= 0) return 5;   
                if (token.IndexOf("IF_Mi+1_") >= 0) return 6;  
                if (token.IndexOf("Proc") >= 0) return 7;
                if (token.IndexOf("DCL") >= 0) return 8;    //?

                if (token == "W14" || token == "W15" || token == "W16" || token == "W17" || token == "W18")
                {
                    return 9;   //тип (int,char)
                }

                if (token == "W02") return 10;  //end
                if (token == "O00") return 11;  //:=
                if (token == "W12") return 12;  //or
                if (token == "W11") return 13;  //and
                if (token == "W13") return 14;  //not

                if (token == "O06" || token == "O07" || token == "O08" || token == "O09" || token == "O10" || token == "O11")
                {
                    return 15;  //сравнения
                }

                if (token == "O01" || token == "O02") return 16; // + -
                if (token == "O03" || token == "O04") return 17; // * /
                if (token == "O16") return 18;  // степень
                if (token == "БП") return 19;  // Безусловный переход
                if (token == "УПЛ") return 20;  //Условный переход ложный
                if (token == "GOTO") return 21;  
                if (token == "W00") return 22;  //program
                if (token[0] == 'V') return 23;  //var
                //24 вверху (из за Fi)

            }
            if (state == 1)
            {
                if ((token != "" && token[0] == 'F') || token == "R15") return 0;   //Function i 
                else return 1;
            }
            return 777;
        }
        /*******************************        2 LABA                                    ************************************************/
        private void Button_ClickLR1(object sender, RoutedEventArgs e)
        {
            textBoxOPZ.Text = "";
            Stack stack = new Stack();
          
            string[] arrayBox = textBoxOutToken.Text.Split('\n');
            
            int countBegin = 0;
            int countBeginLevel = 0;

            int countIf = 1;
            int countCrocedure = 0;
            int countFunction = 0;

            for (int i = 0; i < arrayBox.Length; i++)
            {
                buff = arrayBox[i];                             //считали первую строку
                string[] token = buff.Split(' ');               //разбили её по пробелам на массив

                

                for (int j = 0; j < token.Length; j++) {
                    token[j] = token[j].Replace("\r", "");          //Удалили в элементе все лишнее
                    if (token[j] == "" || token[j] == "\n" || token[j] == " " || token[j] == "\r") continue;    //Если там был пробел, то ничего не надо делать

                    string lastInStack = stack.StackTokens.Count == 0 ? "" : stack.StackTokens.Last();          //Находим, что последнее в масиве, если ничего, то пустая строка

                   

                    string move = getFindMoveForToken(whatLastInTheStack(lastInStack, stack.state) , WhatIsToken(token[j], stack.state), stack.state); //Поучаем данные из таблицы


                    List<string> ArrayMove = new List<string>(move.Split(','));         //Создаем Лист, в котором перечислены действия
 
                    while (ArrayMove.Count > 0)
                    {
                        //textBoxOPZ.Text += ArrayMove[0]+"\n";

                      
                        if (ArrayMove[0] == "Pop")              stack.pop();
                        if (ArrayMove[0] == "Pop(X)")           stack.pop(token[j]);
                        if (ArrayMove[0] == "Pop(:)")           stack.pop(":");
                        if (ArrayMove[0] == "Pop(NP)")          stack.pop("НП");    
                        if (ArrayMove[0] == "Pop(KP)")          stack.pop("КП");    
                        if (ArrayMove[0] == "Pop(i_j_NP)")      stack.pop("НП_" + (++countBegin) + "_" + (++countBeginLevel));
                        if (ArrayMove[0] == "Pop(BP)")          stack.pop("БП");
                        if (ArrayMove[0] == "Pop(Mi+1:)")       stack.pop("Мi:_" + stack.getLastInt());

                        if (ArrayMove[0] == "Push")             stack.push(token[j]);
                        if (ArrayMove[0] == "Push(2A)")         stack.push("АЭМ_2");
                        if (ArrayMove[0] == "Push(GOTO)")       stack.push("GOTO");
                        if (ArrayMove[0] == "Push(1F)")         stack.push("F_1");
                        if (ArrayMove[0] == "Push(perem_0)")    stack.push("V_0");


                        if (ArrayMove[0] == "Swap(i+1_A)")      stack.plusToLast(1);
                        if (ArrayMove[0] == "Swap(i+1_F)")      stack.plusToLast(1);
                        if (ArrayMove[0] == "Swap(2F)")         stack.plusToLast(1);
                        if (ArrayMove[0] == "Swap(perem_i+1)")  stack.plusToLast(1);
                        if (ArrayMove[0] == "Swap(G)")          stack.swap(token[j]);


                        //proc
                        if (ArrayMove[0] == "Push(PROC_1_1)") stack.push("PROC_" + (countCrocedure++) + "_" + 1);
                        //endproc

                        if (ArrayMove[0] == "Push(FUNC_i_i)") stack.push("FUNC_" + (+countFunction) + "_1");

                            //if
                        if (ArrayMove[0] == "Pop(UPL_Mi_1)")            stack.pop("UPL_Mi_"+ countIf);  
                        if (ArrayMove[0] == "Pop(Mi+1_BP_Mi:)")         stack.pop("Мi_БП_Мi:_" + (stack.getLastInt() + 1) + "_" + (stack.getLastInt())); 
                        if (ArrayMove[0] == "Pop(IF_Mi_i => Mi:_i)")    stack.pop("Мi:_" + stack.getLastInt());
                        if (ArrayMove[0] == "Swap(IF_Mi_1)")            stack.swap("IF_Mi_" + countIf++); 
                        if (ArrayMove[0] == "Swap(Mi+1_IF)")            { stack.plusToLast(1);  countIf++; }
                        if (ArrayMove[0] == "Push(IF)")                 stack.push("IF");
                        //endif

                        if (ArrayMove[0] == "getOut")           stack.getOut();
                        if (ArrayMove[0] == "Hold")             j--;

                        if (ArrayMove[0] == "State(1)")         stack.state = 1;
                        if (ArrayMove[0] == "State(0)")         stack.state = 0;

                        if (ArrayMove[0] == "KP()")
                        {
                            stack.pop("КП");
                            if (lastInStack == "W01" /*begin - НП*/)  stack.getOut();
                        }  



                        ArrayMove.RemoveAt(0);

                    }
                    // textBoxOPZ.Text += "\n======\n";

                    /*
                     //СУПЕР ОТЛАДЧИК СТЕКА!!!
                    textBoxOPZ.Text += "\n======Стек: \n";
                    foreach (string str in stack.StackTokens)
                    {
                        textBoxOPZ.Text += str+'\n';
                    }
                    textBoxOPZ.Text += "\nВых: \n";
                    foreach (string str in stack.OutString)
                    {
                        textBoxOPZ.Text += str+'\n';
                    }*/
                }
            }
            foreach (string str in stack.OutString)
            {
                //НАПИСАТЬ ОБРАБОТЧИК ДЛЯ i
                textBoxOPZ.Text += str+'\n';
            }
            

        }

        

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private int WhatIsToken (string token, int state)
        {
            //ТАБЛИЦА ПО ГОРИЗОНТАЛИ
            //ИФ СТАТЕ == 1 ТО ЧУТЬ ПО ДРУГОМУ ИСКАТЬ, ТО ЖЕ САМОЕ ДЛЯ ВЕРХНЕЙ ФУНКЦИИ
            if (state == 0 )
            {
                if (token[0] == 'N') return 0;   //N - число (N01)
                if (token[0] == 'S') return 1;   //S - Строка (S01)
                if (token[0] == 'I' || token[0] == 'F') return 2;   //I - Переменная (I01)

                if (token == "R15") return 3;   // (
                if (token == "R16") return 4;   // )

                if (token == "R08") return 5;   // ]
                if (token == "R03") return 6;   // ,
                if (token == "R06") return 7;   // ;
                if (token == "W03") return 8;   //if
                if (token == "W05") return 9;   //then
                if (token == "W04") return 10;  //else
                if (token == "W01") return 11;  //begin
                if (token == "???") return 12;  //???
                if (token == "W02") return 13;  //end

                if (token == "W14" || token == "W15" || token == "W16" || token == "W17" || token == "W18")
                {
                    return 14;  //тип
                }

                if (token == "O00") return 15;  //:=
                if (token == "W12") return 16;  //or
                if (token == "W11") return 17;  //and
                if (token == "W13") return 18;  //not

                if (token == "O06" || token == "O07" || token == "O08" || token == "O09" || token == "O10" || token == "O11")
                {
                    return 19;  //отношения
                }
                if (token == "O01" || token == "O02") return 20;  //+ -
                if (token == "O03" || token == "O04") return 21;  // * /

                if (token == "O16") return 22;  // ^
                if (token == "R04") return 23;  // .
                if (token == "R05") return 24;  // :
                if (token == "R07") return 25;  // [
                if (token == "W10") return 26;  // GOTO
                if (token[0] == 'G') return 27;  // Метка (G1)
                if (token == "W00") return 28;  //program
                if (token == "W06") return 29;  //var
                if (token == "W08") return 30;  //procedure
                if (token == "W09") return 31;  //function
            }
            if (state == 1)
            {
                if (token == "R16") return 0;   // )
                if (token == "R15") return 1;   // (
                if (token == "R07") return 2;   // [
                if (token[0] == 'N' || token[0] == 'S' || token[0] == 'I') return 3;
                if (token == "R05") return 4;
                else return 5;
            }
            return 404;
        }
        private int WhatIsIt(char parsbuff)
        {
            if (parsbuff == 'e' || parsbuff == 'E') { return 7; } //экспериментальная функция
            
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

            
            MessageBox.Show("Разделитель не найден!");
            return 80;
            
        }


        class Interpreter
        {
            //List<string> Out = new List<string>();
            public List<string> stack = new List<string>();

            public Interpreter()
            {
                
            }

            public void del()   //удаляет последний элемент в стеке
            {
                stack.RemoveAt(stack.Count - 1);
            }

            public void plus (string x)     //соединяет 2 последних элемента в один
            { 
                stack[stack.Count - 2] += x + stack[stack.Count - 1];
                del();
            }

            public void plusOpp (string x)  //соединяет 2 элемента в 1, и добавляет скобочки
            {
                stack[stack.Count - 2] = "( " + stack[stack.Count - 2] + x + stack[stack.Count - 1] + " )";
                del();
            }
        }

        private void Button_ClickLR3(object sender, RoutedEventArgs e)
        {
            textBoxFortran.Text = "";
            string[] arrayBox = textBoxOPZ.Text.Split('\n');

            Interpreter stack = new Interpreter();
            Interpreter lastInFunction = new Interpreter();

            foreach (string token in arrayBox)
            {
                if (token == "") textBoxFortran.Text += '\n';
                else if (token[0] == 'I' || token[0] == 'S' || token[0] == 'N')
                {
                    stack.stack.Add(token);
                }
                else if (token == "W00")
                {
                    stack.stack[stack.stack.Count - 1] = "Program " + swap(stack.stack.Last());
                    textBoxFortran.Text += swap(stack.stack.Last()) + '\n';

                }
                else if (token.IndexOf("НП_") >= 0) { }
                else if (token[0] == 'O' || token == "W11" || token == "W12" || token == "W13" )       //операции + - * / := ...
                {
                    if (token == "O00") stack.plusOpp(" O08 ");  //:= заменить на =
                    else
                    stack.plusOpp(" " + token + " ");
                }
                else if (token[0] == 'F')
                {
                    int F = Convert.ToInt32((token.Split('_'))[1]);
                    if(F==1)
                    {
                        stack.stack[stack.stack.Count - 1] += " () ";
                    } else
                    {
                        while (F > 2)
                        {
                            stack.plus(", ");
                            F--;
                        }
                        stack.plus(" ( ");
                        stack.stack[stack.stack.Count - 1] += " )";
                    }
                }
                else if (token == "КП")
                {
                    for (int i = stack.stack.Count - 1,  j=0 ; i>=0; j = i--)
                    {
                        if (stack.stack[i].IndexOf("Program ") >= 0)
                        {
                            if (i == stack.stack.Count-1) break;
                            textBoxFortran.Text += swap(stack.stack[j]) + '\n';
                            stack.stack.RemoveAt(j);
                            i = stack.stack.Count;
                        }
                    }
                    
                    textBoxFortran.Text += "End ";
                    if (stack.stack.Last().IndexOf("Program ") >= 0)
                    {
                        textBoxFortran.Text += swap(stack.stack.Last());
                        stack.del();
                    }
                }

                /*
                 I24
                I25
                O06
                UPL_Mi_1
                НП
                I23
                I26
                O00
                КП
                Мi_БП_Мi:_2_1
                I23
                I27
                O00
                Мi:_2
                КП
                */
                //textBoxFortran.Text += token;
            }

            if (stack.stack.Count >0 )
            {
                textBoxFortran.Text += "\n==========\n";
                foreach (string std in stack.stack)
                {
                    textBoxFortran.Text += std + '\n';
                }
                
            }
                // textBoxFortran.Text = textBoxFortran.Text + buff+ " ";

        }

        private string swap(string x)
        {
            string[] arr = x.Split(' ');
            for (int i = 0; i < arr.Length; i++)
            {
                bool f = false;
                foreach (KeyValuePair<string, string> keyValue in dictionary.ToArray())
                {
                    if (keyValue.Value == arr[i])
                    {
                        arr[i] = keyValue.Key;
                        f = true;
                        break;
                    }
                }
                if (f) continue;
               if(arr[i][0] == 'N')
                {
                    int count = 0;
                    if (arr[i].Length == 2)
                    {
                        string strNum = arr[1];
                        count = Convert.ToInt32(strNum);
                    }
                    else if (arr[i].Length == 3)
                    {
                        string strNum = arr[1] + arr[2];
                        count = Convert.ToInt32(strNum);
                    }
                    arr[i] = arrayNumbers[count];
                }

            }

            return String.Join(" ", arr);
        
        }
    }
}
