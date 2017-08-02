using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Json;

using SQLite;
using Xamarin.Forms;
using PCLStorage;

namespace PortableTesting
{
    public class PCLMechanics
    {
        Data rawData = Data.Instance();

        #region StaticStringsToReadForSQLite

        public enum SQLNames
        {
            TherapistEmail = 1,
            Notifications = 2,
            SleepHours = 3,
            Mania = 4,
            Depression = 5,
            Medication = 6,
            Eating = 7,
            Sleeping = 8,
            Journal = 9,
            WakeTime = 10,
            EatTime = 11,
            EmailReminder = 12,
            Contact1 = 13,
            Contact2 = 14,
            Contact3 = 15,
            Contact4 = 16,
            Contact5 = 17,
        }

        #endregion StaticStringsToReadForSQLite

        public void PCLCreateFile(string fileName, string content)
        {
            IFolder localStorage = FileSystem.Current.LocalStorage;
            IFolder folder = localStorage.CreateFolderAsync("DailyLog", CreationCollisionOption.OpenIfExists).Result;
            IFile file = folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting).Result;
            file.WriteAllTextAsync(content);
        }

        //Create a file in the local storage system for each platform
        public IFile PCLCreateorOpenFile(string fileName)
        {
            IFolder localStorage = FileSystem.Current.LocalStorage;
            IFolder folder = localStorage.CreateFolderAsync("DailyLog", CreationCollisionOption.OpenIfExists).Result;
            IFile file = folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists).Result;
            return file;
        }

        public List<IFile> PCLOpenFolder()
        {
            IFolder localStorage = FileSystem.Current.LocalStorage;
            IFolder folder = localStorage.GetFolderAsync("DailyLog").Result;
            return folder.GetFilesAsync().Result as List<IFile>;
        }


        public string PCLReadFile(string fileName)
        {
            IFolder localStorage = FileSystem.Current.LocalStorage;
            IFolder folder = localStorage.CreateFolderAsync("DailyLog", CreationCollisionOption.OpenIfExists).Result;

            try
            {
                IFile file = folder.GetFileAsync(fileName).Result;
                return ReadFile(file, fileName).Result;
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                return string.Empty;
            }
        }

        public async Task<string> ReadFile(IFile f, string fileName)
        {
            return await Task.Run(() => f.ReadAllTextAsync()).ConfigureAwait(false);
        }
    }

    //New save routines
    public class emoteDatabase
    {
        PCLMechanics pclM = new PCLMechanics();

        readonly SQLiteAsyncConnection database;

        //open or create database
        public emoteDatabase()
        {
            database = new SQLiteAsyncConnection(pclM.PCLCreateorOpenFile("EmoteDB.DB3").Path);
            database.CreateTableAsync<emoteItem>().Wait();
        }


        public Task<List<emoteItem>> GetEmoteItems()
        {
            return database.Table<emoteItem>().ToListAsync();
        }


        public Task<emoteItem> GetEmoteItem(int id)
        {
            return database.Table<emoteItem>().Where(t => t.ID == id).FirstOrDefaultAsync();
        }


        public Task<emoteItem> GetEmoteItem(DateTime date)
        {
            return database.Table<emoteItem>().Where(t => t.Date == date).FirstOrDefaultAsync();
        }


        public void AddEmotion(SavableEmotion savEmote)
        {
            var emotionalItem = new emoteItem
            {
                EmotionTiedTo = savEmote.EmotionTiedTo,
                Name = savEmote.Name,
                Notes = savEmote.Notes,
                SliderValue = savEmote.SliderValue,
                Date = DateTime.Now
            };

            if (emotionalItem.ID != 0)
            {
                database.UpdateAsync(emotionalItem);
            }
            database.InsertAsync(emotionalItem);
        }
    }

    public class emoteItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public long SliderValue { get; set; }
        public string Notes { get; set; }
        public int EmotionTiedTo { get; set; }
        public DateTime Date { get; set; }
    }

    //Save and Load methods for Settings
    public class settingsDataBase
    {
        PCLMechanics pclM = new PCLMechanics();
        readonly SQLiteConnection database;

        //TODO: Fix for windows phone
        public settingsDataBase()
        {
            database = new SQLiteConnection(pclM.PCLCreateorOpenFile("SettingsDB.DB3").Path);

            database.CreateTable<settingsItem>();
        }

        public List<settingsItem> GetAllSettings()
        {
            return database.Table<settingsItem>().ToList();
        }

        public List<settingsItem> GetSettings(int id)
        {
            return database.Table<settingsItem>().Where(t => t.ID == id).ToList();
        }

        public List<settingsItem> GetSettings(string name)
        {
            return database.Table<settingsItem>().Where(t => t.Name == name).ToList();
        }

        public settingsItem GetSetting(int id)
        {
            return database.Table<settingsItem>().Where(t => t.ID == id).FirstOrDefault();
        } 

        public settingsItem GetSetting(string name)
        {
            return database.Table<settingsItem>().Where(t => t.Name == name).FirstOrDefault();
        }

        public void DeleteDB()
        {
            database.DeleteAll<settingsDataBase>();
        }

        public void DeleteSetting(settingsItem setting)
        {
            database.Table<settingsItem>().Delete(t => t == setting);
        }

        public void DeleteSetting(int id)
        {
            database.Table<settingsItem>().Delete(t => id == t.ID);
        }

        public void DeleteSetting(string name)
        {
            database.Table<settingsItem>().Delete(t => name == t.Name);
        }

        public void AddSetting(settingsItem savEmote)
        {
            var settingsItem = new settingsItem
            {
                ID = savEmote.ID,
                Name = savEmote.Name,
                Value = savEmote.Value,
            };

            if (settingsItem.ID != 0)
            {
                database.Update(settingsItem);
            }
            database.Insert(settingsItem);
        }
    }

    public class settingsItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}
