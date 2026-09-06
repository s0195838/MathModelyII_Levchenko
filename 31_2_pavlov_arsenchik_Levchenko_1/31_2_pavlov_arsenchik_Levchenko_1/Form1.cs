using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _31_2_pavlov_arsenchik_Levchenko_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (((Button)sender).BackColor == Color.Navy)
            {
                ((Button)sender).BackColor = Color.Black;  //меняем цвет кнопки.
            }
            else
            {
                ((Button)sender).BackColor = Color.Navy;

            }    
        }
    }
}
