using CommunityToolkit.Mvvm.ComponentModel;

namespace Guages.WPF
{
    internal class MainWindowViewModel : ObservableRecipient
    {
        public MainWindowViewModel()
        {
            Score = 235;
        }

        public int Score { get; set; }
    }
}