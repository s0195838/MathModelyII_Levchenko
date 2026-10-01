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


namespace _31_2_pavlov_arsenchik_Levchenko_1
{
    public partial class Form1 : Form
    {
            
        private double[] inputPixels;//массив входных данных
        public Form1()
        {
            InitializeComponent();
            inputPixels = new double[15];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (((Button)sender).BackColor == Color.Navy)
            {   
                ((Button)sender).BackColor = Color.Black;  //меняем цвет кнопки.
                inputPixels[((Button)sender).TabIndex] = 1; // меняем значение в массиве
            }
            else
            {
                ((Button)sender).BackColor = Color.Navy;
                inputPixels[((Button)sender).TabIndex] = 0; // меняем значение в массиве

            }    
        }

        private void buttonSaveTest_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "test.txt";
            string tmpStr = numericUpDown_NecessaryOutput.Value.ToString();
            for (int i = 0; i < inputPixels.Length; i++)
            {
                tmpStr += " " + inputPixels[i].ToString();
            }
            tmpStr += "\n";
            File.AppendAllText(path,tmpStr);

        }

        private void buttonsavetrainSampleClick_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "train.txt";
            string tmpStr = numericUpDown_NecessaryOutput.Value.ToString();
            for (int i = 0; i < inputPixels.Length; i++)
            {
                tmpStr += " " + inputPixels[i].ToString();
            }
            tmpStr += "\n";
            File.AppendAllText(path, tmpStr);
            //C:\Users\Ryzen\source\repos\31_2_pavlov_arsenchik_Levchenko_1\31_2_pavlov_arsenchik_Levchenko_1\bin\Debug
        }
    }
}
