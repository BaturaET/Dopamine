using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace Dopamine.Views.NowPlaying
{
    public partial class NowPlaying : UserControl
    {
        private Timer hideControlsTimer = new Timer();

        public bool CanShowControls
        {
            get { return Convert.ToBoolean(GetValue(CanShowControlsProperty)); }
            set { SetValue(CanShowControlsProperty, value); }
        }

        public static readonly DependencyProperty CanShowControlsProperty =
            DependencyProperty.Register(nameof(CanShowControls), typeof(bool), typeof(NowPlaying), new PropertyMetadata(null));

        public NowPlaying()
        {
            InitializeComponent();

            this.hideControlsTimer.Interval = 2000;
            this.hideControlsTimer.Elapsed += new ElapsedEventHandler(this.CleanupNowPlayingHandler);
            this.ShowControls();
        }

        private void ShowControls()
        {
            this.hideControlsTimer.Stop();
            this.CanShowControls = true;
            this.hideControlsTimer.Start();
        }

        public void CleanupNowPlayingHandler(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!this.BackButton.IsMouseOver)
                {
                    this.CanShowControls = false;
                }
            }));
        }

        private void NowPlaying_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.ShowControls();
        }

        private void NowPlaying_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.AlignBackgroundCoverArt();
        }

        private void AlignBackgroundCoverArt()
        {
            try
            {
                this.BackgroundCoverArtControl.Margin = new Thickness(0, -Convert.ToInt32(this.ActualHeight / 2), 0, 0);
            }
            catch (Exception)
            {
                // Swallow this exception
            }
        }
    }
}
