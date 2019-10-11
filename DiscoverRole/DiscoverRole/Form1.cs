using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace DiscoverRole
{
    public partial class Form1 : Form
    {
        HashSet<Point>[] p;
        Label[] us;
        Label[] ts;
        Label[] usR;
        Label[] tsR;
        Label[] rl;
        List<HashSet<int>> roleusers = new List<HashSet<int>>();
        bool render, renderR;
        public Form1()
        {
            InitializeComponent();
            render = false;
            renderR = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
        private void Load_ControlsB()
        {
            for (int i = 0; i < us.Length; i++)
            {
                us[i] = new Label();
                us[i].Text = "U" + (i + 1);
                us[i].Width = 30;
                us[i].AutoSize = true;
                us[i].Top = 50;
                us[i].Left = us[i].Width * i + 10;
                Controls.Add(us[i]);
            }
            for (int i = 0; i < ts.Length; i++)
            {
                ts[i] = new Label();
                ts[i].Text = "P" + (i + 1);
                ts[i].Width = 30;
                ts[i].AutoSize = true;
                ts[i].Top = 200;
                ts[i].Left = ts[i].Width * i + 10;
                Controls.Add(ts[i]);
            }
        }

        private void Load_ControlsA()
        {
            for (int i = 0; i < usR.Length; i++)
            {
                usR[i] = new Label();
                usR[i].Text = "U" + (i + 1);
                usR[i].Width = 30;
                usR[i].AutoSize = true;
                usR[i].Top = 275;
                usR[i].Left = usR[i].Width * i + 10;
                Controls.Add(usR[i]);
            }
            for (int i = 0; i < tsR.Length; i++)
            {
                tsR[i] = new Label();
                tsR[i].Text = "P" + (i + 1);
                tsR[i].Width = 30;
                tsR[i].AutoSize = true;
                tsR[i].Top = 625;
                tsR[i].Left = tsR[i].Width * i + 10;
                Controls.Add(tsR[i]);
            }
        }

        private void Load_Points()
        {
            Random rnd = new Random();
            //string str;
            
            for (int t = 0; t < p.Length; t++)
            {
                int g = -1;
                //str = "U"+(t+1)+": ";
                p[t] = new HashSet<Point>();
                int k = rnd.Next(1, ts.Length);
                for (int i = 0; i < k; i++)
                {
                    int kk;
                    do
                    {
                        kk = rnd.Next(0, ts.Length);
                    } while (kk < g);
                    g = kk;
                    //str += "P" + (kk + 1)+"(";
                    //str+=
                    p[t].Add(ts[kk].Location);
                    //str += "),";
                }
                //listBox1.Items.Add(str);
            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (render)
            {
                for(int i=0;i<us.Length;i++)
                {
                    for (int j = 0; j < p[i].Count; j++)
                    {
                        e.Graphics.DrawLine(Pens.Black, new Point(us[i].Location.X, us[i].Location.Y+us[i].Height), p[i].ElementAt(j));
                    }
                }
                render = false;
            }
            if (renderR)
            {

                for (int i = 0; i < roleusers.Count; i++)
                {
                    for (int j = 0; j < roleusers[i].Count; j++)
                    {
                        e.Graphics.DrawLine(Pens.Black, new Point(usR[roleusers[i].ElementAt(j)].Location.X, usR[roleusers[i].ElementAt(j)].Location.Y + usR[roleusers[i].ElementAt(j)].Height), rl[i].Location);
                        for (int k = 0; k < p[roleusers[i].ElementAt(j)].Count; k++)
                        {
                            e.Graphics.DrawLine(Pens.Black, new Point(rl[i].Location.X, rl[i].Location.Y + rl[i].Height), new Point(p[roleusers[i].ElementAt(j)].ElementAt(k).X, 625));
                        }
                    }
                }
                renderR = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Load_Points();
            render = true;
            Refresh();
            button3.Enabled = true;
            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Should not be Empty !");
                textBox1.Focus();
                return;
            }
            if (int.Parse(textBox1.Text) <= 0 || int.Parse(textBox1.Text) > 45)
            {
                MessageBox.Show("Users must be between 1 and 45 !");
                textBox1.Clear();
                textBox1.Focus();
                return;
            }

            if (int.Parse(textBox2.Text) < 3 || int.Parse(textBox2.Text) > 45)
            {
                MessageBox.Show("Permissions must be between 3 and 45 !");
                textBox2.Clear();
                textBox2.Focus();
                return;
            }

            us = new Label[int.Parse(textBox1.Text)];
            p = new HashSet<Point>[int.Parse(textBox1.Text)];
            ts = new Label[int.Parse(textBox2.Text)];
            Load_ControlsB();
            button1.Enabled = true;
            panel1.Enabled = false;
        }
        private void calculateRoles()
        {
            Point p1 = new Point(1,1), p2 = new Point(1,1);
            roleusers = new List<HashSet<int>>();
            List<Point>[] ppp = new List<Point>[p.Length];
            for (int i = 0; i < p.Length; i++)
            {
                ppp[i] = new List<Point>();
                foreach (Point ap in p[i])
                    ppp[i].Add(ap);
            }
            
            for (int i = 0; i < ppp.Length; i++)
            {
                if (ppp[i].Count == 0)
                    continue;
                HashSet<int> tmp = new HashSet<int>();
                tmp.Add(i);
                for (int j = i+1; j < ppp.Length; j++)
                {
                    bool same = false;
                    if ((ppp[j].Count>0) && (ppp[i].Count == ppp[j].Count))
                    {
                        same = true;
                        for (int k = 0; k < ppp[i].Count; k++)
                        {
                            if (!(ppp[i][k].Equals(ppp[j][k])))
                            {
                                same = false;
                                break;
                            }
                        }
                    }
                    if (same)
                    {
                        ppp[j].Clear();
                        tmp.Add(j);
                        MessageBox.Show("Permissions for U" + (i + 1) + " = U" + (j + 1) + " are same.");
                    }
                }
                roleusers.Add(tmp);
            }
            rl = new Label[roleusers.Count];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            usR = new Label[int.Parse(textBox1.Text)];
            tsR = new Label[int.Parse(textBox2.Text)];
            Load_ControlsA();            
            calculateRoles();
            Load_Roles();
            render = true;
            renderR = true;
            Refresh();

        }
        private void Load_Roles()
        {
            for (int i = 0; i < rl.Length; i++)
            {
                rl[i] = new Label();
                rl[i].Text = "R" + (i + 1);
                rl[i].Width = 30;
                rl[i].AutoSize = true;
                rl[i].Top = 445;
                rl[i].Left = rl[i].Width * i + 10;
                Controls.Add(rl[i]);
            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            NumbersOnly(sender, e);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            NumbersOnly(sender, e);
        }

        private void NumbersOnly(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar != 8)
            if (!(e.KeyChar >= 48 && e.KeyChar <= 57))
                e.Handled = true;
            else
                e.Handled = false;
        }
    }
}