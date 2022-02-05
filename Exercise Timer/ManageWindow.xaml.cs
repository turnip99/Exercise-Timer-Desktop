using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
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
    /// Interaction logic for ManageWindow.xaml
    /// </summary>
    public partial class ManageWindow : Window
    {
        List<string> exerciseList = new List<string>();
        DataTable ExerciseData = new DataTable();
        public ManageWindow()
        {
            InitializeComponent();
            ButtonSave.Click += ButtonSave_Click;
            KeyDown += new KeyEventHandler(ManageWindow_KeyDown);
            ExerciseData.Columns.Add("Name");
            ExerciseData.Columns.Add("Min Duration");
            ExerciseData.Columns.Add("Max Duration");
            ExerciseData.Columns.Add("Downtime");
            ExerciseData.Columns.Add("Min Reps");
            ExerciseData.Columns.Add("Max Reps");
            ExerciseData.Columns.Add("Per Side?");
            ExerciseData.Columns.Add("Resources?");
            int counter = 0;
            string line;
            StreamReader file = new StreamReader(@"./Exercises.txt");
            while ((line = file.ReadLine()) != null)
            {
                exerciseList.Add(line);
                counter++;
            }
            file.Close();
            char[] lineArray;
            for (int i = 1; i < counter; i++)
            {
                var dataRow = ExerciseData.NewRow();
                lineArray = exerciseList[i].ToCharArray();
                int column = 0;
                StringBuilder builder = new StringBuilder();
                for (int j = 0; j < lineArray.Length; j++)
                {
                    if (lineArray[j] == '/')
                    {
                        dataRow[column] = builder.ToString();
                        builder.Clear();
                        column++;
                    }
                    else
                    {
                        builder.Append(lineArray[j]);
                    }
                }
                ExerciseData.Rows.Add(dataRow);
            }
            ExerciseGrid.ItemsSource = ExerciseData.DefaultView;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void ManageWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Save();
            }
        }

        private void Save()
        {
            int temp;
            char tempChar;
            StreamReader file = new StreamReader(@"./Exercises.txt");
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(file.ReadLine());
            file.Close();
            for (int i = 0; i < ExerciseGrid.Items.Count - 1; i++)
            {
                if (ExerciseData.Rows[i][0].ToString().Length == 0)
                {
                    MessageBox.Show("The name of exercise (" + ExerciseData.Rows[i][0] + ") is non-existent.");
                    return;
                }
                if (ExerciseData.Rows[i][0].ToString().Length > 20)
                {
                    MessageBox.Show("The name of exercise (" + ExerciseData.Rows[i][0] + ") is too long.");
                    return;
                }
                if (!int.TryParse(ExerciseData.Rows[i][1].ToString(), out temp) || !int.TryParse(ExerciseData.Rows[i][2].ToString(), out temp) || !int.TryParse(ExerciseData.Rows[i][3].ToString(), out temp) || !int.TryParse(ExerciseData.Rows[i][4].ToString(), out temp) || !int.TryParse(ExerciseData.Rows[i][5].ToString(), out temp) ||
                    int.Parse(ExerciseData.Rows[i][1].ToString()) <= 0 || int.Parse(ExerciseData.Rows[i][2].ToString()) <= 0 || int.Parse(ExerciseData.Rows[i][3].ToString()) < 0 || int.Parse(ExerciseData.Rows[i][4].ToString()) <= 0 || int.Parse(ExerciseData.Rows[i][5].ToString()) <= 0)
                {
                    MessageBox.Show("One of the times for exercise (" + ExerciseData.Rows[i][0] + ") is an invalid number.");
                    return;
                }
                if (int.Parse(ExerciseData.Rows[i][1].ToString()) > int.Parse(ExerciseData.Rows[i][2].ToString()))
                {
                    MessageBox.Show("The minimum time for exercise (" + ExerciseData.Rows[i][0] + ") exceed the maximum.");
                    return;
                }
                if (int.Parse(ExerciseData.Rows[i][4].ToString()) > int.Parse(ExerciseData.Rows[i][5].ToString()))
                {
                    MessageBox.Show("The minimum repeats for exercise (" + ExerciseData.Rows[i][0] + ") exceed the maximum.");
                    return;
                }
                if (!char.TryParse(ExerciseData.Rows[i][6].ToString(), out tempChar) || (char.Parse(ExerciseData.Rows[i][6].ToString().ToUpper()) != 'N' && char.Parse(ExerciseData.Rows[i][6].ToString().ToUpper()) != 'Y'))
                {
                    MessageBox.Show("The 'Both sides' input for Exercise (" + ExerciseData.Rows[i][0] + ") is invalid. Only Y (yes) and N (no) are valid inputs.");
                    return;
                }
                if (!char.TryParse(ExerciseData.Rows[i][7].ToString(), out tempChar) || (char.Parse(ExerciseData.Rows[i][7].ToString().ToUpper()) != 'N' && char.Parse(ExerciseData.Rows[i][7].ToString().ToUpper()) != 'F' && char.Parse(ExerciseData.Rows[i][7].ToString().ToUpper()) != 'W' && char.Parse(ExerciseData.Rows[i][7].ToString().ToUpper()) != 'B'))
                {
                    MessageBox.Show("The 'Resources' input for exercise (" + ExerciseData.Rows[i][0] + ") is invalid. Only N (none), F (floor), W (wall) and B (band/ball) are valid inputs.");
                    return;
                }
                builder.AppendLine(ExerciseData.Rows[i][0].ToString() + "/" + ExerciseData.Rows[i][1].ToString() + "/" + ExerciseData.Rows[i][2].ToString() + "/" + ExerciseData.Rows[i][3].ToString() + "/" + ExerciseData.Rows[i][4].ToString() + "/" + ExerciseData.Rows[i][5].ToString() + "/" + ExerciseData.Rows[i][6].ToString().ToUpper() + "/" + ExerciseData.Rows[i][7].ToString().ToUpper() + "/");
            }
            FileStream fs = new FileStream(@"./Exercises.txt", FileMode.Open);
            fs.SetLength(0);
            byte[] info = new UTF8Encoding(true).GetBytes(builder.ToString());
            fs.Write(info, 0, info.Length);
            fs.Close();
            MessageBox.Show("Changes saved.");
            Close();
        }
    }
}
