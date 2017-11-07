using System.Collections.Generic;
using UnityEngine;
using UnityExpansion.IO;

namespace UnityExpansion.Audio
{
    /// <summary>
    /// Provides sound player functionality.
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
    ///         // Sound options
    ///         SoundOptions options = new SoundOptions
    ///         {
    ///             Loop = false,
    ///             Unique = false,
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
    /// <seealso cref="UnityExpansion.Audio.SoundOptions" />
    public class Sound
    {
        private static GameObject _container;
        private static Transform _containerTransform;

        private static List<SoundClip> _clips = new List<SoundClip>();

        /// <summary>
        /// Is sound currently muted.
        /// </summary>
        public static bool IsMuted = false;

        /// <summary>
        /// Initializes the Sound class.
        /// </summary>
        static Sound()
        {
            _container = new GameObject("SoundContainer");
            _containerTransform = _container.transform;

            IsMuted = Cache.Get("UnityExpansion.Audio.Sound.IsMuted", false);

            GameObject.DontDestroyOnLoad(_container);
        }

        /// <summary>
        /// Mutes and unmutes all sound clips
        /// </summary>
        /// <param name="value">Value</param>
        public static void SetMute(bool value)
        {
            IsMuted = value;

            for(int i = 0; i < _clips.Count; i++)
            {
                _clips[i].SetMute(value);
            }

            Cache.Set("UnityExpansion.Audio.Sound.IsMuted", IsMuted);
        }

        /// <summary>
        /// Plays the specified audio source.
        /// </summary>
        /// <param name="path">Audio source path in Resources folder</param>
        /// <param name="options">Sound options</param>
        public static void Play(string path, SoundOptions options = null)
        {
            if(options.Unique && IsClipPlaying(path))
            {
                return;
            }

            SoundClip item = GetClipOrCreateNew(path);
            item.Play(options);
            item.SetMute(IsMuted);
        }

        /// <summary>
        /// Stops sound clip with specified source path.
        /// </summary>
        /// <param name="path">Sound clip source path in Resources folder</param>
        public static void Stop(string path)
        {
            SoundClip item = GetClip(path);

            if (item != null)
            {
                item.Stop();
            }
        }

        /// <summary>
        /// Stops all sound clips.
        /// </summary>
        public static void StopAll()
        {
            for(int i = 0; i < _clips.Count; i++)
            {
                _clips[i].Stop();
            }
        }

        // Checks the clip with specified path is currently playing.
        private static bool IsClipPlaying(string path)
        {
            SoundClip clip = GetClip(path);

            return clip != null ? clip.IsPlaying : false;
        }

        // Get clip by path or create new one if clip not found
        private static SoundClip GetClipOrCreateNew(string path)
        {
            SoundClip output = GetClipNotPlaying(path);

            if (output == null)
            {
                output = CreateClip(path);
            }

            return output;
        }

        // Get clip by path, return null if clip is currently playing
        private static SoundClip GetClipNotPlaying(string path)
        {
            return _clips.Find(x => x.Path == path && !x.IsPlaying);
        }

        // Get clip by path
        private static SoundClip GetClip(string path)
        {
            return _clips.Find(x => x.Path == path);
        }

        // Creates new sound clip
        private static SoundClip CreateClip(string path)
        {
            SoundClip clip = new SoundClip(path);

            clip.SetParent(_containerTransform);

            _clips.Add(clip);

            return clip;
        }
    }
}