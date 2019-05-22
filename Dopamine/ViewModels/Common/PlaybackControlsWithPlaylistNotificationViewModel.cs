﻿using Digimezzo.Foundation.Core.Utils;
using Dopamine.Services.Collection;
using Dopamine.Services.Playback;
using Dopamine.Services.Playlist;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Timers;

namespace Dopamine.ViewModels.Common
{
    public class PlaybackControlsWithPlaylistNotificationViewModel : BindableBase
    {
        private ICollectionService collectionService;
        private IPlaybackService playbackService;
        private IPlaylistService playlistService;
        private string addedTracksToPlaylistText;
        private bool showAddedTracksToPlaylistText;
        private Timer showAddedTracksToPlaylistTextTimer;
        private int showAddedTracksToPlaylistTextSeconds = 2;
    
        public DelegateCommand PlaylistNotificationMouseEnterCommand { get; set; }
      
        public string AddedTracksToPlaylistText
        {
            get { return this.addedTracksToPlaylistText; }
            set { SetProperty<string>(ref this.addedTracksToPlaylistText, value); }
        }

        public bool ShowAddedTracksToPlaylistText
        {
            get { return this.showAddedTracksToPlaylistText; }
            set
            {
                SetProperty<bool>(ref this.showAddedTracksToPlaylistText, value);

                if (value)
                {
                    this.showAddedTracksToPlaylistTextTimer.Stop();
                    this.showAddedTracksToPlaylistTextTimer.Start();
                }
            }
        }
      
        public PlaybackControlsWithPlaylistNotificationViewModel(ICollectionService collectionService, IPlaybackService playbackService,IPlaylistService playlistService)
        {
            this.collectionService = collectionService;
            this.playbackService = playbackService;
            this.playlistService = playlistService;

            this.PlaylistNotificationMouseEnterCommand = new DelegateCommand(() => this.HideText());

            this.playlistService.TracksAdded += (numberTracksAdded, playlist) =>
            {
                string text = ResourceUtils.GetString("Language_Added_Track_To_Playlist");

                if (numberTracksAdded > 1)
                {
                    text = ResourceUtils.GetString("Language_Added_Tracks_To_Playlist");
                }

                this.AddedTracksToPlaylistText = text.Replace("{numberoftracks}", numberTracksAdded.ToString()).Replace("{playlistname}", playlist);

                this.ShowAddedTracksToPlaylistText = true;
            };

            this.playbackService.AddedTracksToQueue += iNumberOfTracks =>
            {
                string text = ResourceUtils.GetString("Language_Added_Track_To_Now_Playing");

                if (iNumberOfTracks > 1)
                {
                    text = ResourceUtils.GetString("Language_Added_Tracks_To_Now_Playing");
                }

                this.AddedTracksToPlaylistText = text.Replace("{numberoftracks}", iNumberOfTracks.ToString());

                this.ShowAddedTracksToPlaylistText = true;
            };

            this.showAddedTracksToPlaylistTextTimer = new Timer();
            this.showAddedTracksToPlaylistTextTimer.Interval = TimeSpan.FromSeconds(this.showAddedTracksToPlaylistTextSeconds).TotalMilliseconds;
            this.showAddedTracksToPlaylistTextTimer.Elapsed += ShowAddedTracksToPlaylistTextTimerElapsedHandler;
        }
     
        private void ShowAddedTracksToPlaylistTextTimerElapsedHandler(object sender, ElapsedEventArgs e)
        {
            this.HideText();
        }

        private void HideText()
        {
            this.showAddedTracksToPlaylistTextTimer.Stop();
            this.ShowAddedTracksToPlaylistText = false;
        }
    }
}