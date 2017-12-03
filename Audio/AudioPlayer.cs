using System.Collections.Generic;
using UnityEngine;

namespace UnityExpansion.Audio
{
    /// <summary>
    /// Basic audio player that plays sound objects created from audio files placed in Resources folder.
    /// In most cases you don't need to use this class because
    /// Sound and Music already contains most general functionality.
    /// </summary>
    /// <example>
    /// <code>
    /// using UnityExpansion.Audio;
    /// 
    /// public class MyClass
    /// {
    ///     private AudioPlayer _audioPlayer = new AudioPlayer("MyAudio");
    ///     
    ///     public MyClass()
    ///     {
    ///         // Plays "MySoundEffect.wav" that is placed in "Resources/Sound" folder
    ///         _audioPlayer.Play("Sound/MySoundEffect");
    ///         
    ///         // Stops "MySoundEffect.wav"
    ///         _audioPlayer.Stop("Sound/MySoundEffect");
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
    ///         _audioPlayer.Play("Sound/MySoundEffect", options);
    ///     }
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="UnityExpansion.Audio.AudioObject" />
    /// <seealso cref="UnityExpansion.Audio.AudioOptions" />
    public class AudioPlayer
    {
        private GameObject _container;
        private Transform _containerTransform;

        private List<AudioObject> _audioObjects = new List<AudioObject>();

        /// <summary>
        /// Is instance currently muted.
        /// </summary>
        public bool IsMuted = false;

        /// <summary>
        /// Initializes a new instance of the AudioPlayer and creates
        /// empty GameObject to contain audio objects.
        /// </summary>
        /// <param name="name">Name of new GameObject</param>
        public AudioPlayer(string name = "AudioContainer")
        {
            _container = new GameObject(name);
            _containerTransform = _container.transform;

            GameObject.DontDestroyOnLoad(_container);
        }

        /// <summary>
        /// Mutes and unmutes current instance.
        /// </summary>
        /// <param name="value">True to mute all audio files, false to unmute</param>
        public void SetMute(bool value)
        {
            IsMuted = value;

            for (int i = 0; i < _audioObjects.Count; i++)
            {
                _audioObjects[i].SetMute(value);
            }
        }

        /// <summary>
        /// Plays specified audio file.
        /// </summary>
        /// <param name="path">Audio file path in Resources folder</param>
        /// <param name="options">Audio options</param>
        public void Play(string path, AudioOptions options = null)
        {
            AudioObject item = GetClipOrCreateNew(path);

            item.Play(options);
            item.SetMute(IsMuted);
        }

        /// <summary>
        /// Stops specified audio file.
        /// </summary>
        /// <param name="path">Audio file path in Resources folder</param>
        public void Stop(string path)
        {
            AudioObject item = GetClip(path);

            if (item != null)
            {
                item.Stop();
            }
        }

        /// <summary>
        /// Stops all audio files.
        /// </summary>
        public void StopAll()
        {
            for (int i = 0; i < _audioObjects.Count; i++)
            {
                _audioObjects[i].Stop();
            }
        }

        /// <summary>
        /// Checks specified audio file is currently playing.
        /// </summary>
        /// <param name="path">Audio file path in Resources folder</param>
        /// <returns>True if audio file is currently playing</returns>
        public bool IsPlaying(string path)
        {
            AudioObject clip = GetClip(path);

            return clip != null ? clip.IsPlaying : false;
        }

        // Get AudioObject by audio file path or create new one if not found
        private AudioObject GetClipOrCreateNew(string path)
        {
            AudioObject output = GetClipNotPlaying(path);

            if (output == null)
            {
                output = CreateClip(path);
            }

            return output;
        }

        // Get AudioObject by audio file path, return null if AudioObject is currently playing
        private AudioObject GetClipNotPlaying(string path)
        {
            return _audioObjects.Find(x => x.Path == path && !x.IsPlaying);
        }

        // Get AudioObject by audio file path
        private AudioObject GetClip(string path)
        {
            return _audioObjects.Find(x => x.Path == path);
        }

        // Creates new AudioObject and adds it to list
        private AudioObject CreateClip(string path)
        {
            AudioObject clip = new AudioObject(path);

            clip.SetParent(_containerTransform);

            _audioObjects.Add(clip);

            return clip;
        }
    }
}