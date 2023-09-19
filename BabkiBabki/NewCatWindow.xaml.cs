using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using BabkiBabki.JSON;

namespace BabkiBabki
{
    public partial class NewCatWindow : Window
    {
        private List<Category> catsList;
        private MainWindow  window;
        public NewCatWindow( MainWindow win, List<Category> csList)
        {
            catsList = csList;
            window = win;
            InitializeComponent();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            Category newCat = new Category();
            newCat.categoryName = TextBoxBob.Text;
            catsList.Add(newCat);
            JSONEditor.Serialize(catsList);
            SaveButton.Content = "Сохранено!";
            window.UpdateCatsList();
        }
    }
}