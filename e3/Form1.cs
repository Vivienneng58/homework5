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

namespace e3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        String[] path = new String[2];
        String[] file = new string[2];

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                file[0] = ofd.FileName;
                this.textBox1.Text = file[0];

                path[0] = this.textBox1.Text;
                this.textBox3.Text = File.ReadAllText(file[0], Encoding.Default);//读取指定路径的文件              
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                file[1] = ofd.FileName;
                this.textBox2.Text = file[1];
                path[1] = this.textBox2.Text;
                this.textBox3.Text = File.ReadAllText(file[1], Encoding.Default);//读取指定路径的文件  
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int b;
            int n = path.Length;
            FileStream[] fileIn = new FileStream[n];
            using (FileStream fileOut = new FileStream("C:/Users/Anyone/source/repos/homework5/e3/new.txt", FileMode.Create))
            {
                for (int i = 0; i < n; i++)
                {
                    try
                    {
                        fileIn[i] = new FileStream(path[i], FileMode.Open);
                        while ((b = fileIn[i].ReadByte()) != -1)
                            fileOut.WriteByte((byte)b);
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        fileIn[i].Close();
                    }

                }
            }
        }
    }
}
