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
using System.Windows.Shapes;

namespace balance.Views
{
    /// <summary>
    /// Window3.xaml の相互作用ロジック
    /// </summary>
    public partial class TagSetting : Window
    {
        //public GameOfTag.Refresh GetRefresh = null;

        public double ReciveSpeed { get; set; }
        public int ReciveSize { get; set; }
        public int ReciveTime { get; set; }

        int timeset_min = 0;
        int timeset_sec = 0;


        public TagSetting()
        {
            InitializeComponent();

            OrgSpeed.Items.Add("速い");
            OrgSpeed.Items.Add("普通");
            OrgSpeed.Items.Add("遅い");

            OrgSize.Items.Add("大きい");
            OrgSize.Items.Add("普通");
            OrgSize.Items.Add("小さい");
 

            OrgSpeed.SelectedIndex = 1;
            OrgSize.SelectedIndex = 1;

            for (int i = 0; i < 6; i++)
            {
                TimeSet_min.Items.Add(timeset_min);

                if (timeset_min != 5)
                {
                    timeset_min += 1;
                }
            }

            TimeSet_min.SelectedIndex = 0;



            for (int i = 0; i < 6; i++)
            {
                TimeSet_sec.Items.Add(timeset_sec);

                if(timeset_sec!= 50)
                {
                    timeset_sec += 10;
                }
            }

            TimeSet_sec.SelectedIndex = 3;


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (OrgSpeed.Text == "速い")
            {
                ReciveSpeed = 1.0;
                Application.Current.Properties["TagSpeed"] = "速い";
            }
            else if (OrgSpeed.Text == "普通")
            {
                ReciveSpeed = 0.75;
                Application.Current.Properties["TagSpeed"] = "普通";
            }
            else if (OrgSpeed.Text == "遅い")
            {
                ReciveSpeed = 0.5;
                Application.Current.Properties["TagSpeed"] = "遅い";
            }
            else
            {
                ReciveSpeed = 0.75;
                Application.Current.Properties["TagSpeed"] = "普通";
            }

            if (OrgSize.Text == "大きい")
            {
                ReciveSize = 250;
                Application.Current.Properties["TagSize"] = "大きい";
            }
            else if (OrgSize.Text == "普通")
            {
                ReciveSize = 200;
                Application.Current.Properties["TagSize"] = "普通";
            }
            else if (OrgSize.Text == "小さい")
            {
                ReciveSize = 150;
                Application.Current.Properties["TagSize"] = "小さい";
            }
            else
            {
                ReciveSpeed = 200;
                Application.Current.Properties["TagSize"] = "普通";
            }
            ReciveTime = int.Parse(TimeSet_min.Text) * 60 + int.Parse(TimeSet_sec.Text);

            // MessageBox.Show("設定の変更が完了しました。");

            this.Close();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ReciveTime = 30;
            this.Close();
        }
    }
}
