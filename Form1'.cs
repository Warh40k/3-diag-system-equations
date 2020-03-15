using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
     public partial class Form1 : Form
     {
        public Form1()
        {
            InitializeComponent();
          
            button1.Enabled = true;
            textBox1.Enabled = true;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            textBox4.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button5.Enabled = false;
            groupBox1.Enabled = false;
            label2.Enabled = false;
            groupBox3.Enabled = false;
        }

        private void Ochistka()
        {
            textBox1.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            radioButton2.Checked = false;
            radioButton1.Checked = false;
            dataGridView1.Rows.Clear();
            textBox1.Focus();
            Program.IMass = 1;
            Program.JMass = 1;

           // button1.Enabled = true;
            // textBox1.Enabled = true;
            groupBox2.Enabled = true;

            /*radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            textBox4.Enabled = false;
            button2.Enabled = false;

           button5.Enabled = false;*/
            button3.Enabled = false;
            button3.BackColor = Color.RosyBrown;
            groupBox1.Enabled = false;
            groupBox3.Enabled = false;
            label2.Enabled = false;
            label2.Clear();
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //_________РАСЧЕТ (КНОПКА "РАСЧЕТ")___________

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            if (radioButton1.Checked || radioButton2.Checked)
            {
                //Преобразование исходных данных о кол-ве в размер матрицы
                int n = Convert.ToInt32(textBox1.Text);

                
                    //Задание массивов
                    double[] c = new double[n];      //Массив - верхняя диагональ
                    double[] b = new double[n];      //Массив - главная диагональ
                    double[] a = new double[n];      //Массив - нижняя диагональ
                    double[] d = new double[n];      //Массив - свободные члены СЛАУ
                    double[,] M = Program.Mass;      //Массив коэффициентов
                label2.Enabled = true;
                // Проверка выбора способа ввода исходных данных в программу
               
                if (radioButton2.Checked)//Для ручного ввода исходных данных
                {
                    
                    for (int i = 1; i < n; i++)//Проверка на трехдиагональность
                        for (int l = 1; l < n; l++)
                                if (i != l && Math.Abs(i - l) > 1)
                                    if (Math.Abs(Program.Mass[i, l]) > 0)
                                        label2.Text = "Введенная матрица не трехдиагональная - система примет нужные числа за 0";
                        //Переназначение введенной матрицы свободных членов
                        for (int i = 0; i < n; i++)
                            d[i] = Program.MassSCH[i + 1];
                    }
                    else if(radioButton1.Checked)//Для имитации исходных данных
                    {
                        Random rnd = new Random();

                        for(int i=1; i<n+1;i++)
                        {
                            if(i!=n)
                            {
                                M[i, i + 1] = rnd.Next(1, 20);
                                M[i + 1, i] = rnd.Next(1, 20);
                            }
                            M[i, i] = rnd.Next(1, 20);
                            d[i-1] = rnd.Next(1, 20);
                        }   
                    }
                    for (int i = 0; i < n; i++)  //Заполнение массивов диагоналей
                        if (i == 0)
                        {
                            b[i] = M[i+1, i+1];
                            a[i] = 0;
                        }
                        else
                        {
                            a[i] = M[i+1, i];
                            c[i - 1] = M[i, i + 1];
                            b[i] = M[i + 1, i + 1];
                        }

                //Сохранение массива коэффициентов в строку для вывода
                string str = "[";
                string strd = "[";
                for (int i=1;i<n+1;i++)
                {
                    for (int t = 1; t < n+1; t++)
                    {
                        str += " "+ M[i, t] + " ";
                    }
                    str += ";";
                    strd += " " + d[i-1] + " ";
                }
                str = str.TrimEnd(';');
                strd = strd.TrimEnd();
                if(radioButton1.Checked)
                    label2.Text= "Случайная матрица: "+str+"]\nСлучайный вектор св. членов:"+strd+"]";

                //Задание начальных коэффициентов прогонки
                double[] y = new double[n];
                    y[0] = b[0];

                    double[] alpha = new double[n];
                    alpha[0] = (-c[0]) / y[0];

                    double[] beta = new double[n];
                    beta[0] = d[0] / y[0];

                    //Прямая прогонка
                    for (int i = 1; i < n - 1; i++)
                    {
                        y[i] = b[i] + a[i] * alpha[i - 1];
                        alpha[i] = (-c[i]) / y[i];
                        beta[i] = (d[i] - a[i] * beta[i - 1]) / y[i];
                    }
                    y[n - 1] = b[n - 1] + a[n - 1] * alpha[n - 2];
                    beta[n - 1] = (d[n - 1] - a[n - 1] * beta[n - 2]) / y[n - 1];

                    //Обратная прогонка
                    double[] x = new double[n];
                    x[n - 1] = beta[n - 1];
                    int j = n - 2;
                    while (j != -1)
                    {
                        x[j] = alpha[j] * x[j + 1] + beta[j];
                        j--;
                    }

                    //Вывод полученных реультатов через DataGridView
                    for (int i = 0; i < n; i++)
                    {
                        int k = i + 1;
                        dataGridView1.Rows.Add("x" + k, x[i]);
                    }
                }

        }

        

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked) // проверка на выбор "ручного ввода данных"
            {
                //Преобразование исходных данных о кол-ве в размер матрицы
                int m = Convert.ToInt32(textBox1.Text);
                int n = m;
                if (label2.Text.Contains("Матрица свободных членов:"))
                {
                    //Ввод матрицы свободных членов
                    if (Program.IMass <= m)
                    {
                        Program.MassSCH[Program.IMass] = Convert.ToDouble(textBox4.Text);
                        label2.Text += " " + textBox4.Text + " ";
                        Program.IMass += 1;
                        if (Program.IMass <= m)
                        {
                            textBox3.Text = "(" + Program.IMass.ToString() + ")";
                            textBox4.Text = "";
                            textBox4.Focus();
                        }
                        else
                        {
                            textBox3.Text = "";
                            textBox4.Text = "";
                            label2.Text+= "]\nВвод данных закончен";
                            button3.Enabled = true;
                            button3.BackColor = Color.Red;
                            button2.Enabled = false;
                            textBox4.Enabled = false;
                           
                        }
                    }
                }
                else
                {
                    //Ввод матрицы коэффициентов
                    try
                    {
                        if (Program.IMass <= m)
                        {

                            Program.Mass[Program.IMass, Program.JMass] = Convert.ToDouble(textBox4.Text);
                            label2.Text += " " + textBox4.Text + " ";
                            if (Program.JMass < n)
                                Program.JMass += 1;
                            else if (Program.JMass == n)
                            {
                                Program.IMass += 1;
                                Program.JMass = 1;
                                label2.Text += ";";
                            }
                            if (Program.IMass <= m)
                            {
                                textBox3.Text = "(" + Program.IMass.ToString() + ";" + Program.JMass.ToString() + ")";
                                textBox4.Text = "";
                                textBox4.Focus();
                            }
                            else
                            {
                                // Окончание ввода матрицы коэффициентов
                                textBox4.Text = "";
                                label2.Text = label2.Text.TrimEnd(';') + "]";
                                label2.Text += "\nМатрица свободных членов: [";
                                textBox4.Focus();
                                Program.IMass = 1;
                                Program.JMass = 1;
                                textBox3.Text = "(" + Program.IMass.ToString() + ")";
                            }
                        }
                    }
                    catch(System.FormatException exc)
                    {
                        MessageBox.Show(exc.Message, "Ошибка", MessageBoxButtons.OK);
                        textBox4.Focus();
                        textBox4.Clear();
                    }
                    
                }
            }
        }

        //_______________ОЧИСТКА ПОЛЕЙ (КНОПКА "ОЧИСТИТЬ")__________________

        private void button4_Click(object sender, EventArgs e)
        {
            Ochistka();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            
                if (textBox1.Text != "")
                {
                    if (int.TryParse(textBox1.Text, out int result) == true)
                    {
                        if(int.Parse(textBox1.Text) > 2)
                        {
                            groupBox3.Enabled = true;
                            groupBox2.Enabled = false;
                            radioButton1.Enabled = true;
                            radioButton2.Enabled = true;
                            button5.Enabled = true;
                            
                    }
                        else
                        {
                            MessageBox.Show("Количество переменных должно быть не меньше 3", "Ошибка", MessageBoxButtons.OK);
                            textBox1.Focus();
                            textBox1.Clear();
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("Входная строка имела неверный формат", "Ошибка", MessageBoxButtons.OK);
                        textBox1.Focus();
                        textBox1.Clear();
                    }
                
        }
            

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                groupBox3.Enabled=false;

                groupBox1.Enabled = true;
                button2.Enabled = true;
                textBox4.Enabled = true;
                label2.Enabled = true;
                textBox4.Focus();
                //Начало ввода матрицы коэффициентов
                textBox3.Text = "(" + Program.IMass.ToString() + ";" + Program.JMass.ToString() + ") =";
                label2.Text= "Матрица коэффициентов: [";
            }
            else if(radioButton1.Checked)
            {
                label2.Enabled = true;
                button5.Enabled = false;
                groupBox3.Enabled = false;
                groupBox1.Enabled = true;
                button3.Enabled = true;
                button3.BackColor = Color.Red;
                button3.Focus();
                label2.Enabled = true;
                label2.Text = "Случайная матрица: ";
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
