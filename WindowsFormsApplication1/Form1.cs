using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        const int InputNumbers = 0;
        const int InputAlternative = 1;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // С любой новой строки/с любой новой строки от любой позиции(обозначенной пробелами, символами Tab) вводится число.
            // Цифры в числе пишутся слитно, после числа с любого пробела/символа Tab вводятся альтернативы.
            // Альтернатива может отделяться от другой любым количеством пробелов, символов Tab, знаков '>', и любым количеством пробелов, символов Tab и знаков '>',
            // расположенных как угодно.

            // Что бы всё было по тем же принципам, но в строчке сначала располагалась альтернатива, а затем цифра, символы читаются наоборот - справа налево.

            int InputMode = InputNumbers;

            bool bFirst = true;
            bool bFlag;
            bool bErrorAlt = true;
            string text = "";
            string alternatives = "";
            string numbers = "0123456789";
            string symbols = "> \r\0	";
            int[] digits = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] position = new int[2];
            int i, j, n, f, ff = 0, oldf = 0;
            int c = 1;
            int[] nums;
            int CountAlt = 0;

            text = textBox1.Text;
            int l = text.Length;
            if(l == 0)
            {
                MessageBox.Show("Задача задана неверно", "Ошибка");
                return;
            }

            // Принцип Кондорсе гласит что для определения волеизъявления народа необходимо, чтобы каждый избиратель не просто отдал свой голос,а проранжировал кандидатов в порядке предпочтения.
            // Победителем по Кондорсе называется кандидат, который побеждает любого другого кандидата при парном сравнении по правилу большинства.
            // Если парные сравнения образуют цикл, то победителя по Кондорсе нет, и говорят, что имеет место так называемый парадокс Кондорсе.
            // Парадокс Кондорсе - следствие принципа Кондорсе, т.к использование правила простого большинства не может обеспечить транзитивность итогового бинарного отношения
            // (например A>B и B>C,следовательно A>C) общественнного предпочтения среди выбираемых кандидатов. В силу нетранзитированности результат может зависеть от порядка голосования,
            // что дает возможность манипулировать результатами.
            textBox2.Text += "\r\nПринцип Кондорсе гласит что для определения волеизъявления народа необходимо, чтобы каждый избиратель не просто отдал свой голос,а проранжировал кандидатов в порядке предпочтения.\r\nПобедителем по Кондорсе называется кандидат, который побеждает любого другого кандидата при парном сравнении по правилу большинства.\r\nЕсли парные сравнения образуют цикл, то победителя по Кондорсе нет, и говорят, что имеет место так называемый парадокс Кондорсе.\r\nПарадокс Кондорсе - следствие принципа Кондорсе, т.к использование правила простого большинства не может обеспечить транзитивность итогового бинарного отношения\r\n(например A>B и B>C,следовательно A>C) общественнного предпочтения среди выбираемых кандидатов. В силу нетранзитированности результат может зависеть от порядка голосования,\r\nчто дает возможность манипулировать результатами.\r\n";

            for (i = 0; i < l; i++)
            {
                if (text[i] == symbols[2]) c++;
                while (text[i] == symbols[2])
                {
                    i += 2;
                    if (i == l) break;
                    while ((text[i] == symbols[1]) || (text[i] == symbols[4]))
                    {
                        i++;
                        if (i == l) break;
                    }
                }
                if (i == l) break;
            }

            nums = new int[c];
            for (i = 0; i < c; i++) nums[i] = 0;

            c = 0;
            i = 0;
            while (true)
            {
                if (InputMode == InputNumbers)
                {
label1:
                    bFlag = false;
                    while (text[i] == symbols[2])
                    {
                        //в этом случае text[i] это "\r" перед text[i+1] = "\n"
                        i += 2;
                        bFlag = true;
                        if (i == l) goto label3; // если достигли границы массива text, то выйти
                    }
                    while ((text[i] == symbols[1]) || (text[i] == symbols[4]))
                    {
                        i++;
                        bFlag = true;
                        if (i == l) goto label3; // если достигли границы массива text, то выйти
                    }
                    if (bFlag) goto label1;

                    j = 0;
                    while (j < 10)
                    {
                        if (text[i] == numbers[j])
                        {
                            nums[c] = nums[c] * 10 + digits[j];
                            i++;
                            if (i == l) goto label3; // если достигли границы массива text, то выйти
                            j = -1;
                        }
                        j++;
                    }
                    if ((j == 10) && (text[i] != symbols[1]) && (text[i] != symbols[4]))
                    {
                        MessageBox.Show("Задача задана неверно", "Ошибка");
                        return;
                    }
                    c++;
                    InputMode = InputAlternative;
                }
                if (InputMode == InputAlternative)
                {
                    while (true)
                    {
label2:
                        if (text[i] == symbols[2])
                        {
                            if (bErrorAlt)
                            {
                                MessageBox.Show("Задача задана неверно", "Ошибка"); // если в массив alternatives не удалось записать
                                // первую ранжировку(а если по логике алгоритма - хоть одну ранжировку)
                                return;
                            }
                            break;
                        }

                        if ((text[i] == symbols[0]) || (text[i] == symbols[1]) || (text[i] == symbols[4]))
                        {
                            i++;
                            if (i == l)
                            {
                                if (bErrorAlt)
                                {
                                    MessageBox.Show("Задача задана неверно", "Ошибка"); // если в массив alternatives не удалось записать
                                    // первую ранжировку(а если по логике алгоритма - хоть одну ранжировку)
                                    return;
                                }
                                if ((oldf - ff) != CountAlt)
                                {
                                    MessageBox.Show("Задача задана неверно", "Ошибка");
                                    return;
                                }
                                goto label3; // если достигли границы массива text, то выйти
                            }
                            goto label2;
                        }

                        if (!bFirst)
                        {
                            bErrorAlt = true;
                            for (n = 0; n < CountAlt; n++)
                            {
                                if (text[i] == alternatives[n])
                                {
                                    bErrorAlt = false;
                                    break;
                                }
                            }
                            if (bErrorAlt)
                            {
                                MessageBox.Show("Задача задана неверно", "Ошибка");
                                return;
                            }

                            f = oldf;
                            while (f != ff)
                            {
                                if (alternatives[f-1] == text[i])
                                {
                                    MessageBox.Show("Задача задана неверно", "Ошибка");
                                    return;
                                }
                                f--;
                            }
                            if ((oldf - ff) == CountAlt)
                            {
                                MessageBox.Show("Задача задана неверно", "Ошибка");
                                return;
                            }
                            alternatives += text[i];
                            oldf++;
                            i++;
                            if (i == l)
                            {
                                if ((oldf - ff) != CountAlt)
                                {
                                    MessageBox.Show("Задача задана неверно", "Ошибка");
                                    return;
                                }
                                goto label3; // если достигли границы массива text, то выйти
                            }
                        }
                        else
                        {
                            f = CountAlt;
                            while (f != 0)
                            {
                                if (alternatives[f-1] == text[i])
                                {
                                    MessageBox.Show("Задача задана неверно", "Ошибка");
                                    return;
                                }
                                f--;
                            }
                            alternatives += text[i];
                            oldf++;
                            CountAlt++;
                            bErrorAlt = false;
                            i++;
                            if (i == l) goto label3;
                        }
                    }

                    if ((oldf - ff) != CountAlt)
                    {
                        MessageBox.Show("Задача задана неверно", "Ошибка");
                        return;
                    }
                    bFirst = false;
                    alternatives += " ";
                    oldf++;
                    ff = oldf;
                    InputMode = InputNumbers;
                }
            }
label3:
            if(alternatives.Length == 0)
            {
                MessageBox.Show("Задача задана неверно", "Ошибка");
                return;
            }
            if(alternatives[alternatives.Length - 1] == symbols[1])
            {
                alternatives = alternatives.Remove(alternatives.Length - 1);
            }
            textBox2.Text += "Все альтернативы(по условию):\r\n" + alternatives;
            alternatives += symbols[3];

            // (a + b + c)^2 = a^2 + b^2 + c^2 + (a*b + a*c+ b*c) + (b*a + c*a + c*b)
            // {A,B,C} x {A,B,C}
            //----------------------------Для языка C---------------------------------
            //int lenght = (2*C(2,CountAlt))*3; C(int k,int n) - число сочетаний.
            //char* variants = new char[lenght];
            //------------------------------------------------------------------------
            string variants = "";

            i = 0;
            bFirst = true;
            bool bNoSearch = true;
            int oldi = 0;
            int oldj = 0;
            int h = 0;
            string strV = "  ";
            while (alternatives[i] != symbols[1])
            {
                j = h;
                while (alternatives[j] != symbols[1])
                {
                    if (alternatives[i] == alternatives[j]) goto label4;
                    //variants += alternatives[i] + alternatives[j] + " "; // такой строчкой в variants заполняются неверные значения
                    if (bNoSearch)
                    {
                        if (bFirst) bFirst = false;
                        else variants += symbols[1];
                        variants += alternatives[i];
                        variants += alternatives[j];
                        strV = "";
                        strV += alternatives[i];
                        strV += alternatives[j];
                        bNoSearch = false;
                        oldi = i;
                        oldj = j;
                    }
                    if ((strV[1] == alternatives[i]) && (strV[0] == alternatives[j]))
                    {
                        variants += symbols[1];
                        variants += alternatives[i];
                        variants += alternatives[j];
                        i = oldi;
                        j = oldj;
                        if (alternatives[j + 1] == symbols[1]) h++;
                        strV = "  ";
                        bNoSearch = true;
                    }
label4:
                    j++;
                }
                i++;
            }

            textBox2.Text += "\r\nВсе возможные пары для сравнения друг с другом:\r\n" + variants;
            textBox2.Text += "\r\n";
            variants += symbols[3];

            i = 0;
            j = 0;
            n = 0;
            oldi = 0;
            oldj = 0;
            string str = "\r\n", s = "";
            string symbolsAlt = "  ";
            int a, index = 0, num = 0, CountNum = 0;
            bool bMoreIdenticalVariants = false;
            while (true)
            {
                if (alternatives[j] == symbols[1])
                {
                    j++; // чтобы перейти на один шаг вправо с пробела в массиве alternatives
                    oldj++; // чтобы вернуться туда, куда мы попали при переходе через пробел
                }

                bFirst = true;
                while ((alternatives[j] != symbols[1]) && (alternatives[j] != symbols[3]))
                {
                    if (alternatives[j] == variants[i])
                    {
                        position[n] = j;
                        n++;
                    }
                    j++;
                }
                if (n == 1)
                {
                    if (bFirst)
                    {
                        j = oldj;
                        i++;
                        bFirst = false;
                    }
                    else
                    {
                        MessageBox.Show("Задача задана неверно", "Ошибка");
                        return;
                    }
                }
                if (n == 2)
                {
                    if (alternatives[j] == symbols[3])
                    {
                        j = 0; // сбросим счётчик для массива alternatives
                        i++;
                        if (variants[i] == symbols[3])
                        {
                            // Завершим процесс, который начали в if (position[0] < position[1])
                            if (bMoreIdenticalVariants)
                            {
                                str = str.Remove(index);
                                str += s;
                                break; // выйдем за пределы цикла while (true)
                            }
                            else break;
                        }
                        i++; // чтобы перейти на один шаг вправо с пробела в массиве variants
                        oldi = i; // чтобы запомнить позицию, в которую перешли
                    }
                    n = 0;
                    oldj = j;
                    i = oldi;

                    if (position[0] < position[1]) // если нашли вариант в строчке условия
                    {
                        a = (position[0] + 1) / (CountAlt + 1);

                        if ((alternatives[position[0]] == symbolsAlt[0]) && (alternatives[position[1]] == symbolsAlt[1]))
                        {
                            num += nums[a];
                            s = "";
                            s += num.ToString() + symbols[1] + alternatives[position[0]] + symbols[0] + alternatives[position[1]] + symbols[1];
                            bMoreIdenticalVariants = true; // скорее всего один вид парного сравнения будет повторяться в альтернативах,
                            // поэтому необходимо накопить сумму по повторяющимся парным сравнениям в num и заного добавить переменную s, содержащую эту сумму, к str
                        }
                        else
                        {
                            if (bMoreIdenticalVariants)
                            {
                                bMoreIdenticalVariants = false;
                                str = str.Remove(index);
                                str += s;
                                num = nums[a];
                                CountNum++;
                                index = str.Length;
                                str += nums[a].ToString() + symbols[1] + alternatives[position[0]] + symbols[0] + alternatives[position[1]] + symbols[1];
                            }
                            else
                            {
                                num = nums[a];
                                CountNum++;
                                index = str.Length;
                                str += nums[a].ToString() + symbols[1] + alternatives[position[0]] + symbols[0] + alternatives[position[1]] + symbols[1];
                            }
                        }

                        symbolsAlt = "";
                        //symbolsAlt += alternatives[position[0]] + alternatives[position[1]]; // такой строчкой записываются неверные значения
                        symbolsAlt += alternatives[position[0]];
                        symbolsAlt += alternatives[position[1]];

                        textBox2.Text += nums[a].ToString() // itoa(int input, char *buffer, 10) Для языка C
                            + symbols[1] + alternatives[position[0]] + symbols[0] + alternatives[position[1]] + symbols[1];
                    }
                }
            }
            textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 1);
            str = str.Remove(str.Length - 1);
            textBox2.Text += str + "\r\n";
            s = "\r\n";
            i = 2;
            l = str.Length;
            //delete nums; // Для языка C++
            nums = new int[CountNum];
            for (c = 0; c < CountNum; c++) nums[c] = 0;
            c = 0;
            while (c < CountNum)
            {
                j = 0;
                while (j < 10)
                {
                    if (str[i] == numbers[j])
                    {
                        nums[c] = nums[c] * 10 + digits[j];
                        i++;
                        j = -1;
                    }
                    j++;
                }
                if((c % 2) == 1)
                {
                    if(nums[c-1] > nums[c])
                    {
                        //s += str[oldi+1] + str[oldi+2] + str[oldi+3] + symbols[1];  //такой строчкой заполняются неверные значения
                        s += str[oldi + 1];
                        s += str[oldi + 2];
                        s += str[oldi + 3];
                        s += symbols[1];
                    }
                    if(nums[c-1] < nums[c])
                    {
                        //s += str[i+1] + str[i+2] + str[i+3] + symbols[1];  //такой строчкой заполняются неверные значения
                        s += str[i + 1];
                        s += str[i + 2];
                        s += str[i + 3];
                        s += symbols[1];
                    }
                }
                c++;
                oldi = i;
                i += 5;
                if (i > l) break;
            }
            s = s.Remove(s.Length - 1);
            textBox2.Text += "По правилу большинства выиграли следующие парные сравнения:" + s;
        }
    }
}
