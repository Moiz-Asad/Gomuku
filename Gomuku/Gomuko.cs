using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gomuku
{
    public partial class Gomuko : Form
    {
        int Dim;
        Cell[,] Cs;
        Cell cc;
        int WC;
        COLOR Turn = COLOR.RED;
        public Gomuko()
        {
            InitializeComponent();
        }
        void TURNChange()
        {
            if(Turn == COLOR.RED)
            {
                Turn = COLOR.BLACK;
            }
            else
            {
                Turn = COLOR.RED;
            }
        }
        private void InIt()
        {
            Grid.Controls.Clear();
            Cs = new Cell[Dim, Dim];
            for (int i = 0; i < Dim; i++)
            {
                for (int j = 0; j < Dim; j++)
                {
                    int H = Grid.Height / Dim - 10;
                    int W = Grid.Width / Dim - 10;
                    Cell C = new Cell(H, W);
                    Grid.Controls.Add(C);
                    if (radioButton4.Checked == true)
                    {
                        C.Click += new System.EventHandler(WhenCellSelectHVH);
                        Cs[i, j] = C;
                    }
                    else
                        if (radioButton5.Checked == true)
                        {
                            C.Click += new System.EventHandler(WhenCellSelectHVC);
                            Cs[i, j] = C;
                        }
                        else
                        {
                            Random r = new Random();
                            int a = r.Next(0, 10);
                            if (a % 2 == 0)
                            {
                                radioButton4.Checked = true;
                                C.Click += new System.EventHandler(WhenCellSelectHVH);
                                Cs[i, j] = C;
                            }
                            else
                            {
                                radioButton5.Checked = true;
                                C.Click += new System.EventHandler(WhenCellSelectHVC);
                                Cs[i, j] = C;
                            }
                        }
                }
            }
        }
        void WhenCellSelectHVH(object sender, EventArgs e)
        {
           cc = (Cell)sender;
           if( cc.Occupier != COLOR.WHITE )
           {
               MessageBox.Show("Wrong Move!");
               return;
           }
           //cc.Occupier = Turn;
           UpdateBoard();
           if(IsWin(Turn))
           {
               if (cc.Occupier == COLOR.RED)
               {
                   String Red = "Red Wins";
                   MessageBox.Show(Red);
                   Grid.Controls.Clear();
               }
               else
               if(cc.Occupier == COLOR.BLACK)
               {
                   String Blue = "Blue Wins";
                   MessageBox.Show(Blue);
                   Grid.Controls.Clear();
               }
           }
           if (IsDraw())
           {
               MessageBox.Show("Game Is Draw!");
               Grid.Controls.Clear();
           }
           TURNChange();
        }

        void ComputerMove()
        {
            int temp = WC;
            Random R = new Random();
            int ri = 0, ci = 0;
            do
            {
                ri = R.Next(0, Dim);
                ci = R.Next(0, Dim);
            }
            while (Cs[ri, ci].Occupier != COLOR.WHITE);
            cc = Cs[ri, ci];
            for (int i = 0; i < Dim; i++)
            {
                for (ri = 0; ri < Dim; ri++)
                {
                    for (ci = 0; ci < Dim; ci++)
                    {
                        if (Cs[ri, ci].Occupier == COLOR.WHITE)
                        {
                            Cs[ri, ci].Occupier = Turn;
                            if (IsWin(Turn))
                            {
                                Cs[ri, ci].Occupier = COLOR.WHITE;
                                cc = Cs[ri, ci];
                                return;
                            }
                            else
                            {
                                Cs[ri, ci].Occupier = COLOR.WHITE;
                            }
                        }
                    }
                }
                temp--;
            }
        }
        void WhenCellSelectHVC(object sender, EventArgs e)
        {
            cc = (Cell)sender;
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    if (cc.Occupier != COLOR.WHITE)
                    {
                        MessageBox.Show("Wrong Move!");
                        return;
                    }
                }
                else
                {
                   ComputerMove();
                }

                UpdateBoard();
                if (IsWin(Turn))
                {
                    if (cc.Occupier == COLOR.RED)
                    {
                        String Red = "Red Wins";
                        MessageBox.Show(Red);
                        Grid.Controls.Clear();
                    }
                    else
                    if (cc.Occupier == COLOR.BLACK)
                    {
                        String Blue = "Blue Wins";
                        MessageBox.Show(Blue);
                        Grid.Controls.Clear();
                    }
                }
                if (IsDraw())
                {
                    MessageBox.Show("Game Is Draw!");
                    Grid.Controls.Clear();
                }
                TURNChange();
            }
        }
        void UpdateBoard()
        {
            cc.Occupier = Turn;
            if(Turn == COLOR.BLACK)
            {
                cc.BackColor = Color.Blue;
            }
            else
            {
                cc.BackColor = Color.Red;
            }
        }
        bool IsHorizontal(int ri, int ci, COLOR T)
        {
            if(ci+ WC > Dim)
            {
                return false;
            }
            for(int i=0;i<WC;i++)
            {
                if (Cs[ri, ci + i].Occupier != Turn)
                {
                    return false;
                }
            }
            return true;
        }
        bool IsVertical(int ri, int ci, COLOR T)
        {
            if (ri + WC > Dim)
            {
                return false;
            }
            for (int i = 0; i < WC; i++)
            {
                if (Cs[ri + i, ci].Occupier != Turn)
                {
                    return false;
                }
            }
            return true;
        }
        bool IsDiagonalL(int ri, int ci, COLOR T)
        {
            if (ri + WC > Dim || ci + WC > Dim)
            {
                return false;
            }
            for (int i = 0; i < WC; i++)
            {
                if (Cs[ri + i, ci + i].Occupier != Turn)
                {
                    return false;
                }
            }
            return true;
        }
        bool IsDiagonalR(int ri, int ci, COLOR T)
        {
            if(ri + WC > Dim || ci + WC > Dim)
            {
                return false;
            }
            else
            {
                int a=WC-1;
                int b=ci;
                for (int i = 0; i < WC; i++)
                {
                    if (Cs[a , b].Occupier != Turn)
                    {
                        return false;
                    }
                    a--;
                    b++;
                }
                return true;
            }
        }
        bool IsDraw()
        {
            for(int i=0;i<Dim;i++)
            {
                for(int j=0;j<Dim;j++)
                {
                    if (Cs[i, j].Occupier == COLOR.WHITE)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        bool IsWin(COLOR T)
        {
            for (int i = 0; i < Dim; i++)
            {
                for (int j = 0; j < Dim; j++)
                {
                    if (DoIWinHere(i, j, T))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        bool DoIWinHere(int ri, int ci, COLOR T)
        {
            return IsHorizontal(ri, ci, T) || IsVertical(ri, ci, T) || IsDiagonalL(ri, ci, T) || IsDiagonalR(ri, ci, T);
        }
        private void Start_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked == true)
            {
                Dim = 5;
                WC = Dim;
            }
            else  if(radioButton2.Checked == true)
            {
                Dim = 10;
                WC = Dim;
            }
            else if (radioButton3.Checked == true)
            {
                Dim = 15;
                WC = Dim;
            }
            else
            {
                MessageBox.Show("Please Select Grid Size!");
            }
            InIt();
        }

        private void Grid_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Gomuko_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

    }
}
