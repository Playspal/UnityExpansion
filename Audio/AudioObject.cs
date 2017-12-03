using UnityEngine;

namespace UnityExpansion.Audio
{
    /// <summary>
    /// Creates AudioClip from specified audio file and attaches it to
    /// new GameObject with AudioSource component to provide basic sound functionality.
    /// In most cases you don't need to use this class because
    /// Sound and Music already contains most general functionality.
    /// </summary>
    /// <example>
    /// <code>
    /// using UnityExpansion.Audio;
    /// 
    /// public class MyClass
    /// {
    ///     public MyClass()
    ///     {
    ///         // Creates new AudioObject with "MySoundEffect.wav" that is placed in "Resources/Sound" folder
    ///         AudioObject myAudioObject = new AudioObject("Sound/MySoundEffect");
    /// 
    ///         // Plays sound
    ///         myAudioObject.Play();
    ///         
    ///         // Stops sound
    ///         myAudioObject.Stop();
    ///         
    ///         // Audio options
    ///         AudioOptions options = new AudioOptions
    ///         {
    ///             Loop = false,
    ///             Volume = 0.5f,
    ///             Pitch = 1
    ///         };
    ///         
    ///         // Plays sound with provided options
    ///         myAudioObject.Play(options);
    ///     }
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="UnityExpansion.Audio.AudioPlayer" />
    /// <seealso cref="UnityExpansion.Audio.AudioOptions" />
    public class AudioObject
    {
        private GameObject _container;
        private Transform _containerTransform;

        private AudioClip _audioClip;
        private AudioSource _audioSource;

        /// <summary>
        /// Audio file path in Resources folder.
        /// </summary>
        public string Path;

        /// <summary>
        /// Is the instance playing right now.
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return _audioSource.isPlaying;
            }
        }

        /// <summary>
        /// Creates new AudioObject from specified audio file.
        /// </summary>
        /// <param name="path">Audio file path in Resources folder</param>
        public AudioObject(string path)
        {
            Path = path;

            _container = new GameObject("SoundItem");
            _containerTransform = _container.transform;

            _audioClip = Resources.Load(path) as AudioClip;
            _audioSource = _container.AddComponent<AudioSource>();
            _audioSource.clip = _audioClip;
        }

        /// <summary>
        /// Plays this instance with specified options.
        /// </summary>
        /// <param name="options">Audio options</param>
        public void Play(AudioOptions options = null)
        {
            if (options != null)
            {
                _audioSource.volume = options.Volume;
                _audioSource.pitch = options.Pitch;
                _audioSource.loop = options.Loop;
            }

            _audioSource.Play();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            _audioSource.Stop();
        }

        /// <summary>
        /// Mutes and unmutes this instance.
        /// </summary>
        public void SetMute(bool value)
        {
            _audioSource.mute = value;
        }

        /// <summary>
        /// Sets parent transform for this instance.
        /// </summary>
        /// <param name="parent">New parent transform</param>
        public void SetParent(Transform parent)
        {
            _containerTransform.SetParent(parent);
        }
    }
}