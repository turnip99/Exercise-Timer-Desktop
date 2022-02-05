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
    /// Interaction logic for TimerWindow.xaml
    /// </summary>
    public partial class TimerWindow : Window
    {
        Exercise_Selecter selecter;
        char[] data;
        List<string> list = new List<string>();
        public TimerWindow()
        {
            InitializeComponent();
            KeyDown += new KeyEventHandler(ManageWindow_KeyDown);
            ButtonEnter.Click += ButtonEnter_Click;
            StreamReader file = new StreamReader(@"./Exercises.txt");
            data = file.ReadLine().ToCharArray();
            string line;
            while ((line = file.ReadLine()) != null)
            {
                list.Add(line);
            }
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
            int i = 0;
            string temp = "";
            while (data[i] != '/')
            {
                temp += data[i];
                i++;
            }
            txtTime.Text = ConvertToTime(int.Parse(temp));
            i += 1;
            temp = "";
            while (data[i] != '/')
            {
                temp += data[i];
                i++;
            }
            txtBreak.Text = ConvertToTime(int.Parse(temp));
            file.Close();
        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            Enter();
        }

        private void ManageWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Enter();
            }
        }

        private void Enter()
        {
            int time = ConvertToMinutes(txtTime.Text);
            if (time == 0)
            {
                MessageBox.Show("Invalid work time input");
                return;
            }
            int Break = ConvertToMinutes(txtBreak.Text);
            if (Break == 0)
            {
                MessageBox.Show("Invalid exercise time input");
                return;
            }
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
            builder.Append(time.ToString() + "/");
            builder.Append(Break.ToString() + "/");
            builder.Append(data[data.Length - 6] + "/");
            builder.Append(data[data.Length - 4] + "/");
            builder.AppendLine(data[data.Length - 2] + "/");
            for (int i = 0; i < list.Count; i++)
            {
                if (i < (list.Count-1))
                {
                    builder.AppendLine(list[i]);
                }
                else
                {
                    builder.Append(list[i]);
                }
            }
            FileStream fs = new FileStream(@"./Exercises.txt", FileMode.Open);
            byte[] info = new UTF8Encoding(true).GetBytes(builder.ToString());
            fs.SetLength(0);
            fs.Write(info, 0, info.Length);
            fs.Close();
            selecter = new Exercise_Selecter(Break, (bool)CheckFloor.IsChecked, (bool)CheckWall.IsChecked, (bool)CheckBand.IsChecked);
            RunWindow RunWindow = new RunWindow(selecter, time, Break);
            RunWindow.ShowDialog();
        }

        private int ConvertToMinutes(string text)
        {
            int temp;
            if (int.TryParse(text, out temp) && temp > 0)
            {
                return temp;
            }
            else
            {
                text.ToCharArray();
                int stage = 1;
                string before = "";
                string after = "";
                for (int i = 0; i < text.Length; i++)
                {
                    if (stage == 1)
                    {
                        if (int.TryParse(text[i].ToString(), out temp))
                        {
                            before += text[i].ToString();
                        }
                        else if (text[i] == ':')
                        {
                            stage = 2;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else if (stage == 2)
                    {
                        if (int.TryParse(text[i].ToString(), out temp) && (text[i - 1] != ':' || temp < 7))
                        {
                            after += text[i].ToString();
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                if (after.Length != 2 || int.Parse(after) >= 60)
                {
                    return 0;
                }
                int minutes = 0;
                minutes += (60 * int.Parse(before));
                minutes += int.Parse(after);
                return minutes;
            }
        }

        private string ConvertToTime(int seconds)
        {
            string time = "";
            time += (seconds / 60).ToString();
            time += ":";
            string temp = (seconds % 60).ToString();
            if (temp.Length != 2)
            {
                string x = "0";
                x += temp;
                temp = x;
            }
            time += temp;
            return time;
        }
    }
}
