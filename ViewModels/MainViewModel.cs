using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Media;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using KonataClock.Models;

namespace KonataClock.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly DispatcherTimer _clockTimer;
        private readonly DispatcherTimer _alarmTimer;
        private readonly DispatcherTimer _stopwatchTimer;
        private readonly DispatcherTimer _countdownTimer;
        private readonly Stopwatch _stopwatch = new();

        private string _currentTimeDisplay = string.Empty;
        private string _notificationMessage = string.Empty;
        private bool _isNotificationVisible;

        private string _newAlarmTime = "07:00";
        private string _newAlarmLabel = "Wake up with Konata!";

        private string _stopwatchDisplay = "00:00:00";
        private bool _stopwatchRunning;

        private string _timerInputMinutes = "00";
        private string _timerInputSeconds = "30";
        private string _timerDisplay = "00:00";
        private bool _timerRunning;
        private TimeSpan _timerRemaining = TimeSpan.Zero;

        public MainViewModel()
        {
            Alarms = new ObservableCollection<Alarm>();

            _clockTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _clockTimer.Tick += (_, _) => UpdateCurrentTime();
            _clockTimer.Start();

            _alarmTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _alarmTimer.Tick += (_, _) => CheckAlarms();
            _alarmTimer.Start();

            _stopwatchTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            _stopwatchTimer.Tick += (_, _) => UpdateStopwatchDisplay();

            _countdownTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _countdownTimer.Tick += (_, _) => TickCountdown();

            UpdateCurrentTime();
        }

        public ObservableCollection<Alarm> Alarms { get; }

        public string CurrentTimeDisplay
        {
            get => _currentTimeDisplay;
            set { _currentTimeDisplay = value; OnPropertyChanged(); }
        }

        public string NotificationMessage
        {
            get => _notificationMessage;
            set { _notificationMessage = value; OnPropertyChanged(); }
        }

        public bool IsNotificationVisible
        {
            get => _isNotificationVisible;
            set { _isNotificationVisible = value; OnPropertyChanged(); }
        }

        public string NewAlarmTime
        {
            get => _newAlarmTime;
            set { _newAlarmTime = value; OnPropertyChanged(); }
        }

        public string NewAlarmLabel
        {
            get => _newAlarmLabel;
            set { _newAlarmLabel = value; OnPropertyChanged(); }
        }

        public string StopwatchDisplay
        {
            get => _stopwatchDisplay;
            set { _stopwatchDisplay = value; OnPropertyChanged(); }
        }

        public bool StopwatchRunning
        {
            get => _stopwatchRunning;
            set { _stopwatchRunning = value; OnPropertyChanged(); }
        }

        public string TimerInputMinutes
        {
            get => _timerInputMinutes;
            set { _timerInputMinutes = value; OnPropertyChanged(); }
        }

        public string TimerInputSeconds
        {
            get => _timerInputSeconds;
            set { _timerInputSeconds = value; OnPropertyChanged(); }
        }

        public string TimerDisplay
        {
            get => _timerDisplay;
            set { _timerDisplay = value; OnPropertyChanged(); }
        }

        public bool TimerRunning
        {
            get => _timerRunning;
            set { _timerRunning = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void UpdateCurrentTime()
        {
            CurrentTimeDisplay = DateTime.Now.ToString("hh:mm:ss tt");
        }

        public void AddAlarm()
        {
            if (TimeSpan.TryParse(NewAlarmTime, out var time))
            {
                Alarms.Add(new Alarm
                {
                    Time = time,
                    Label = string.IsNullOrWhiteSpace(NewAlarmLabel) ? "Konata alarm" : NewAlarmLabel,
                    IsEnabled = true
                });
                NewAlarmLabel = "";
            }
            else
            {
                ShowNotification("Please enter a valid time in HH:MM format.");
            }
        }

        public void DeleteAlarm(Guid id)
        {
            var alarm = FindAlarm(id);
            if (alarm != null)
            {
                Alarms.Remove(alarm);
            }
        }

        public void UpdateAlarmTime(Guid id, string newTime)
        {
            if (!TimeSpan.TryParse(newTime, out var parsed))
            {
                ShowNotification("Could not parse the new time. Use HH:MM format.");
                return;
            }

            var alarm = FindAlarm(id);
            if (alarm != null)
            {
                alarm.Time = parsed;
            }
        }

        public void UpdateAlarmLabel(Guid id, string label)
        {
            var alarm = FindAlarm(id);
            if (alarm != null)
            {
                alarm.Label = label;
            }
        }

        private Alarm? FindAlarm(Guid id)
        {
            foreach (var alarm in Alarms)
            {
                if (alarm.Id == id)
                {
                    return alarm;
                }
            }
            return null;
        }

        private void CheckAlarms()
        {
            var now = DateTime.Now;
            foreach (var alarm in Alarms)
            {
                if (!alarm.IsEnabled)
                {
                    continue;
                }

                if (alarm.LastTriggeredDate.Date != now.Date &&
                    alarm.Time.Hours == now.Hour &&
                    alarm.Time.Minutes == now.Minute &&
                    Math.Abs(alarm.Time.Seconds - now.Second) < 1)
                {
                    alarm.LastTriggeredDate = now;
                    TriggerKonataAlert($"Alarm: {alarm.Label}");
                }
            }
        }

        public void StartStopwatch()
        {
            if (StopwatchRunning)
            {
                PauseStopwatch();
                return;
            }

            _stopwatch.Start();
            StopwatchRunning = true;
            _stopwatchTimer.Start();
        }

        public void PauseStopwatch()
        {
            _stopwatch.Stop();
            StopwatchRunning = false;
            _stopwatchTimer.Stop();
        }

        public void ResetStopwatch()
        {
            _stopwatch.Reset();
            StopwatchDisplay = "00:00:00";
            StopwatchRunning = false;
            _stopwatchTimer.Stop();
        }

        private void UpdateStopwatchDisplay()
        {
            StopwatchDisplay = _stopwatch.Elapsed.ToString("hh\:mm\:ss");
        }

        public void StartTimer()
        {
            if (TimerRunning)
            {
                PauseTimer();
                return;
            }

            if (_timerRemaining == TimeSpan.Zero)
            {
                if (!int.TryParse(TimerInputMinutes, out var minutes)) minutes = 0;
                if (!int.TryParse(TimerInputSeconds, out var seconds)) seconds = 0;
                _timerRemaining = new TimeSpan(0, minutes, seconds);
            }

            if (_timerRemaining <= TimeSpan.Zero)
            {
                ShowNotification("Set a countdown time first!");
                return;
            }

            TimerRunning = true;
            _countdownTimer.Start();
            UpdateTimerDisplay();
        }

        public void PauseTimer()
        {
            TimerRunning = false;
            _countdownTimer.Stop();
        }

        public void ResetTimer()
        {
            PauseTimer();
            _timerRemaining = TimeSpan.Zero;
            TimerDisplay = "00:00";
        }

        private void TickCountdown()
        {
            if (_timerRemaining <= TimeSpan.Zero)
            {
                FinishTimer();
                return;
            }

            _timerRemaining = _timerRemaining.Subtract(TimeSpan.FromSeconds(1));
            UpdateTimerDisplay();

            if (_timerRemaining <= TimeSpan.Zero)
            {
                FinishTimer();
            }
        }

        private void UpdateTimerDisplay()
        {
            TimerDisplay = _timerRemaining.ToString("mm\:ss");
        }

        private void FinishTimer()
        {
            PauseTimer();
            if (_timerRemaining < TimeSpan.Zero)
            {
                _timerRemaining = TimeSpan.Zero;
            }
            UpdateTimerDisplay();
            TriggerKonataAlert("Timer finished!");
            _timerRemaining = TimeSpan.Zero;
        }

        private void TriggerKonataAlert(string message)
        {
            NotificationMessage = message;
            IsNotificationVisible = true;
            PlayKonataSound();
        }

        public void DismissNotification()
        {
            IsNotificationVisible = false;
        }

        private void PlayKonataSound()
        {
            try
            {
                var player = new SoundPlayer("Assets/Audio/konata_alarm.wav");
                player.Play();
            }
            catch
            {
                // Intentionally ignore audio issues to keep the UI responsive.
            }
        }

        private void ShowNotification(string message)
        {
            NotificationMessage = message;
            IsNotificationVisible = true;
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
