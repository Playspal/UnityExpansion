using System;
using System.Collections.Generic;

using UnityEngine;

namespace UnityExpansion.UI.Animation
{
    [Serializable]
    public class UiAnimationClip
    {
        /// <summary>
        /// Unique ID of animation clip.
        /// </summary>
        public string ID { get { return _id; } }

        /// <summary>
        /// The name.
        /// </summary>
        public string Name = "Animation Clip";

        /// <summary>
        /// List of animation segments.
        /// </summary>
        public List<UiAnimationClipSegment> Items = new List<UiAnimationClipSegment>();

        /// <summary>
        /// Start animation clip on awake.
        /// </summary>
        public bool PlayOnAwake = false;

        /// <summary>
        /// Start animation if parent UiLayoutElement show begin.
        /// </summary>
        public bool PlayOnLayoutElementShow = false;

        /// <summary>
        /// Start animation if parent UiLayoutElement hide begin.
        /// </summary>
        public bool PlayOnLayoutElementHide = false;

        /// <summary>
        /// Animation will be started after the signal with specified name will be dispatched.
        /// Used if PlayOnSignal is true.
        /// </summary>
        public string[] PlayOnSignals = new string[0];

        /// <summary>
        /// Loop animation clip.
        /// </summary>
        public bool Loop = false;

        // Unique ID of animation clip.
        //[SerializeField, HideInInspector]
        [SerializeField]
        private string _id;

        /// <summary>
        /// Initializes a new instance of the <see cref="UiAnimationClip"/> class.
        /// </summary>
        public UiAnimationClip()
        {
            _id = "a" + new System.Random().Next(1000000, 9999999).ToString();
        }

        /// <summary>
        /// Gets animation clip duration.
        /// </summary>
        /// <returns>Time in seconds</returns>
        public float GetDuration()
        {
            float output = 0;

            for (int i = 0; i < Items.Count; i++)
            {
                output = Mathf.Max(Items[i].Delay + Items[i].Duration, output);
            }

            return output;
        }

        /// <summary>
        /// Rewinds animation clip to specified time.
        /// </summary>
        /// <param name="time">Time in seconds</param>
        public void Rewind(float time)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Goto(time);
            }
        }

        /// <summary>
        /// Rewinds animation clip to the begining (first frame).
        /// </summary>
        public void RewindToBegin()
        {
            Rewind(0);
        }

        /// <summary>
        /// Rewinds animation clip to the end (last frame).
        /// </summary>
        public void RewindToEnd()
        {
            Rewind(GetDuration());
        }
    }
}