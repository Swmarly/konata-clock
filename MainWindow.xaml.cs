using System;
using System.Windows;
using System.Windows.Controls;
using KonataClock.ViewModels;

namespace KonataClock
{
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel => (MainViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddAlarm_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddAlarm();
        }

        private void DeleteAlarm_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Guid id)
            {
                ViewModel.DeleteAlarm(id);
            }
        }

        private void AlarmTime_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Tag is Guid id)
            {
                ViewModel.UpdateAlarmTime(id, textBox.Text);
            }
        }

        private void AlarmLabel_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Tag is Guid id)
            {
                ViewModel.UpdateAlarmLabel(id, textBox.Text);
            }
        }

        private void StopwatchStartPause_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.StartStopwatch();
        }

        private void StopwatchReset_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ResetStopwatch();
        }

        private void TimerStartPause_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.StartTimer();
        }

        private void TimerReset_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ResetTimer();
        }

        private void DismissNotification_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DismissNotification();
        }
    }
}
