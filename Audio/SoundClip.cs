using UnityEngine;

namespace UnityExpansion.Audio
{
    public class SoundClip
    {
        private GameObject _container;
        private Transform _containerTransform;

        private AudioClip _audioClip;
        private AudioSource _audioSource;

        private SoundOptions _soundOptions;

        /// <summary>
        /// Sound clip's source path in Resources folder.
        /// </summary>
        public string Path;

        /// <summary>
        /// Is the clip playing right now.
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return _audioSource.isPlaying;
            }
        }

        /// <summary>
        /// Creates new SoundClip from specified audio source.
        /// </summary>
        /// <param name="path">Audio source path in Resources folder</param>
        public SoundClip(string path)
        {
            Path = path;

            _container = new GameObject("SoundItem");
            _containerTransform = _container.transform;

            _audioClip = Resources.Load(path) as AudioClip;
            _audioSource = _container.AddComponent<AudioSource>();
            _audioSource.clip = _audioClip;
        }

        /// <summary>
        /// Plays this clip with specified options.
        /// </summary>
        /// <param name="options">Sound options</param>
        public void Play(SoundOptions options = null)
        {
            if (options != null)
            {
                _audioSource.volume = options.Volume;
                _audioSource.pitch = options.Pitch;
                _audioSource.loop = options.Loop;
            }

            _soundOptions = options;

            _audioSource.Play();
        }

        /// <summary>
        /// Stops this clip.
        /// </summary>
        public void Stop()
        {
            _audioSource.Stop();
        }

        /// <summary>
        /// Mutes and unmutes the clip.
        /// </summary>
        public void SetMute(bool value)
        {
            _audioSource.mute = value;
        }

        /// <summary>
        /// Sets container's parent transform.
        /// </summary>
        /// <param name="parent">New parent transform</param>
        public void SetParent(Transform parent)
        {
            _containerTransform.SetParent(parent);
        }
    }
}