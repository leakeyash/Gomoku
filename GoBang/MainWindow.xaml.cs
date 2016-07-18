using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GoBang
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _interval = 20;
        private int _lineNum = 15;
        private int _margin = 10;
        int[,] _chessArray = new int[15, 15];
        private int _chessCount = 0;
        public MainWindow()
        {
            InitializeComponent();
            DrawBord();

        }

        public void DrawBord()
        {
            int interval = _interval;
            int rowNum = _lineNum, columnNum = _lineNum;
            int width = interval * (columnNum - 1), height = interval * (rowNum - 1);
            int margin = _margin;

            for (int i = 0; i < rowNum; i++)
            {
                Line rowLine = new Line();
                rowLine.X1 = margin + 0;
                rowLine.X2 = margin + width;
                rowLine.Y1 = margin + i * interval;
                rowLine.Y2 = rowLine.Y1;
                rowLine.Stroke = Brushes.Black;
                this.ChessBoard.Children.Add(rowLine);
            }

            for (int i = 0; i < columnNum; i++)
            {
                Line Line = new Line();
                Line.X1 = margin + i * interval;
                Line.X2 = margin + i * interval;
                Line.Y1 = margin;
                Line.Y2 = margin + height;
                Line.Stroke = Brushes.Black;
                this.ChessBoard.Children.Add(Line);
            }

        }

        private void ChessBoard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var p = Mouse.GetPosition(ChessBoard);
            double x = p.X - _margin;
            double y = p.Y - _margin;
            int xNum = (int)(x / _interval);
            int yNum = (int)(y / _interval);
            double xD = x % _interval;
            double yd = y % _interval;
            if (xD > (_interval / 2)) xNum++;
            if (yd > (_interval / 2)) yNum++;

            int actualLeft = _margin + xNum * _interval - _interval / 2;
            int actualTop = _margin + yNum * _interval - _interval / 2;
            //if (_chesses.ContainsKey(actualLeft) && _chesses[actualLeft] == actualTop) return;

            //_chesses.Add(actualLeft,actualTop);
            if (_chessArray[xNum, yNum] != 0) return;

            _chessCount++;

            bool isBlack = true;
            if (_chessCount%2 == 0)
            {
                _chessArray[xNum, yNum] = 2;
                isBlack = false;
            }          
            else
            {
                _chessArray[xNum, yNum] = 1;
                isBlack = true;
            }
                     
            Ellipse ep = new Ellipse();
            ep.Height = ep.Width = _interval;
            //ep.Fill = e.ClickCount/2 == 0 ? Brushes.Black : Brushes.White;
            ep.Fill = isBlack ? Brushes.Black : Brushes.White;
            ChessBoard.Children.Add(ep);
            Canvas.SetLeft(ep, actualLeft);
            Canvas.SetTop(ep, actualTop);

            if (CheckWinner(xNum, yNum, _chessArray[xNum, yNum]))
            {
                if (isBlack)
                {
                    MessageBox.Show("Black Win!");
                }
                else
                {
                    MessageBox.Show("White Win!");
                }
                ClearChesses();
            }
        }

        public bool CheckWinner(int x, int y, int origin)
        {
            if (CheckLeftRight(x, y, origin) || CheckTopBottom(x, y, origin) || CheckLeftTopToRightBottom(x, y, origin) ||
                CheckLeftBottomToRightTop(x, y, origin))
                return true;
           // return CheckLeftRight(x, y, origin);
            return false;
        }

        private bool CheckLeftRight(int x, int y, int origin)
        {
            int left = x, right = x;
            while (left - 1 >= 0)
            {
                if (_chessArray[left - 1, y] == origin)
                    left = left - 1;
                else
                {
                    break;
                }
            }
            if (right - left + 1 >= 5) return true;

            while (right + 1 < 15)
            {
                if (_chessArray[right + 1, y] == origin)
                    right = right + 1;
                else
                {
                    break;
                }
            }
            if (right - left + 1 >= 5) return true;

            return false;
        }

        private bool CheckTopBottom(int x, int y, int origin)
        {
            int top = y, bottom = y;
            while (top - 1 >= 0)
            {
                if (_chessArray[x, top - 1] == origin)
                    top = top - 1;
                else
                {
                    break;
                }
            }
            if (bottom - top + 1 >= 5) return true;
            while (bottom + 1 < 15)
            {
                if (_chessArray[x, bottom + 1] == origin)
                    bottom = bottom + 1;
                else
                {
                    break;
                }
            }
            if (bottom - top + 1 >= 5) return true;

            return false;
        }

        private bool CheckLeftTopToRightBottom(int x, int y, int origin)
        {
            int left = x, right = x;
            int top = y, bottom = y;

            int sum = 0;
            while (left - 1 >= 0&&top-1>=0)
            {
                if (_chessArray[left - 1, top - 1] == origin)
                {
                    left = left - 1;
                    top = top - 1;
                    sum++;
                }
                else
                {
                    break;
                }
            }
            if (sum + 1 >= 5) return true;

            while (right + 1 < 15&&bottom+1<15)
            {
                if (_chessArray[right + 1, bottom + 1] == origin)
                {
                    right = right + 1;
                    bottom = bottom + 1;
                    sum++;
                }
                else
                {
                    break;
                }
            }
            if (sum + 1 >= 5) return true;

            return false;
        }

        private bool CheckLeftBottomToRightTop(int x, int y, int origin)
        {
            int left = x, right = x;
            int top = y, bottom = y;

            int sum = 0;
            while (left - 1 >= 0 && bottom + 1 < 15)
            {
                if (_chessArray[left - 1, bottom + 1] == origin)
                {
                    left = left - 1;
                    bottom = bottom + 1;
                    sum++;
                }
                else
                {
                    break;
                }
            }
            if (sum + 1 >= 5) return true;

            while (right + 1 < 15 && top - 1 >=0)
            {
                if (_chessArray[right + 1, top - 1] == origin)
                {
                    right = right + 1;
                    top = top - 1;
                    sum++;
                }
                else
                {
                    break;
                }
            }
            if (sum + 1 >= 5) return true;

            return false;
        }

        private void ClearChesses()
        {
            for (int i = 0; i < ChessBoard.Children.Count; i++)
            {
                if (ChessBoard.Children[i] is Ellipse)
                {
                    ChessBoard.Children.Remove(ChessBoard.Children[i]);
                    i--;
                }
            } 
            _chessArray=new int[15,15];          
        }

        private void NewGame_OnClick(object sender, RoutedEventArgs e)
        {
            ClearChesses();
        }
    }
}
