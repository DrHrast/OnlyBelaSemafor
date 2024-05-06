using System.ComponentModel;
using SQLite;

namespace OnlyBelaSemafor.Models
{
    public class GameModel : INotifyPropertyChanged
    {
        [PrimaryKey]
        public string Id { get; init; } = Guid.NewGuid().ToString();
        
        private string _teamOneName = "Team 1";
        private string _teamTwoName = "Team 2";
        private int _scoreTarget = 501;

        public string TeamOneName
        {
            get => _teamOneName; 
            set
            { 
                _teamOneName = value;
                OnPropertyChanged(nameof(TeamOneName));
            }
        }
        public string TeamTwoName
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
