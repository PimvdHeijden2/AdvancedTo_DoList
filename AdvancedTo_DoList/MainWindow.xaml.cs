using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;


namespace AdvancedTo_DoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<int> chosenIndexes = new List<int>();
        List<string> ChosenLines = new List<string>();

        public BrushConverter bc = new BrushConverter();
        string year = "";
        string month = "";
        string day = "";

        string id = "";
        string title = "";
        string date = "";
        string prio = "";

        public MainWindow()
        {
            InitializeComponent();
            CheckOutdatedProjects();
            GenerateTasks(false);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();

        }
        private void CheckOutdatedProjects()
        {
            List<string> lines = new List<string>();

            List<string> ToBeDeleted = new List<string>();
            
            using (var sr = new StreamReader("..\\..\\DataBase\\DataBase.csv"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                    string id = "";
                    string day = "";
                    string month = "";
                    string year = "";

                    string[] values;

                    values = line.Split('_');

                    id = values[0].Replace("_", string.Empty);
                    day = values[3].Replace("_", string.Empty);
                    month = values[2].Replace("_", string.Empty);
                    year = values[4].Replace("_", string.Empty);


                    string today = DateTime.Now.ToString("MM dd yyyy");
                    DateTime Today = DateTime.ParseExact(today, "MM dd yyyy", CultureInfo.InvariantCulture);


                    DateTime dueDate = DateTime.ParseExact($"{month} {day} {year}", "MM dd yyyy", CultureInfo.InvariantCulture);
                    if (dueDate < Today.Date)
                    {
                        Console.WriteLine($"{dueDate}   {today}");
                        ToBeDeleted.Add(line);
                    }

                }
            }
           
            List<string> remainingLines = new List<string>();
            foreach(var line in lines)
            {
                if (!ToBeDeleted.Contains(line))
                {
                    remainingLines.Add(line);
                }
            }

            File.WriteAllLines("..\\..\\DataBase\\DataBase.csv", remainingLines);


        }


        private void GenerateTasks(bool newItemAdded) // generates possible tasks to be done for that day
        {
            CanvasItems.Children.Clear();
            var today = DateTime.Now.ToString("dd MMM yyyy");
            var lastRecordedDate = File.ReadLines("..\\..\\DataBase\\Todays_Date.csv").Last();

            chosenIndexes.Clear();
            ChosenLines.Clear();

            // Reading today's tasks indexes
            var todaysTaskLines = File.ReadLines("..\\..\\DataBase\\Todays_Tasks.csv").ToList();

            // Handle newly added item if applicable
            if (newItemAdded == true)
            {
                List<string> lines = new List<string>();
                using (var sr3 = new StreamReader("..\\..\\DataBase\\DataBase.csv"))
                {
                    string line;
                    while ((line = sr3.ReadLine()) != null)
                    {
                        lines.Add(line);

                    }
                }
                if (chosenIndexes.Count <= 5)
                {
                    if (lines.Count >= 2)
                    {
                        // Get the second last line (index: lines.Count - 2)
                        string secondLastLine = lines[lines.Count - 2];
                        Console.WriteLine(secondLastLine);
                        ChosenLines.Add(secondLastLine);

                    }
                    else
                    {
                        string secondLastLine = lines[lines.Count - 1];
                        Console.WriteLine(secondLastLine);

                        ChosenLines.Add(secondLastLine);

                    }
                    Console.WriteLine(lines.Count);
                }
                using (var sw = new StreamWriter("..\\..\\DataBase\\Todays_Date.csv"))
                {
                    sw.Write(".");
                }
                var process = Process.GetCurrentProcess();
                Process.Start(process.MainModule.FileName);
                Application.Current.Shutdown();
            }
            // If last recorded date is today and there are tasks for today
            if (lastRecordedDate == today && todaysTaskLines.Count > 0)
            {
                Console.WriteLine("test1");
                string line;
                int i = 0;

                using (var sr = new StreamReader("..\\..\\DataBase\\Todays_Tasks.csv"))
                {
                    while((line = sr.ReadLine()) != null)
                    {
                        ChosenLines.Add(line);
                    }
                    
                    foreach(string LineDB in ChosenLines)
                    {

                        CreateTaskItem(LineDB);


                    }
                    
                }
            }
            else if (lastRecordedDate != today || todaysTaskLines.Count == 0)
            {
                Console.WriteLine("test2");

                // Update today's date
                using (var sw = new StreamWriter("..\\..\\DataBase\\Todays_Date.csv"))
                {
                    sw.WriteLine(today);
                }

                CanvasItems.Children.Clear();
                List<string> lines = new List<string>();
                using (var sr3 = new StreamReader("..\\..\\DataBase\\DataBase.csv"))
                {
                    string line;
                    while ((line = sr3.ReadLine()) != null)
                    {
                        lines.Add(line);
                        
                    }
                }
                using (var sr3 = new StreamReader("..\\..\\DataBase\\DataBase.csv"))
                {
                    

                    // Combine factors to determine final probability
                    string line;
                    while ((line = sr3.ReadLine()) != null)
                    {
                        var values = line.Split('_');
                        int prio = Convert.ToInt32(values[1]);
                        Random rnd = new Random();
                        double baseProbability = 0.4; // Base probability for any task
                        double priorityFactor = (10 - prio) / 10.0; // Higher priority tasks have a higher factor
                        double taskFactor = 1.0 / lines.Count; // More tasks reduce individual task probability

                        double probability = baseProbability + (priorityFactor * taskFactor);
                        if (prio == 1 || rnd.NextDouble() <= probability)
                        {
                            ChosenLines.Add(line);
                        }
                    }
                }

                

                Random rnd2 = new Random();
                while (chosenIndexes.Count < 5 && chosenIndexes.Count < ChosenLines.Count -1)
                {
                    Console.WriteLine(ChosenLines.Count);
                    int randomIndex = rnd2.Next(0, ChosenLines.Count - 1);
                    if (!chosenIndexes.Contains(randomIndex))
                    {
                        Console.WriteLine(randomIndex);

                        chosenIndexes.Add(randomIndex);
                    }
                }
            }
            if(lastRecordedDate != today || todaysTaskLines.Count == 0)
            {
                // Write today's tasks indexes to file
                using (var sw = new StreamWriter("..\\..\\DataBase\\Todays_Tasks.csv"))
                {
                    foreach (var index in chosenIndexes)
                    {
                        sw.WriteLine($"{ChosenLines[index]}");
                        CreateTaskItem(ChosenLines[index]);
                    }
                }
            }
            
        }

        private void CreateTaskItem(string line)
        {
            string id = "";
            string title = "";
            string day = "";
            string month = "";
            string year = "";
            string prio = "";

            string[] values;
            values = line.Split('_');
            

            id = values[0].Replace("_", string.Empty);
            prio = values[1].Replace("_", string.Empty);
            month = values[2].Replace("_", string.Empty);
            day = values[3].Replace("_", string.Empty);
            year = values[4].Replace("_", string.Empty);
            title = values[5].Replace("_", string.Empty);

            






            // Create the outer border
            Border outerBorder = new Border
            {
                CornerRadius = new CornerRadius(12),
                Margin = new Thickness(0,8,0,0),
                Height = 60,
                Width = 370,
                Uid = id,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f8f8f8")),
                Effect = new DropShadowEffect
                {
                    BlurRadius = 10,
                    ShadowDepth = 2.5,
                    Opacity = 0.5,
                    Color = (Color)ColorConverter.ConvertFromString("#636363")
                }
            };

            // Create the main stack panel
            StackPanel mainStackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical
            };

            // Create the top stack panel
            StackPanel topStackPanel = new StackPanel();

            TextBlock titleTextBlock = new TextBlock
            {
                Margin = new Thickness(15, 5, 0, 0),
                FontWeight = FontWeights.Medium,
                FontSize = 24,
                Text = title
            };

            TextBlock dueDateTextBlock = new TextBlock
            {
                Margin = new Thickness(16, 0, 0, 0),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#203a98")),
                FontWeight = FontWeights.Normal,
                FontSize = 13,
                Text = $"Due {day} {month} {year}"
            };

            topStackPanel.Children.Add(titleTextBlock);
            topStackPanel.Children.Add(dueDateTextBlock);

            // Create the bottom stack panel
            StackPanel bottomStackPanel = new StackPanel
            {
                Margin = new Thickness(0, -55, 0, 0),
                Height = 60
                
            };

            Panel.SetZIndex(bottomStackPanel, 4);


            // Create the inner border
            Border cbxItem = new Border
            {
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4d5bbf")),
                BorderThickness = new Thickness(2),
                CornerRadius = new CornerRadius(40),
                Background = Brushes.Transparent,
                Width = 35,
                Height = 35,
                Margin = new Thickness(280, 15, 0, 0),
                Effect = new DropShadowEffect
                {
                    BlurRadius = 10,
                    ShadowDepth = 2.5,
                    Opacity = 0.3,
                    Color = (Color)ColorConverter.ConvertFromString("#636363")
                }
            };

            // Add mouse event handler
            cbxItem.MouseLeftButtonDown += Border_MouseLeftButtonDown;

            bottomStackPanel.Children.Add(cbxItem);

            mainStackPanel.Children.Add(topStackPanel);
            mainStackPanel.Children.Add(bottomStackPanel);

            outerBorder.Child = mainStackPanel;

            // Add the outer border to the canvas
            CanvasItems.Children.Add(outerBorder);
            id = "";
            title = "";
            day = "";
            month = "";
            year = "";
            prio = "";
        }
        private void AddDeadline(int prio, string date, string name)
        {
            StreamReader sr = new StreamReader("..\\..\\DataBase\\DataBase.csv");
            string firstline = sr.ReadLine();
            List<string> lines = new List<string>();
            sr.Close();
            string line;
            string strID = "";
            if (firstline != null)
            {
                strID = firstline.Substring(0).Remove(3);
                int idCounter = Convert.ToInt32(strID);
                StreamReader sr2 = new StreamReader("..\\..\\DataBase\\DataBase.csv");

                while ((line = sr2.ReadLine()) != null)
                {
                    idCounter++;
                    lines.Add(line);
                }

                string id = (idCounter).ToString();
                id = "0" + id;
                if (id.Length <= 2)
                {
                    id = "0" + id;
                }

                sr2.Close(); 
                StreamWriter sw = new StreamWriter("..\\..\\DataBase\\DataBase.csv");
                
                sw.Write($"{id}_{prio}_{date}_{name}\n");

                sw.Close();
            }
            else
            {
                string id = "001";
                StreamWriter sw = new StreamWriter("..\\..\\DataBase\\DataBase.csv");
                sw.WriteLine($"{id}_{prio}_{date}_{name}");
                sw.Close();

            }
            
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            Border CbxItem = (Border)sender;
            if (CbxItem.Background != Brushes.Transparent)//IS already clicked
            {
                CbxItem.Background = Brushes.Transparent;
            }
            else //IS NOT already clicked
            {
                CbxItem.Background = (Brush)bc.ConvertFrom("#4d5bbf");
            }

        }

        private void Menu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnAddDeadLineMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Storyboard clickScaleAnimation = (Storyboard)FindResource("ClickScaleAnimationIN");

            clickScaleAnimation.Begin(TasksMenu);
            CheckClickTaskMenu.Visibility = Visibility.Visible;

        }
        
        private void CheckClickTaskMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {   
            Storyboard clickScaleAnimation = (Storyboard)FindResource("ClickScaleAnimationOUT");

            clickScaleAnimation.Begin(TasksMenu);
            CheckClickTaskMenu.Visibility = Visibility.Hidden   ;

        }

        private void btnSubmitProject_Click(object sender, RoutedEventArgs e)
        {
            if(tbxName.Text.Length != 0 && dpDueDate.Text.Length != 0 && tbxPrio.Text.Length != 0)
            {
                string name = tbxName.Text;
                string prio = tbxPrio.Text;
                string date = dpDueDate.Text;
                

                
                string[] dateParts = date.Split('/');
                month = dateParts[0].Replace("/", string.Empty);
                day = dateParts[1].Replace("/", string.Empty);
                year = dateParts[2].Replace("/",  string.Empty);

                date = $"{day}_{month}_{year}";


                AddDeadline(Convert.ToInt32(prio), date, name);
                
                month = "";
                day = "";
                tbxName.Text = "";
                dpDueDate.Text = null;
                dpDueDate.SelectedDate = null;
                tbxPrio.Text = "";



                GenerateTasks(true);
            }
        }
        
    }
}
