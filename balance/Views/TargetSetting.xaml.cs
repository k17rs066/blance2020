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

namespace balance.Views
{
    /// <summary>
    /// TargetSetting.xaml の相互作用ロジック
    /// </summary>
    public partial class TargetSetting : Page
    {
        public TargetSetting()
        {
            InitializeComponent();


            GameMode.Items.Add("通常モード");
            GameMode.Items.Add("練習モード");

            GameMode.SelectedIndex = 0;

            Placement.Items.Add("パターン1");
            Placement.Items.Add("パターン2");
            Placement.Items.Add("パターン3");

            Placement.SelectedIndex = 0;
        }

        private void back(object sender,RoutedEventArgs e)
        {
            var nextPage = new GameSelect();
            NavigationService.Navigate(nextPage);
        }

        private void Combo_GameMode(object sender,RoutedEventArgs e)
        {
            if (GameMode.Text =="通常モード") 
            {
                Plecement_Label.Visibility = Visibility.Visible;
                Placement.Visibility = Visibility.Visible;

            }
            else if (GameMode.Text == "練習モード") 
            {
                Plecement_Label.Visibility = Visibility.Hidden;
                Placement.Visibility = Visibility.Hidden;
            }
        }

        private void decision(object sender, RoutedEventArgs e)
        {
            if (GameMode.Text == "通常モード")
            {
                if (Placement.Text == "パターン1")
                {
                    var nextPage = new TargetGame();
                    NavigationService.Navigate(nextPage);
                }
                else if (Placement.Text == "パターン2")
                {
                    var nextPage = new TargetGame_p2();
                    NavigationService.Navigate(nextPage);
                }
                else if (Placement.Text == "パターン3")
                {
                    var nextPage = new TargetGame_p3();
                    NavigationService.Navigate(nextPage);
                }
            }
            else if (GameMode.Text == "練習モード")
            {
                var nextPage = new TargetGame_Prac();
                NavigationService.Navigate(nextPage);
            }
         }

        private void Placement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void GameMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GameMode.Text == "通常モード")
            {
                Plecement_Label.Visibility = Visibility.Hidden;
                Placement.Visibility = Visibility.Hidden;


            }
            else if (GameMode.Text == "練習モード")
            {
                Plecement_Label.Visibility = Visibility.Visible;
                Placement.Visibility = Visibility.Visible;
            }
        }
    }
}
