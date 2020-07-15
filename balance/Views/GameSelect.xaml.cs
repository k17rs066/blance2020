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
using System.Windows.Threading;

namespace balance.Views
{
    /// <summary>
    /// GameSelect.xaml の相互作用ロジック
    /// </summary>
    public partial class GameSelect : Page
    {
        public GameSelect()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();



        }



        void timer_Tick(object sender, EventArgs e)
        {
            datetimeb.Text = DateTime.Now.ToString();
        }

        private void back(object sender, RoutedEventArgs e)
        {
            var nextPage = new k_Top();
            NavigationService.Navigate(nextPage);
        }

        private void attack(object sender, RoutedEventArgs e)
        {
            var nextPage = new GameMode();
            NavigationService.Navigate(nextPage);
        }

        private void target(object sender, RoutedEventArgs e)
        {
            var nextPage = new TargetModeSelect();
            NavigationService.Navigate(nextPage);
        }

        private void fall(object sender, RoutedEventArgs e)
        {
            var nextPage = new FTGameModeSelect();
            NavigationService.Navigate(nextPage);
        }
    }
}
