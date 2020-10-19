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
        public GameOfTag.Refresh GetRefresh = null;

        public double ReciveSpeed { get; set; }
        public int ReciveTime { get; set; }

        int timeset = 15; 
        public TagSetting()
        {
            InitializeComponent();

            OrgSpeed.Items.Add("速い");
            OrgSpeed.Items.Add("普通");
            OrgSpeed.Items.Add("遅い");




            OrgSpeed.SelectedIndex = 1;




            for (int i = 0; i < 4; i++)
            {
                TimeSet.Items.Add(timeset);

                if(timeset!= 90)
                {
                    timeset += 15;
                }
            }

            TimeSet.SelectedIndex = 1;


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (OrgSpeed.Text == "速い")
            {
                ReciveSpeed = 1.5;
            }
            else if (OrgSpeed.Text == "普通")
            {
                ReciveSpeed = 1.0;
            }
            else if (OrgSpeed.Text == "遅い")
            {
                ReciveSpeed = 0.5;
            }

            ReciveTime = int.Parse(TimeSet.Text);
            // MessageBox.Show("設定の変更が完了しました。");

            this.Close();
        }
    }
}
