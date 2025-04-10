using System;
using System.Collections.ObjectModel;


namespace VictimRoll
{
    public partial class MainPage : ContentPage
    {


        private ObservableCollection<string> victims = new();
        private string selectedClass = string.Empty;
        private readonly string dataFolder = Path.Combine(AppContext.BaseDirectory, "Data");

        public MainPage()
        {
            InitializeComponent();
            victimList.ItemsSource = victims;
            LoadClassList();
        }

        private void LoadClassList()
        {
            Directory.CreateDirectory(dataFolder);
            var files = Directory.GetFiles(dataFolder, "*.txt").Select(Path.GetFileNameWithoutExtension).ToList();
            classPicker.ItemsSource = files;
        }

        private void OnClassSelected(object sender, EventArgs e)
        {
            if (classPicker.SelectedItem != null)
            {
                selectedClass = classPicker.SelectedItem.ToString();
                LoadStudentList();
            }
        }


        private void SaveClass()
        {
            if (!string.IsNullOrEmpty(selectedClass))
            {
                File.WriteAllLines(Path.Combine(dataFolder, $"{selectedClass}.txt"), victims);
            }
        }

        private void LoadStudentList()
        {
            victims.Clear();
            string path = Path.Combine(dataFolder, $"{selectedClass}.txt");
            if (File.Exists(path))
            {
                foreach (var line in File.ReadAllLines(path))
                {
                    victims.Add(line);

                }
            }
        }

        private void OnAddStudentClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(victimEntry.Text))
            {
                victims.Add(victimEntry.Text.Trim());
                victimEntry.Text = string.Empty;
            }
            SaveClass();
            LoadStudentList();
        }

        private void OnRemoveStudentClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(victimEntry.Text))
            {
                victims.Remove(victimEntry.Text.Trim());
                victimEntry.Text = string.Empty;
            }
            SaveClass();
            LoadStudentList();
        }

        private async void OnDrawStudentClicked(object sender, EventArgs e)
        {
            string classVar = selectedClass;
            if (victims.Count > 1)
            {
                var random = new Random();
                int index;
                for (int i = 0; i < 250; i++)
                {
                    if (selectedClass != classVar)
                    {
                        drawResult.Text = "Nie wybrano listy lub wybrana lista jest pusta";
                        break;            
                    }
                    index = random.Next(victims.Count);
                    if (i == 249)
                    {
                        drawResult.Text = victims[index];
                        await Task.Delay(1000);
                        drawResult.Text = $"Ofiara: {victims[index]}";
                    }
                    else if (i < 249 && i > 220)
                    {
                        drawResult.Text = victims[index];
                        await Task.Delay(200);
                    }
                    else if (i < 221 && i > 190)
                    {
                        drawResult.Text = victims[index];
                        await Task.Delay(125);
                    }
                    else if (i < 191 && i > 150)
                    {
                        drawResult.Text = victims[index];
                        await Task.Delay(50);
                    }
                    else 
                    {
                        drawResult.Text = victims[index];
                        await Task.Delay(20);
                    }
                }
                
            }
            else if (victims.Count == 1)
            {
                drawResult.Text = $"Ofiara: {victims[0]}";
            }
            else 
            {
                drawResult.Text = "Nie wybrano listy lub wybrana lista jest pusta";
            }
        }

        private void OnLoadClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedClass))
            {
                LoadStudentList();
            }
        }


        private void OnCreateClassClicked(object sender, EventArgs e)
        {
            string newClassName = newClassEntry.Text?.Trim();

            if (!string.IsNullOrEmpty(newClassName))
            {
                string filePath = Path.Combine(dataFolder, $"{newClassName}.txt");

                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath, "");
                    newClassEntry.Text = string.Empty;
                    LoadClassList();
                    classPicker.SelectedItem = newClassName;
                    selectedClass = newClassName;
                    victims.Clear();
                    DisplayAlert("Uwaga", "Utworzono klasę!!", "OK");
                }
                else
                {
                    DisplayAlert("Uwaga", "Klasa już istnieje!", "OK");
                }
            }
        }

        private void OnDeleteClassClicked(object sender, EventArgs e)
        {
            string newClassName = newClassEntry.Text?.Trim();
            if (!string.IsNullOrEmpty(newClassName))
            {
                if (File.Exists(Path.Combine(dataFolder, $"{newClassName}.txt")))
                {
                    File.Delete(Path.Combine(dataFolder, $"{newClassName}.txt"));
                    DisplayAlert("Uwaga", "Usunięto klasę!!", "OK");
                    classPicker.SelectedItem = null;
                    selectedClass = null;
                    LoadClassList();
                    victims.Clear();

                }
                    
                else
                {
                    DisplayAlert("Uwaga", "Nie ma takiej klasy!!", "OK");
                }
            }
        }

    }

}
