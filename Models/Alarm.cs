using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KonataClock.Models
{
    public class Alarm : INotifyPropertyChanged
    {
        private TimeSpan _time;
        private string _label = string.Empty;
        private bool _isEnabled = true;
        private DateTime _lastTriggeredDate = DateTime.MinValue;

        public Guid Id { get; } = Guid.NewGuid();

        public TimeSpan Time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        public string Label
        {
            get => _label;
            set
            {
                _label = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public DateTime LastTriggeredDate
        {
            get => _lastTriggeredDate;
            set
            {
                _lastTriggeredDate = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string TimeDisplay => DateTime.Today.Add(Time).ToString("hh:mm tt");
    }
}
