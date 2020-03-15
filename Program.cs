using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    static class Program
    {
        //задали глобальные переменные
        public static double[,] Mass = new double[21,21]; //переменная для матрицы коэффициентов
        public static double[] MassSCH = new double[21];  //переменная для матрицы свободных членов
        public static int IMass = 1; 
        public static int JMass = 1;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
