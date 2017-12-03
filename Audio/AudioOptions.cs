namespace UnityExpansion.Audio
{
    /// <summary>
    /// Audio object options
    /// </summary>
    /// <seealso cref="UnityExpansion.Audio.AudioPlayer" />
    /// <seealso cref="UnityExpansion.Audio.AudioObject" />
    public class AudioOptions
    {
        /// <summary>
        /// Loop sound clip or play once.
        /// </summary>
        public bool Loop = false;

        /// <summary>
        /// Volume of sound clip.
        /// </summary>
        public float Volume = 1;

        /// <summary>
        /// Pitch of sound clip.
        /// </summary>
        public float Pitch = 1;
    }
}