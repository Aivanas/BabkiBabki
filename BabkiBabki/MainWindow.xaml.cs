using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BabkiBabki.JSON;
using BabkiBabki.Properties;
using Newtonsoft.Json;
using DataGridTextColumn = MaterialDesignThemes.Wpf.DataGridTextColumn;

namespace BabkiBabki
{
    

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        partial void UpdateNotesList();

        public int allNotesSum;
        private List<Category> catsList;
        private List<Note> notesList;
        private List<String> stringCatsList = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            NotesDataGrid.AutoGeneratingColumn += NotesDataGrid_AutoGeneratingColumn;
            NotesDataGrid.IsReadOnly = true;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DatePickerName.SelectedDate = DateTime.Today;
            notesList = new List<Note>();
            
            UpdateNotesList();
            
            UpdateCatsList();

        }

        public void UpdateCatsList()
        {
            catsList = JSONEditor.Deserialize < List<Category>>("../../Cats.json");
            stringCatsList.Clear();
            foreach (Category cat in catsList)
            {
                stringCatsList.Add(cat.categoryName);
            }

            OutlinedComboBox.ItemsSource = null; //Так надо иначе кебаб
            OutlinedComboBox.ItemsSource = stringCatsList;
        }
        partial void UpdateNotesList()
        {
            allNotesSum = 0;
            notesList = JSONEditor.Deserialize<List<Note>>("../../Notes.json");
            List<Note> todayNotes = new List<Note>();

            if (notesList != null)
            {
                foreach (var note in notesList)
                {
                    allNotesSum += note.noteSum;
                    AllMoney.Text = allNotesSum.ToString();
                    if (note.noteDate == DatePickerName.SelectedDate)
                    {
                        todayNotes.Add(note);
                    }
                }
            }
            
            NotesDataGrid.ItemsSource = null;
            NotesDataGrid.ItemsSource = todayNotes;
        }

        private void NotesDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "ID")
            {
                e.Column.Visibility = Visibility.Hidden;
            }
            if (e.PropertyName == "noteDate")
            {
                e.Column.Visibility = Visibility.Hidden;
            }
            if (e.PropertyName == "noteName") 
            {
                e.Column.Header = "Запись";
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
            if (e.PropertyName == "noteCategory") 
            {
                e.Column.Header = "Категория";
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
            if (e.PropertyName == "noteSum") 
            {
                e.Column.Header = "Сумма";
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
            if (e.PropertyName == "noteIsDone") 
            {
                e.Column.Header = "В процессе (чего?)";
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                e.Column.IsReadOnly = false;
            }
            
        }
        
        private void NoteNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(NoteNameTextBox.Text);
            if (NoteNameTextBox.Text == "Введите сюда название")
            {
                NoteNameTextBox.Text = "";
            }
        }

        private void NumTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NumTextBox.Text == "Абаба (!!!)")
            {
                NumTextBox.Text = "";
            }
        }

        private void AddNoteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (NoteNameTextBox.Text != ""  && OutlinedComboBox.Text != "" && int.TryParse(NumTextBox.Text, out int num)
                && DatePickerName.SelectedDate != null)
            {
                
                Note note = new Note();

                if (notesList != null)
                {
                    note.ID = notesList[notesList.Count - 1].ID + 1;
                }
                else
                {
                    note.ID = 0;
                }
                note.noteName = NoteNameTextBox.Text;
                note.noteCategory = OutlinedComboBox.Text;
                note.noteSum = int.Parse(NumTextBox.Text);
                note.noteIsDone = false;
                note.noteDate = (DateTime)DatePickerName.SelectedDate;
                List<Note> newNotesList;
                if (notesList!=null)
                {
                   newNotesList = new List<Note>(notesList);
                }
                else
                {
                    newNotesList = new List<Note>();
                }
                
                newNotesList.Add(note);
                JSONEditor.Serialize(newNotesList); 
                notesList = newNotesList; 
                UpdateNotesList();
            }
            else
            {
                MessageBox.Show("Ты кого надурить пытаешься? Думаешь я тебя не переиграю, не уничтожу?" +
                                " Я тебя унитожу!");
            }
            
        }

        private void NewCatButton_OnClick(object sender, RoutedEventArgs e)
        {
            NewCatWindow newCatWindow = new NewCatWindow(this, catsList);
            newCatWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            newCatWindow.Show();
        }

        private void NotesDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaveButton.Content = "Не забудь сохранить!";
            Note note = NotesDataGrid.SelectedItem as Note;
            if (note != null)
            {
                NoteNameTextBox.Text = note.noteName;
                OutlinedComboBox.SelectedItem = note.noteCategory; 
                NumTextBox.Text = note.noteSum.ToString();
            }
            
        }

       

        private void DatePickerName_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateNotesList();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            Note note = NotesDataGrid.SelectedItem as Note;
            note.noteName = NoteNameTextBox.Text;
            note.noteCategory = OutlinedComboBox.Text;
            note.noteSum = int.Parse(NumTextBox.Text);

            if (catsList != null)
            {
                foreach (Note lnote in notesList)
                {
                    if (lnote.ID == note.ID)
                    {
                        lnote.noteName = note.noteName;
                        lnote.noteCategory = note.noteCategory;
                        lnote.noteSum = note.noteSum;
                        lnote.noteIsDone = note.noteIsDone ;
                    }
                }
            }    
            JSONEditor.Serialize(notesList); 
            UpdateNotesList();
            
            
        }
    }
}
