using UnityExpansion.IO;

namespace UnityExpansion.Audio
{
    /// <summary>
    /// Static class to play background music. Only one music track can be played at the same time.
    /// </summary>
    /// <example>
    /// <code>
    /// using UnityExpansion.Audio;
    /// 
    /// public class MyClass
    /// {
    ///     public void GotoMainMenu()
    ///     {
    ///         // Plays "MainMenuMusic.wav" that is placed in "Resources/Sound" folder
    ///         Music.Play("MainMenuMusic.wav");
    ///     }
    ///     
    ///     public void GotoGameplay()
    ///     {
    ///         // Audio options
    ///         AudioOptions options = new AudioOptions
    ///         {
    ///             Loop = true,
    ///             Volume = 0.5f,
    ///             Pitch = 1
    ///         };
    ///         
    ///         // Plays "GameplayMusic.wav" with provided options
    ///         Music.Play("GameplayMusic.wav", options);
    ///     }
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="UnityExpansion.Audio.AudioOptions" />
    /// <seealso cref="UnityExpansion.Audio.Sound" />
    public static class Music
    {
        private const string STASH_KEY_MUTE = "UnityExpansion.Audio.Music.IsMuted";

        private static AudioPlayer _audioPlayer;

        /// <summary>
        /// Mutes and unmutes music.
        /// Set true to mute all audio files, false to unmute.
        /// </summary>
        public static bool Mute
        {
            get
            {
                return _audioPlayer.IsMuted;
            }

            set
            {
                _audioPlayer.SetMute(value);

                Stash.Set(STASH_KEY_MUTE, value);
            }
        }

        /// <summary>
        /// Initializes the Sound class.
        /// </summary>
        static Music()
        {
            _audioPlayer = new AudioPlayer("MusicContainer");
            _audioPlayer.SetMute(Stash.Get(STASH_KEY_MUTE, false));
        }

        /// <summary>
        /// Plays the specified music track.
        /// If another track is currently playing it will be stopped.
        /// </summary>
        /// <param name="path">Audio file path in Resources folder</param>
        /// <param name="options">Audio options</param>
        public static void Play(string path, AudioOptions options = null)
        {
            if (_audioPlayer.IsPlaying(path))
            {
                return;
            }

            _audioPlayer.StopAll();
            _audioPlayer.Play(path, options);
        }

        /// <summary>
        /// Stops music.
        /// </summary>
        public static void Stop()
        {
            _audioPlayer.StopAll();
        }
    }
}