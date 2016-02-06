using Motive.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Motive.Core.Models;
using Motive.Core.Diagnostics;
using Motive.Core.Media;
using Motive.Core.Timing;

public class AudioContentPlayer : SingletonComponent<AudioContentPlayer>
{
    public float FadeDuration = 5f;

    private Logger m_logger;

    IAudioPlayerChannel m_channel;

    Dictionary<LocalizedAudioContent, IAudioPlayer> m_playablePlayers;

    List<IAudioPlayer> m_soundtrackPlayers;

    bool m_isPlayingSoundtrack;

    IAudioPlayer CurrentSoundtrackPlayer
    {
        get
        {
            if (m_soundtrackPlayers.Count > 0)
            {
                return m_soundtrackPlayers.Last();
            }

            return null;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        m_logger = new Logger(this);
    }

    protected override void Start()
    {
        m_channel = Platform.Instance.CreateAudioPlayerChannel();
        m_soundtrackPlayers = new List<IAudioPlayer>();
        m_playablePlayers = new Dictionary<LocalizedAudioContent, IAudioPlayer>();
    }

    public void PlayAudioContent(LocalizedAudioContent audioContent, string route, Action onComplete)
    {
        if (audioContent == null)
        {
            m_logger.Warning("Playable did not contain audio content!");

            if (onComplete != null)
            {
                onComplete();
            }

            return;
        }

        var path = WebServices.Instance.MediaDownloadManager.GetPathForItem(audioContent.MediaItem.Url);
        var player = m_channel.CreatePlayer(new Uri(path));
        player.Loop = audioContent.Loop;
        player.Volume = audioContent.Volume;

        lock (m_playablePlayers)
        {
            m_playablePlayers.Add(audioContent, player);
        }

        // Set up values

        if (route == PlayableContentRoute.Ambient)
        {
            player.Play((success) => { onComplete(); });
        }
        else
        {
            bool fadeIn = false;

            if (CurrentSoundtrackPlayer != null && m_isPlayingSoundtrack)
            {
                fadeIn = true;

                Fader.FadeOut(CurrentSoundtrackPlayer, TimeSpan.FromSeconds(FadeDuration));
            }

            m_soundtrackPlayers.Add(player);

            if (m_isPlayingSoundtrack)
            {
                if (fadeIn)
                {
                    Fader.FadeIn(player, TimeSpan.FromSeconds(FadeDuration), audioContent.Volume);
                }
                else
                {
                    player.Play();
                }
            }
        }
    }

    public void StopPlaying(LocalizedAudioContent audioContent)
    {
        IAudioPlayer player = null;

        if (m_playablePlayers.TryGetValue(audioContent, out player))
        {
            m_playablePlayers.Remove(audioContent);

            if (m_soundtrackPlayers.Contains(player))
            {
                var origSoundtrackPlayer = CurrentSoundtrackPlayer;

                m_soundtrackPlayers.Remove(player);

                if (m_isPlayingSoundtrack)
                {
                    if (origSoundtrackPlayer == player)
                    {
                        // If we just removed the currently playing
                        // soundtrack player

                        Fader.FadeOut(origSoundtrackPlayer, TimeSpan.FromSeconds(FadeDuration), () => { origSoundtrackPlayer.Dispose(); });

                        if (CurrentSoundtrackPlayer != null)
                        {
                            // If we are swapping another one in, fade it in

                            Fader.FadeIn(CurrentSoundtrackPlayer, TimeSpan.FromSeconds(FadeDuration));
                        }
                    }
                    else
                    {
                        player.Dispose();
                    }
                }
                else
                {
                    player.Dispose();
                }
            }
        }
    }

    public void StartSoundtrack()
    {
        m_isPlayingSoundtrack = true;

        if (CurrentSoundtrackPlayer != null)
        {
            CurrentSoundtrackPlayer.Play();
        }
    }
}
