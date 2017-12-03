﻿using System;
using System.Collections.Generic;

using UnityEngine;

namespace UnityExpansion.UI.Animation
{
    [Serializable]
    public class UiAnimationClip
    {
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
        /// Start animation on specified signal.
        /// </summary>
        public bool PlayOnSignal = false;

        /// <summary>
        /// Animation will be started after the signal with specified name will be dispatched.
        /// Used if PlayOnSignal is true.
        /// </summary>
        public string PlayOnSignalName = string.Empty;

        /// <summary>
        /// Loop animation clip.
        /// </summary>
        public bool Loop = false;

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
        /// Sets the specified playback time.
        /// </summary>
        /// <param name="time">Time in seconds</param>
        public void Goto(float time)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Goto(time);
            }
        }
    }
}