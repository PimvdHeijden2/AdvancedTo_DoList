using System;
using System.Collections.Generic;
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

namespace AdvancedTo_DoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<int> chosenIndexes = new List<int>();
        
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
            GenerateTasks(false);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();

        }

        private void GenerateTasks(bool newItemAdded)//generates possible tasks to be done for that day
        {

            var today = DateTime.Now.ToString("dd MMM yyyy");
            var lastRecordedDate = File.ReadLines("..\\..\\DataBase\\Todays_Date.csv").Last();
            StreamReader sr2 = new StreamReader("..\\..\\DataBase\\Todays_Tasks.csv");
            StreamReader sr3 = new StreamReader("..\\..\\DataBase\\DataBase.csv");
            chosenIndexes.Clear();
            string lineIndexes;
            while ((lineIndexes = sr2.ReadLine()) != null)
            {
                chosenIndexes.Add(Convert.ToInt32(lineIndexes));
            }
            if (lastRecordedDate == today && sr2.ReadToEnd() != "")
            {
                string line;
                string lineDB;
                int i = 0;

                while ((line = sr2.ReadLine()) != null)
                {
                    sr3 = new StreamReader("..\\..\\DataBase\\DataBase.csv");
                    Console.WriteLine(line);
                    while ((lineDB = sr3.ReadLine()) != null)
                    {
                        Console.WriteLine(line + "  " + lineDB);

                        if (i.ToString() == line)
                        {
                            CreateTaskItem(lineDB);
                            Console.WriteLine(line + "  " + lineDB + "   winererererrtetetse");

                            i = 0;
                            sr3.DiscardBufferedData();
                        }
                        i++;

                        if ((lineDB = sr3.ReadLine()) == null)
                        {
                            i = 0;
                            sr3.DiscardBufferedData();
                        }

                    }
                }
                sr3.Close();
                sr2.Close();
            }
            else if (lastRecordedDate != today || sr2.ReadToEnd() == "")
            {
                sr2.Close();
                StreamWriter sw1 = new StreamWriter("..\\..\\DataBase\\Todays_Date.csv");
                StreamReader sr = new StreamReader("..\\..\\DataBase\\DataBase.csv");

                sw1.WriteLine(today);
                sw1.Close();


                CanvasItems.Children.Clear();
                List<string> lines = new List<string>();
                List<string> ChosenLines = new List<string>();


                string line;

                string id = "";
                string title = "";
                string day = "";
                string month = "";
                string year = "";
                string prio = "";

                string[] values;


                while ((line = sr.ReadLine()) != null)
                {

                    lines.Add(line);

                    values = line.Split('_');
                    id = values[0].Replace("_", string.Empty);
                    prio = values[1].Replace("_", string.Empty);
                    day = values[2].Replace("_", string.Empty);
                    month = values[3].Replace("_", string.Empty);
                    year = values[4].Replace("_", string.Empty);
                    title = values[5].Replace("_", string.Empty);
                    int intPrio = Convert.ToInt32(prio);
                    Random rnd = new Random();

                    switch (intPrio)
                    {
                        case 1:
                            ChosenLines.Add(line);
                            break;
                        case 2:
                            if (rnd.Next(1, intPrio) == 1)
                            {
                                ChosenLines.Add(line);
                            }
                            break;
                        case 3:
                            if (rnd.Next(1, intPrio) == 1)
                            {
                                ChosenLines.Add(line);
                            }
                            break;
                        case 4:
                            if (rnd.Next(1, intPrio) == 1)
                            {
                                ChosenLines.Add(line);
                            }
                            break;
                        case 5:
                            if (rnd.Next(1, intPrio) == 1)
                            {
                                ChosenLines.Add(line);
                            }
                            break;
                        case 6:
                            if (rnd.Next(1, intPrio) == 1)
                            {
                                ChosenLines.Add(line);
                            }
                            break;
                        case 7:
                            if (rnd.Next(1, intPrio) == 1)
                            {
                                ChosenLines.Add(line);
                            }
                            break;
                        case 8:
                            if (rnd.Next(1, intPrio) == 1)
                            {
                                ChosenLines.Add(line);
                            }
                            break;
                        case 9:
                            if (rnd.Next(1, intPrio) == 1)
                            {
                                ChosenLines.Add(line);
                            }
                            break;
                    }
                }

                Random rnd2 = new Random();

                if (newItemAdded == true)
                {
                    var NewlyAddedLine = File.ReadLines("..\\..\\DataBase\\DataBase.csv").Last();

                    if (ChosenLines.Count <= 5)
                    {
                        ChosenLines.Add(NewlyAddedLine);
                    }

                }

                while (chosenIndexes.Count < ChosenLines.Count)
                {
                    int randomIndex = rnd2.Next(0, ChosenLines.Count);
                    if(chosenIndexes.Count == 0)
                    {
                        chosenIndexes.Add(randomIndex);
                    }
                    if (!chosenIndexes.Contains(randomIndex))
                    {
                        chosenIndexes.Add(randomIndex);
                    }
                    Console.WriteLine(randomIndex + "    " + chosenIndexes.Count);
                }

                sr.Close();
                // File.WriteAllText("..\\..\\DataBase\\Todays_Tasks.csv", null);
                
                StreamWriter sw2 = new StreamWriter("..\\..\\DataBase\\Todays_Tasks.csv");
                foreach (var index in chosenIndexes)
                {
                    sw2.WriteLine($"{index}");

                    CreateTaskItem(ChosenLines[index]);

                }
                sw2.Close();
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
            day = values[2].Replace("_", string.Empty);
            month = values[3].Replace("_", string.Empty);
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

                string id = (idCounter += 1).ToString();
                id = "0" + id;
                if (id.Length <= 2)
                {
                    id = "0" + id;
                }

                sr2.Close(); 
                StreamWriter sw = new StreamWriter("..\\..\\DataBase\\DataBase.csv");
                foreach (var Line in lines)
                {
                    sw.WriteLine(Line);
                }
                sw.Write($"{id}_{prio}_{date}_{name}");

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
                
                var lastLine = File.ReadLines("..\\..\\DataBase\\DataBase.csv").Last();
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
