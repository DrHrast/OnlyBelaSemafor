using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace OnlyBelaSemafor.Models
{
    public class TeamDisplayModel : INotifyPropertyChanged
    {
        private string _name = "";
        private int _score = 0;
        private int _call = 0;
        private bool _isBela;
        private bool _isCalling;
        private bool _isStilja;
        private bool _isStiljaVisible;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        
        [Range(0, 162, ErrorMessage = "Unos mora biti između 0 i 162.")]
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                OnPropertyChanged();
            }
        }
        public int Call 
        { 
            get => _call; 
            set
            {
                _call = value;
                OnPropertyChanged();
            }
        }
        public bool IsBela 
        { 
            get => _isBela;
            set
            {
                _isBela = value;
                OnPropertyChanged();
            }
        }
        public bool IsCalling 
        { 
            get => _isCalling;
            set
            {
                _isCalling = value;
                OnPropertyChanged();
            }
        }
        public bool IsStilja 
        { 
            get => _isStilja;
            set
            {
                _isStilja = value;
                OnPropertyChanged();
            }
        }
        public bool IsStiljaVisible
        {
            get => _isStiljaVisible;
            set
            {
                _isStiljaVisible = value;
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
            Call = 0;
            IsBela = false;
            IsCalling = false;
            IsStilja = false;
        }
    }
}
