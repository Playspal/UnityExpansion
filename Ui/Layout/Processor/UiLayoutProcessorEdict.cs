using System;
using UnityExpansion.Utilities;

namespace UnityExpansion.UI.Layout.Processor
{
    /// <summary>
    /// TODO: Write description
    /// </summary>
    [Serializable]
    public class UiLayoutProcessorEdict
    {
        /// <summary>
        /// Unique id of sender object.
        /// </summary>
        public string SenderID;

        /// <summary>
        /// Sender event to handle.
        /// </summary>
        public string SenderEvent;

        /// <summary>
        /// Unique id of handler object.
        /// </summary>
        public string HandlerID;

        /// <summary>
        /// Handler method.
        /// </summary>
        public string HandlerMethod;

        // Instance of sender object.
        private object _sender;

        // Instance of handler object.
        private object _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="UiLayoutProcessorEdict"/> class.
        /// </summary>
        public UiLayoutProcessorEdict(string senderID, string senderEvent, string handlerID, string handlerMethod)
        {
            SenderID = senderID;
            SenderEvent = senderEvent;
            HandlerID = handlerID;
            HandlerMethod = handlerMethod;
        }

        public void SetSenderObject(object sender)
        {
            if (_sender != sender)
            {
                _sender = sender;

                UtilityReflection.AddEventHandler(_sender, SenderEvent, OnEvent);
            }
        }

        public void SetHandlerObject(object handler)
        {
            _handler = handler;
        }

        private void OnEvent()
        {
            if (_handler != null)
            {
                UtilityReflection.ExecuteMethod(_handler, HandlerMethod);
            }
        }
    }
}