using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace balance.Views
{
    /// <summary>
    /// MazeGame.xaml の相互作用ロジック
    /// </summary>
    public partial class MazeGame : Page
    {
        public MazeGame()
        {
            InitializeComponent();
        }

        private void printPos(UIElement el)
        {
            int x = (int)Canvas.GetLeft(el);
            int y = (int)Canvas.GetTop(el);
            //textPos.Text = string.Format("x:{0} y:{1}", x, y);
        }

        private bool _isDrag = false;
        private System.Windows.Point _dragOffset;

        /// <summary>
        /// ドラッグ開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mark0_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UIElement el = sender as UIElement;
            if (el != null)
            {
                _isDrag = true;
                _dragOffset = e.GetPosition(el);
                el.CaptureMouse();
            }
        }

        /// <summary>
        /// ドラッグ終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mark0_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDrag == true)
            {
                UIElement el = sender as UIElement;
                el.ReleaseMouseCapture();
                _isDrag = false;
            }
        }

        /// <summary>
        /// ドラック中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mark0_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrag == true)
            {
                System.Windows.Point pt = Mouse.GetPosition(beback);
                UIElement el = sender as UIElement;
                Canvas.SetLeft(el, pt.X - _dragOffset.X);
                Canvas.SetTop(el, pt.Y - _dragOffset.Y);
                printPos(el);
            }

          

        }

        private void start(object sender, RoutedEventArgs e)
        {

        }

        private void back(object sender, RoutedEventArgs e)
        {
            var nextPage = new k_Top();
            NavigationService.Navigate(nextPage);
        }
    }
}
