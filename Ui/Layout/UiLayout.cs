using System;
using System.Collections.Generic;

using UnityEngine;
using UnityExpansion.Utilities;

namespace UnityExpansion.UI
{
    /// <summary>
    /// Main ui layout class.
    /// Provides functionality of ui flow made in visual editor.
    /// </summary>
    public class UiLayout : UiLayoutElement
    {
        [Serializable]
        public class UiAction
        {
            public string SenderID;
            public string SenderEvent;

            public string TargetID;
            public string TargetMethod;

            public UiAction(string senderID, string senderMethod, string targetID, string targetMethod)
            {
                SenderID = senderID;
                SenderEvent = senderMethod;
                TargetID = targetID;
                TargetMethod = targetMethod;
            }

            public bool Equals(UiAction target)
            {
                return
                (
                    SenderID == target.SenderID &&
                    SenderEvent == target.SenderEvent &&
                    TargetID == target.TargetID &&
                    TargetMethod == target.TargetMethod
                );
            }
        }

        [UiLayoutEvent]
        public event Action OnStart;

        [UiLayoutEvent]
        public event Action OnTest1;

        [UiLayoutEvent]
        public event Action OnTest2;

        /// <summary>
        /// List of layout elements attached to this layout.
        /// </summary>
        [SerializeField]
        public List<UiLayoutElement> Prefabs = new List<UiLayoutElement>();

        /// <summary>
        /// List of actions registered on this layout.
        /// </summary>
        [SerializeField]
        public UiAction[] Actions = new UiAction[0];

        private Dictionary<string, object> _registeredObjects = new Dictionary<string, object>();

        public void ActionAdd(string senderID, string senderMethod, string targetID, string targetMethod)
        {
            ActionAdd(new UiAction(senderID, senderMethod, targetID, targetMethod));
        }

        public void ActionAdd(UiAction uiAction)
        {
            for (int i = 0; i < Actions.Length; i++)
            {
                if(Actions[i].Equals(uiAction))
                {
                    return;
                }
            }

            Actions = Actions.Push(uiAction);
        }

        public void ActionRemove(string senderID, string senderMethod, string targetID, string targetMethod)
        {
            for(int i = 0; i < Actions.Length; i++)
            {
                if
                (
                    Actions[i].SenderID == senderID &&
                    Actions[i].SenderEvent == senderMethod &&
                    Actions[i].TargetID == targetID &&
                    Actions[i].TargetMethod == targetMethod
                )
                {
                    ActionRemove(Actions[i]);
                }
            }
        }
        
        public void ActionRemove(UiAction uiAction)
        {
            Actions = Actions.Remove(uiAction);
        }

        protected override void Start()
        {
            base.Start();

            SetupElement(this);

            // Register prefabs
            for(int i = 0; i < Prefabs.Count; i++)
            {
                UiLayoutElement instance = GameObject.Instantiate(Prefabs[i], RectTransform);
                instance.SetActive(false);

                SetupElement(instance);
            }

            OnStart.InvokeIfNotNull();
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                OnTest1.InvokeIfNotNull();
            }

            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                OnTest2.InvokeIfNotNull();
            }
        }

        private void SetupElement(UiLayoutElement layoutElement)
        {
            // If object have PersistantID than it may be used in ui flow
            List<CommonPair<string, object>> objectsWithPersistantID = new List<CommonPair<string, object>>();

            // Find all objects with PersistantID and register them.
            PersistantIDExplorer.Explore
            (
                layoutElement.gameObject,
                SetupObjectWithPersistandID
            );
        }

        private void SetupObjectWithPersistandID(object targetObject, PersistantID persistantID)
        {
            string id = persistantID.ToString();

            for (int i = 0; i < Actions.Length; i++)
            {
                if (Actions[i].SenderID == id)
                {
                    SetupEventHandler(targetObject, Actions[i]);
                }
            }

            _registeredObjects.Add(id.ToString(), targetObject);
        }

        private void SetupEventHandler(object targetObject, UiAction action)
        {
            UtilityReflection.AddEventHandler
            (
                targetObject,
                action.SenderEvent,
                () => InvokeMethod(action.TargetID, action.TargetMethod)
            );
        }

        private void InvokeMethod(string persistantID, string methodName)
        {
            if (_registeredObjects.ContainsKey(persistantID))
            {
                UtilityReflection.ExecuteMethod(_registeredObjects[persistantID], methodName);
            }
        }

        /// <summary>
        /// Disables all mouse events.
        /// </summary>
        [UiLayoutMethod]
        public void MouseDisable() { }

        /// <summary>
        /// Enables all mouse events if it was disabled before.
        /// </summary>
        [UiLayoutMethod]
        public void MouseEnable() { UnityEngine.Debug.LogError("NNN"); }
    }
}