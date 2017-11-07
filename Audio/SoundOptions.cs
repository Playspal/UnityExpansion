namespace UnityExpansion.Audio
{
    /// <summary>
    /// Sound clip options
    /// </summary>
    public class SoundOptions
    {
        /// <summary>
        /// Loop sound clip or play once.
        /// </summary>
        public bool Loop = false;

        /// <summary>
        /// If unique only one instance of sound clip will be played at the same time.
        /// </summary>
        public bool Unique = false;

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