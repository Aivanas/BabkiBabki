using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using BabkiBabki.Properties;
using Newtonsoft.Json;

namespace BabkiBabki.JSON
{
    public class JSONEditor
    {
        /*public static List<Note> GetNotesList(string text)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<Note>>(text);
            }
            catch 
            {
                MessageBox.Show("Джисончик чёт не читается");
            }

            return new List<Note>();
        }

        public static string SerializeNotesList(List<Note> list)
        {
            
            try
            {
                return JsonConvert.SerializeObject(list);

            }
            catch
            {
                MessageBox.Show("JSON ОБКАКАЛСЯ!");
            }
            return String.Empty;

        }

        public static List<Category> GetCategoriesList(string text)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<Category>>(text);
            }
            catch 
            {
                MessageBox.Show("Джисончик чёт не читается");
            }

            return new List<Category>();
        }

        public static string SerializeCatsList(List<Category> list)
        {
            try
            {
                return JsonConvert.SerializeObject(list);
            }
            catch
            {
                MessageBox.Show("И снова жисон насрал, и только какиш я вижу здесь");
            }
            return String.Empty;
        }
        */
        
        public static void Serialize<T>(List<T> dataList)
        {
            try
            {
                if (dataList is List<Note>)
                {
                    File.WriteAllText("../../Notes.json", JsonConvert.SerializeObject(dataList));
                }

                else if (dataList is List<Category>)
                {
                    File.WriteAllText("../../Cats.json", JsonConvert.SerializeObject(dataList));
                }

                else
                {
                    MessageBox.Show("Таким как " + dataList.GetType() + " здесь не рады. Ошибка сериализации");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Андрюха, у нас \n" + exception + "\n возможно криминал, по коням");
            }
           
  
        }

        public static T Deserialize<T>(string path)
        {
            T dataList =  JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return dataList;
        }
        
        
    }
}