using UnityExpansion.IO;

namespace UnityExpansion.Audio
{
    /// <summary>
    /// Static class to play sound effects.
    /// </summary>
    /// <example>
    /// <code>
    /// using UnityExpansion.Audio;
    /// 
    /// public class MyClass
    /// {
    ///     public MyClass()
    ///     {
    ///         // Plays "MySoundEffect.wav" that is placed in "Resources/Sound" folder
    ///         Sound.Play("Sound/MySoundEffect");
    ///         
    ///         // Stops "MySoundEffect.wav"
    ///         Sound.Stop("Sound/MySoundEffect");
    ///         
    ///         // Audio options
    ///         AudioOptions options = new AudioOptions
    ///         {
    ///             Loop = false,
    ///             Volume = 0.5f,
    ///             Pitch = 1
    ///         };
    ///         
    ///         // Plays "MySoundEffect.wav" with provided options
    ///         Sound.Play("Sound/MySoundEffect", options);
    ///     }
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="UnityExpansion.Audio.AudioOptions" />
    /// <seealso cref="UnityExpansion.Audio.Music" />
    public static class Sound
    {
        private const string STASH_KEY_MUTE = "UnityExpansion.Audio.Sound.IsMuted";

        private static AudioPlayer _audioPlayer;

        /// <summary>
        /// Mutes and unmutes sound.
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
        static Sound()
        {
            _audioPlayer = new AudioPlayer("SoundContainer");
            _audioPlayer.SetMute(Stash.Get(STASH_KEY_MUTE, false));
        }

        /// <summary>
        /// Plays the specified audio file.
        /// Only one instance of audio file will be played at the same time.
        /// </summary>
        /// <param name="path">Audio file path in Resources folder</param>
        /// <param name="options">Audio options</param>
        public static void PlayOnce(string path, AudioOptions options = null)
        {
            if (_audioPlayer.IsPlaying(path))
            {
                return;
            }

            Play(path, options);
        }

        /// <summary>
        /// Plays the specified audio file.
        /// </summary>
        /// <param name="path">Audio file path in Resources folder</param>
        /// <param name="options">Audio options</param>
        public static void Play(string path, AudioOptions options = null)
        {
            _audioPlayer.Play(path, options);
        }

        /// <summary>
        /// Stops specified audio file.
        /// </summary>
        /// <param name="path">Sound clip source path in Resources folder</param>
        public static void Stop(string path)
        {
            _audioPlayer.Stop(path);
        }

        /// <summary>
        /// Stops all audio files.
        /// </summary>
        public static void StopAll()
        {
            _audioPlayer.StopAll();
        }
    }
}