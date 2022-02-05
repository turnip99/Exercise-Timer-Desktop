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
using System.IO;

namespace Exercise_Timer
{
    /// <summary>
    /// Interaction logic for GenerateWindow.xaml
    /// </summary>
    public partial class GenerateWindow : Window
    {
        Exercise_Selecter selecter;
        char[] data;
        public GenerateWindow()
        {
            InitializeComponent();
            StreamReader file = new StreamReader(@"./Exercises.txt");
            data = file.ReadLine().ToCharArray();
            if (data[data.Length - 2] == 'Y')
            {
                CheckBand.IsChecked = true;
            }
            else
            {
                CheckBand.IsChecked = false;
            }
            if (data[data.Length - 4] == 'Y')
            {
                CheckWall.IsChecked = true;
            }
            else
            {
                CheckWall.IsChecked = false;
            }
            if (data[data.Length - 6] == 'Y')
            {
                CheckFloor.IsChecked = true;
            }
            else
            {
                CheckFloor.IsChecked = false;
            }
            file.Close();
            Button30.Click += (sender, EventArgs) => { Button_Click(sender, EventArgs, 30); };
            Button45.Click += (sender, EventArgs) => Button_Click(sender, EventArgs, 45);
            Button60.Click += (sender, EventArgs) => Button_Click(sender, EventArgs, 60);
            Button90.Click += (sender, EventArgs) => Button_Click(sender, EventArgs, 90);
            Button120.Click += (sender, EventArgs) => Button_Click(sender, EventArgs, 120);
            Button150.Click += (sender, EventArgs) => Button_Click(sender, EventArgs, 150);
            Button180.Click += (sender, EventArgs) => Button_Click(sender, EventArgs, 180);
            Button240.Click += (sender, EventArgs) => Button_Click(sender, EventArgs, 240);
            Button300.Click += (sender, EventArgs) => Button_Click(sender, EventArgs, 300);
            Button600.Click += (sender, EventArgs) => Button_Click(sender, EventArgs, 600);
            Button900.Click += (sender, EventArgs) => Button_Click(sender, EventArgs, 900);
            Button1200.Click += (sender, EventArgs) => Button_Click(sender, EventArgs, 1200);
        }

        private void Button_Click(object sender, EventArgs e, int time)
        {
            Scrollview1.ScrollToTop();
            if (CheckBand.IsChecked == true)
            {
                data[data.Length - 2] = 'Y';
            }
            else
            {
                data[data.Length - 2] = 'N';
            }
            if (CheckWall.IsChecked == true)
            {
                data[data.Length - 4] = 'Y';
            }
            else
            {
                data[data.Length - 4] = 'N';
            }
            if (CheckFloor.IsChecked == true)
            {
                data[data.Length - 6] = 'Y';
            }
            else
            {
                data[data.Length - 6] = 'N';
            }
            StringBuilder builder = new StringBuilder();
            foreach (char item in data)
            {
                builder.Append(item);
            }
            FileStream fs = new FileStream(@"./Exercises.txt", FileMode.Open);
            byte[] info = new UTF8Encoding(true).GetBytes(builder.ToString());
            fs.Write(info, 0, info.Length);
            fs.Close();
            selecter = new Exercise_Selecter(time, (bool)CheckFloor.IsChecked, (bool)CheckWall.IsChecked, (bool)CheckBand.IsChecked);
            string text = selecter.GetExercises();
            txtExercises.Text = text;
        }
    }
}
