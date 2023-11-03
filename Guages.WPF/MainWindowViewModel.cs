using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Windows.Threading;

namespace Guages.WPF
{
    internal class MainWindowViewModel : ObservableRecipient
    {
        //Private variables
        private DispatcherTimer timer;

        public MainWindowViewModel()
        {
            Score = 235;
            CompassScore = 0;

            //Start the timer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(2500);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        public int CompassScore { get; set; }

        public int Score { get; set; }

        private void timer_Tick(object? sender, EventArgs e)
        {
            var rand = new Random().Next(40, 240);
            Score = rand;

            var rand1 = new Random().Next(0, 35);
            CompassScore = rand1;
            OnPropertyChanged(nameof(Score));
            OnPropertyChanged(nameof(CompassScore));
        }
    }
}