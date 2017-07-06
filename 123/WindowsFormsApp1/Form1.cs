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
            // 删除默认的行和列样式
            tableLayoutPanel3.ColumnStyles.Clear();
            tableLayoutPanel3.RowStyles.Clear();

            tableLayoutPanel3.BackColor = Color.White;
            tableLayoutPanel3.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel3.ColumnCount = 4;
           // tableLayoutPanel3.AutoSize = true;
        }
        private int  zhandianshu= 10;
        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < zhandianshu; i++)
            {
                comboBox1.Items.Add(i);
            }
            comboBox1.SelectedIndex = 1;
        }
        TextBox[] text = new TextBox[10];
        Label[] lab = new Label[10];
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tableLayoutPanel3.Controls.Clear();
            for (int i = 0; i < comboBox1.SelectedIndex; i++)
            {
                lab[i] = new Label();
                lab[i].Text = "目的站点" + i.ToString();
                tableLayoutPanel3.Controls.Add(lab[i]);
                text[i] = new TextBox();
                tableLayoutPanel3.Controls.Add(text[i]);
                
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[comboBox1.SelectedIndex];
            for (int i = 0; i < comboBox1.SelectedIndex; i++)
            {
                try
                {
                  data[i] = (byte)int.Parse(text[i].Text.ToString());  
                }
                catch
                {
                    MessageBox.Show("请输入站点号");
                }
                
            }
        }
    }
}
