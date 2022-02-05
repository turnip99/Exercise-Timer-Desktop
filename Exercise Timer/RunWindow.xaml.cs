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
using System.Media;
using System.Windows.Threading;

namespace Exercise_Timer
{
    /// <summary>
    /// Interaction logic for RunWindow.xaml
    /// </summary>
    public partial class RunWindow : Window
    {
        SoundPlayer DoIt = new SoundPlayer(Properties.Resources.Senator_Palpatine___Do_it);
        SoundPlayer MinorBeep = new SoundPlayer(Properties.Resources.Minor_Beep);
        SoundPlayer MajorBeep = new SoundPlayer(Properties.Resources.Major_Beep);
        Exercise_Selecter selecter;
        int time = 0;
        int totalTime = 0;
        char state = 'W';
        int breakTime;
        int workTime;
        int reminderTime = 0;
        DispatcherTimer totaltimer = new DispatcherTimer();
        DispatcherTimer timer = new DispatcherTimer();
        public RunWindow(Exercise_Selecter selecter, int workTime, int breakTime)
        {
            InitializeComponent();
            KeyDown += new KeyEventHandler(ManageWindow_KeyDown);
            btnContinue.Click += ButtonEnter_Click;
            this.selecter = selecter;
            this.breakTime = breakTime;
            this.workTime = workTime;
            time = workTime;
            txtTotal.Text = ConvertToTime(totalTime);
            txtCurrent.Text = ConvertToTime(time);
            totaltimer.Tick += totalTimer_Tick;
            totaltimer.Interval = new TimeSpan(0, 0, 1);
            totaltimer.Start();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void totalTimer_Tick(object sender, EventArgs e)
        {
            InvalidateVisual();
            if (state == 'P')
            {
                reminderTime++;
            }
            if (reminderTime >= 40 && reminderTime % 40 == 0)
            {
                DoIt.Play();
            }
            totalTime++;
            txtTotal.Text = ConvertToTime(totalTime);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            switch (time)
            {
                case 4: MinorBeep.Play(); break;
                case 3: MinorBeep.Play(); break;
                case 2: MinorBeep.Play(); break;
                default: break;
            }
            switch (state)
            {
                case 'W':
                    time--;
                    if (time == 0)
                    {
                        InvalidateVisual();
                        MajorBeep.Play();
                        state = 'P';
                        lblCurrent.Content = "Break:";
                        Background = Brushes.Bisque;
                        btnContinue.Content = "Continue";
                        btnContinue.Background = Brushes.LightGoldenrodYellow;
                        timer.Stop();
                    }
                    txtCurrent.Text = ConvertToTime(time);
                    break;
                case 'B':
                    time--;
                    InvalidateVisual();
                    if (time == 0)
                    {
                        MajorBeep.Play();
                        state = 'W';
                        time = workTime;
                        txtExercises.Text = "";
                        lblCurrent.Content = "Work:";
                        Background = Brushes.Cyan;
                    }
                    txtCurrent.Text = ConvertToTime(time);
                    break;
                default: break;
            }
        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            Enter();
        }

        private void Enter()
        {
            if (state == 'P')
            {
                InvalidateVisual();
                reminderTime = 0;
                state = 'B';
                time = breakTime;
                txtCurrent.Text = ConvertToTime(time);
                txtExercises.Text = selecter.GetExercises();
                btnContinue.Content = "Pause";
                btnContinue.Background = Brushes.DarkOrange;
                timer.Start();
            }
            else
            {
                if (timer.IsEnabled)
                {
                    btnContinue.Content = "Resume";
                    btnContinue.Background = Brushes.LawnGreen;
                    timer.Stop();
                }
                else
                {
                    btnContinue.Content = "Pause";
                    btnContinue.Background = Brushes.DarkOrange;
                    timer.Start();
                }
            }
        }

        private void ManageWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Enter();
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

        void App_Activated(object sender, EventArgs e)
        {
            InvalidateVisual();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            timer.Stop();
            totaltimer.Stop();
            //MessageBox.Show("Total time = " + ConvertToTime(totalTime) + "!");
        }
    }
}
