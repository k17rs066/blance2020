using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

using balance.DataBase;

namespace balance.Views
{

    
    /// <summary>
    /// Window1.xaml の相互作用ロジック
    /// </summary>
    public partial class Window1 : Window
    {
        int timese;

        public Window1()
        {
            InitializeComponent();
            
            //スコア設定の表示
            for (int i = 50; i <= 100; i += 10)
            {
                if (i == timese)
                {
                    timepercent.Items.Add(new ComboBoxItem() { IsSelected = true, Content = i });
                }
                else
                {
                    timepercent.Items.Add(i);
                }
            }

        }
        public void show()
        {
            DBConnect.Connect("kasiihara.db");
            timese = int.Parse(timepercent.Text);
            string SQLa;
            SQLa = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = 12 and clear_line = " + timese + " and traintype = 'スコアアタック' ";
            //SQLa = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = 10 and clear_line = 50 and traintype = 'タイムアタック' ";
            DBConnect.ExecuteReader(SQLa);


            List<KeyValuePair<int , int>> timevalueList = new List<KeyValuePair<int, int>>();

            //List list = new List<string>();

            while (DBConnect.Reader.Read())
            {

                int clear_record = int.Parse(DBConnect.Reader[3].ToString());
                var date =  " ";
                //var data = date.GetString(DBConnect.Reader[7].ToString());
                //list.Add(DBConnect.Reader[7].ToString().Substring(5, 5));


                //timevalueList.Add(new KeyValuePair<int, int>(date, clear_record));

                Console.WriteLine(DBConnect.Reader[7].ToString() + "," + clear_record);

                

            }

            //Setting data for line chart
            Chart.DataContext = timevalueList;

            //aaa = timevalueList;
            DBConnect.Dispose();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            show();
        }
    }
}
