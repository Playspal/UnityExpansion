using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace UnityExpansion.UI
{
    /// <summary>
    /// Ui manager. Keeps all data about all Ui objects.
    /// </summary>
    public class Ui
    {
        /// <summary>
        /// Attached canvas.
        /// </summary>
        public static Canvas Canvas;

        /// <summary>
        /// List of layers inside of canvas.
        /// </summary>
        public static List<RectTransform> CanvasLayers = new List<RectTransform>();

        /// <summary>
        /// Scale factor of attached canvas.
        /// </summary>
        public static float ScaleFactor
        {
            get
            {
                return Canvas.scaleFactor;
            }
        }

        /// <summary>
        /// Gets specified layer from CanvasLayers list.
        /// </summary>
        /// <param name="index">Layer's index</param>
        /// <returns>RectTransform of specified layer or null if layer not found</returns>
        public static RectTransform GetLayer(int index)
        {
            return CanvasLayers[index];
        }
    }
}