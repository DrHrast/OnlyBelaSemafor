using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OnlyBelaSemafor.Models
{
    class TeamDisplayModel : INotifyPropertyChanged
    {
        //public int ScoreTarget
        //{
        //    get => _scoreTarget;
        //    set
        //    {
        //        _scoreTarget = value;
        //        OnPropertyChanged(nameof(ScoreTarget));
        //    }
        //}
        private string name;
        private int score = 0;
        private int placeholder = 0;
        private int call;
        private bool isBela;
        private bool isCall;
        private bool isStilja;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        public int Score
        {
            get => score;
            set
            {
                score = value;
                OnPropertyChanged();
            }
        }
        public int Placeholder
        {
            get => placeholder; 
            set
            {
                placeholder = value;
                OnPropertyChanged();
            }
        }
        public int Call 
        { 
            get => call; 
            set
            {
                call = value;
                OnPropertyChanged();
            }
        }
        public bool IsBela 
        { 
            get => isBela;
            set
            {
                isBela = value;
                OnPropertyChanged();
            }
        }
        public bool IsCall 
        { 
            get => isCall;
            set
            {
                isCall = value;
                OnPropertyChanged();
            }
        }
        public bool IsStilja 
        { 
            get => isStilja;
            set
            {
                isStilja = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Reset()
        {
            Score = 0;
            Placeholder = 0;
            Call = 0;
            IsBela = false;
            IsCall = false;
            IsStilja = false;
        }
    }
}
