using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lidgren.Network;
using Microsoft.Xna.Framework; 


namespace DBserver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 f2 = new Form2(textBox1.Text.ToString(), int.Parse(textBox2.Text), this);
            f2.Show();
        }


    }
      
}
