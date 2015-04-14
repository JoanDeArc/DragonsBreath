using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Dragon_s_Breath
{
    public partial class Launcher : Form
    {
        public Launcher()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Constants.playing = true;
            Constants.ip = textBox1.Text;
            Constants.port = int.Parse(textBox2.Text);
            Constants.name = textBox3.Text;
            this.Close();
        }
    }
}
