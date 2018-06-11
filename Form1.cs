using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class window : Form
    {
        List<Panel> tail = new List<Panel>();
        int xspeed = 1;
        int yspeed = 0;
        public window()
        {
            InitializeComponent();
            resetFood();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            updateGame();
        }

        private void updateGame()
        {
            if (checkIfEnd())
                endGame();
            tryEat();
            moveSnake();
        }

        private void endGame()
        {
            foreach (Panel p in tail)
                p.Dispose();
            tail.Clear();
            timer1.Interval = 100;
        }

        private bool checkIfEnd()
        {
            foreach(Panel p in tail)
            {
                if(headoverlapping(p))
                {
                    return true;
                }
            }
            if (tail.Count == table.RowCount * table.ColumnCount - 1)
                return true;
            return false;
        }

        private void resetFood()
        {
            food.Hide();
            Random rnd = new Random();
            int x = rnd.Next(0, table.ColumnCount);
            int y = rnd.Next(0, table.RowCount);
            while (!isOk(x, y))
            {
                x = rnd.Next(0, table.ColumnCount);
                y = rnd.Next(0, table.RowCount);
            }
            table.SetCellPosition(food, new TableLayoutPanelCellPosition(x, y));
            food.Show();
        }

        private bool isOk(int x, int y)
        {
            foreach(Panel p in tail)
            {
                if (table.GetCellPosition(p) == new TableLayoutPanelCellPosition(x, y))
                    return false;
            }
            if (table.GetCellPosition(head) == new TableLayoutPanelCellPosition(x, y))
                return false;
            if (table.GetCellPosition(food) == new TableLayoutPanelCellPosition(x, y))
                return false;
            return true;
        }
        private void tryEat()
        {
            if(headoverlapping(food))
            {
                Panel extend = new Panel();
                extend.BackColor = head.BackColor;
                extend.Size = head.Size;
                extend.Margin = head.Margin;
                table.Controls.Add(extend);
                extend.Dock = DockStyle.Fill;
                tail.Add(extend);
                extend.Hide();
                timer1.Interval += timer1.Interval / 100 * (-1);
            }
        }

        private void moveSnake()
        {
            int x = table.GetCellPosition(head).Column;
            int y = table.GetCellPosition(head).Row;
            movehead(xspeed, yspeed);
            movetail(x, y);
        }
        
        private void movehead(int x, int y)
        {
            int col = (table.GetCellPosition(head).Column + x) % table.ColumnCount;
            int row = (table.GetCellPosition(head).Row + y) % table.RowCount;
            if (table.GetCellPosition(head).Column == 0 && xspeed == -1)
            {
                col = table.ColumnCount-1;
            }
            if (table.GetCellPosition(head).Row == 0 && yspeed == -1)
            {
                row = table.RowCount-1;
            }

            table.SetCellPosition(head, new TableLayoutPanelCellPosition(col, row));
        }

        private void movetail(int x, int y)
        {
            if (tail.Count > 0)
            {
                table.SetCellPosition(tail[tail.Count - 1], new TableLayoutPanelCellPosition(x, y));
                Panel move = tail.Last();
                tail.Insert(0, move);
                tail[0].Show();
                tail.RemoveAt(tail.Count - 1);
            }
        }

        private int panelcol(Panel p)
        {
            return table.GetCellPosition(p).Column;
        }

        private int panelrow(Panel p)
        {
            return table.GetCellPosition(p).Row;
        }

        private bool headoverlapping(Panel p)
        {
            if (table.GetCellPosition(p) == new TableLayoutPanelCellPosition(panelcol(head) + xspeed, panelrow(head) + yspeed))
            {
                resetFood();
                return true;
            }
                
            return false;
        }

        private void window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Up && (yspeed!=1 || tail.Count==0))
            {
                xspeed = 0;
                yspeed = -1;
            }
            else if(e.KeyCode == Keys.Down && (yspeed != -1 || tail.Count == 0))
            {
                xspeed = 0;
                yspeed = 1;
            }
            else if (e.KeyCode == Keys.Right && (xspeed != -1 || tail.Count == 0))
            {
                xspeed = 1;
                yspeed = 0;
            }
            else if (e.KeyCode == Keys.Left && (xspeed != 1 || tail.Count == 0))
            {
                xspeed = -1;
                yspeed = 0;
            }
        }
    }
}
