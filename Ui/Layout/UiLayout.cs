using System;

using UnityEngine;
using UnityExpansion.UI.Layout.Processor;

namespace UnityExpansion.UI.Layout
{
    /// <summary>
    /// Main ui layout class.
    /// Provides functionality of ui flow made in visual editor.
    /// </summary>
    public class UiLayout : UiLayoutElement
    {
        [UiLayoutProcessorEvent]
        public event Action OnStart;

        [SerializeField]
        private UiLayoutProcessor _processor = new UiLayoutProcessor();
        
        protected override void Start()
        {
            base.Start();

            _processor.Setup(this);

            OnStart.InvokeIfNotNull();
        }
    }
}