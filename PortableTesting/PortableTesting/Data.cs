using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableTesting
{
    class Data : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private static Data instance = new Data();

        static internal Data Instance()
        {
            return instance;
        }

        #region settings
        
        //Email Address

        //Make into an array of strings
        private string _therapistEmailAd;
        public string therapistEmailAd
        {
            get
            {
                return _therapistEmailAd;
            }
            set
            {
                this._therapistEmailAd = value;
                OnPropertyChanged("Email");
            }
        }


        private string _contact1 = "";
        public string contact1
        {
            get { return _contact1; }
            set
            {
                if (_contact1 != value)
                {
                    _contact1 = value;
                    OnPropertyChanged("Contact1");
                }
            }
        }

        private string _contact2 = "";
        public string contact2
        {
            get { return _contact2; }
            set
            {
                if (_contact2 != value)
                {
                    _contact2 = value;
                    OnPropertyChanged("Contact2");
                }
            }
        }

        private string _contact3 = "";
        public string contact3
        {
            get { return _contact3; }
            set
            {
                if (_contact3 != value)
                {
                    _contact3 = value;
                    OnPropertyChanged("Contact3");
                }
            }
        }

        private string _contact4 = "";
        public string contact4
        {
            get { return _contact4; }
            set
            {
                if (_contact4 != value)
                {
                    _contact4 = value;
                    OnPropertyChanged("Contact4");
                }
            }
        }

        private string _contact5 = "";
        public string contact5
        {
            get { return _contact5; }
            set
            {
                if (_contact5 != value)
                {
                    _contact5 = value;
                    OnPropertyChanged("Contact5");
                }
            }
        }

        //All Notification alerts
        private bool _notificationstate;
        public bool notificationstate
        {
            get
            {
                return _notificationstate;
            }
            set
            {
                this._notificationstate = value;
                OnPropertyChanged("NotificationState");
            }
        }

        private bool _medication;
        public bool Medication
        {
            get
            {
                return _medication;
            }
            set
            {
                this._medication = value;
                OnPropertyChanged("Medication");
            }
        }

        private bool _Eating;
        public bool Eating
        {
            get
            {
                return _Eating;
            }
            set
            {
                this._Eating = value;
                OnPropertyChanged("Eating");
            }
        }

        private bool _Sleeping;
        public bool Sleeping
        {
            get
            {
                return _Sleeping;
            }
            set
            {
                this._Sleeping = value;
                OnPropertyChanged("Sleeping");
            }
        }

        private bool _Journal;
        public bool Journal
        {
            get
            {
                return _Journal;
            }
            set
            {
                this._Journal = value;
                OnPropertyChanged("Journal");
            }
        }

        private bool[] _note;
        public bool[] Note
        {
            get
            {
                return _note;
            }
            set
            {
                this._note = value;
                OnPropertyChanged("Note");
            }
        }

        private TimeSpan _WakeTime;
        public TimeSpan WakeTimes
        {
            get { return _WakeTime; }
            set
            {
                if (value != _WakeTime)
                {
                    _WakeTime = value;
                    OnPropertyChanged("WakeTime");
                }
            }
        }

        private TimeSpan _EatTime;
        public TimeSpan EatTime
        {
            get { return _EatTime; }
            set
            {
                if (value != _EatTime)
                {
                    _EatTime = value;
                    OnPropertyChanged("EatTime");
                }
            }
        }

        private int _EmailReminder;
        public int EmailReminder
        {
            get { return _EmailReminder; }
            set
            {
                if (value != _EmailReminder)
                {
                    _EmailReminder = value;
                    OnPropertyChanged("EmailReminder");
                }
            }
        }

        #endregion settings


        #region DailyLoggings

        private string _sleepHours;
        public string sleepHours
        {
            get
            {
                return _sleepHours;
            }
            set
            {
                this._sleepHours = value;
                OnPropertyChanged("SleepHours");
            }
        }

        private int _maniaVal;
        public int maniaVal
        {
            get
            {
                return _maniaVal;
            }
            set
            {
                if (_maniaVal != value)
                {
                    _maniaVal = value;
                    OnPropertyChanged("Mania");
                }
            }
        }


        private int _depressionVal;
        public int depressionVal
        {
            get
            {
                return _depressionVal;
            }
            set
            {
                if (_depressionVal != value)
                {
                    _depressionVal = value;
                    OnPropertyChanged("Depression");
                }
            }
        }

        #endregion DailyLoggings




        //Settings region needs this to function
        //This was needed to make two-way Binding work
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }


        //Today
        public List<SavableEmotion> emotionalLog = new List<SavableEmotion>();

        //Loaded Materials
        public List<List<SavableEmotion>> loadedStringIntLists = new List<List<SavableEmotion>>();

        //Any other day
        public Dictionary<string, long> savableLoadableDicion = new Dictionary<string, long>();
        
        //List of Strings for Each Emotion
        public List<string> HappyTerms = new List<string>()
        {"Joy", "Interest", "Proud", "Acceptance", "Powerful",
            "Peaceful", "Intimate", "Optomistic",};

        public List<string> AngryTerms = new List<string>()
        { "Critical", "Distant", "Frustrated", "Aggressive", "Mad",
            "Hateful", "Threatened", "Hurt",};

        public List<string> SadTerms = new List<string>()
        { "Bored", "Lonely", "Depressed", "Despair", "Abandoned", "Guilty", };

        public List<string> FearTerms = new List<string>()
        { "Humiliated", "Rejected", "Submissive", "Insecure", "Anxious", "Scared",};

        public List<string> SurpriseTerms = new List<string>()
        { "Startled", "Confused", "Amazed", "Excited", };

        public List<string> DisgustTerms = new List<string>()
        { "Avoidance", "Awful", "Disappointed", "Disaproval", };




    }

    public struct SavableEmotion
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public long SliderValue { get; set; }
        public string Notes { get; set; }
        public int EmotionTiedTo { get; set; }
        public DateTime Date { get; set; }
    }

}
