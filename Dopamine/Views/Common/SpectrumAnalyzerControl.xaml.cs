using CommonServiceLocator;
using Digimezzo.Foundation.Core.Settings;
using Dopamine.Core.Enums;
using Dopamine.Services.Playback;
using Dopamine.Services.Shell;
using System.Windows;
using System.Windows.Controls;

namespace Dopamine.Views.Common
{
    public partial class SpectrumAnalyzerControl : UserControl
    {
        private IPlaybackService playbackService;

        public new object DataContext
        {
            get { return base.DataContext; }
            set { base.DataContext = value; }
        }

        public SpectrumAnalyzerControl()
        {
            InitializeComponent();

            this.playbackService = ServiceLocator.Current.GetInstance<IPlaybackService>();

            this.playbackService.PlaybackSuccess += (_, __) => this.TryRegisterSpectrumPlayers();

            SettingsClient.SettingChanged += (_, e) =>
            {
                if (SettingsClient.IsSettingChanged(e, "Playback", "ShowSpectrumAnalyzer"))
                {
                    this.TryRegisterSpectrumPlayers();
                }
            };

            this.TryRegisterSpectrumPlayers();
        }

        private void TryRegisterSpectrumPlayers()
        {
            this.UnregisterSpectrumPlayers();

            if (!SettingsClient.Get<bool>("Playback", "ShowSpectrumAnalyzer"))
            {
                // The settings don't allow showing the spectrum analyzer
                return;
            }

            if (this.playbackService.Player != null)
            {
                Application.Current.Dispatcher.Invoke(() => this.LeftSpectrumAnalyzer.RegisterSoundPlayer(this.playbackService.Player.GetWrapperSpectrumPlayer(SpectrumChannel.Left)));
                Application.Current.Dispatcher.Invoke(() => this.RightSpectrumAnalyzer.RegisterSoundPlayer(this.playbackService.Player.GetWrapperSpectrumPlayer(SpectrumChannel.Right)));
            }
        }

        private void UnregisterSpectrumPlayers()
        {
            if (this.playbackService.Player != null)
            {
                Application.Current.Dispatcher.Invoke(() => this.LeftSpectrumAnalyzer.UnregisterSoundPlayer());
                Application.Current.Dispatcher.Invoke(() => this.RightSpectrumAnalyzer.UnregisterSoundPlayer());
            }
        }
    }
}
