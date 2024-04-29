using System.ComponentModel;

namespace OnlyBelaSemafor.Models
{
    public class GameModel : INotifyPropertyChanged
    {
        private string? _teamOneName = "Team 1";
        private string? _teamTwoName = "Team 2";
        private int _scoreTarget;

        public string? TeamOneName
        {
            get => _teamOneName; 
            set
            { 
                _teamOneName = value;
                OnPropertyChanged(nameof(TeamOneName));
            }
        }
        public string? TeamTwoName
        {
            get => _teamTwoName;
            set
            {
                _teamTwoName = value;
                OnPropertyChanged(nameof(TeamTwoName));
            }
        }        
        
        public int ScoreTarget
        {
            get => _scoreTarget;
            set
            {
                _scoreTarget = value;
                OnPropertyChanged(nameof(ScoreTarget));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
