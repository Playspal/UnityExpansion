using System;
using System.Collections.Generic;

using UnityEngine;

namespace UnityExpansion.UI.Animation
{
    [Serializable]
    public class UiAnimationClip
    {
        /// <summary>
        /// Invokes after animation clip is finished or stopped.
        /// </summary>
        public event Action OnComplete;

        /// <summary>
        /// Unique ID of animation clip.
        /// </summary>
        public PersistantID ID { get { return _id; } }

        /// <summary>
        /// Parent animation object.
        /// </summary>
        public UiAnimation Animation;

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
        /// Loop animation clip.
        /// </summary>
        public bool Loop = false;

        // Unique ID of animation clip.
        //[SerializeField, HideInInspector]
        [SerializeField]
        private PersistantID _id;

        public void SetAnimationController(UiAnimation animation)
        {
            Animation = animation;
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
        /// Plays this clip.
        /// </summary>
        public void Play()
        {
            if(Animation != null)
            {
                Animation.Play(this);
            }
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
            OnComplete.InvokeIfNotNull();
        }
    }
}