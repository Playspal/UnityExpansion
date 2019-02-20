using System;
using System.Diagnostics;

using UnityEngine;
using UnityExpansion.Utilities;

namespace UnityExpansion.UI
{
    /// <summary>
    /// Main ui layout class.
    /// Provides functionality of ui flow made in visual editor.
    /// </summary>
    [Serializable]
    public class UiLayout : UiLayoutObject
    {
        [Serializable]
        public class UiAction
        {
            public string SenderID;
            public string SenderMethod;

            public string TargetID;
            public string TargetMethod;

            public UiAction(string senderID, string senderMethod, string targetID, string targetMethod)
            {
                SenderID = senderID;
                SenderMethod = senderMethod;

                TargetID = targetID;
                TargetMethod = targetMethod;
            }

            public bool Equals(UiAction target)
            {
                return
                (
                    SenderID == target.SenderID &&
                    SenderMethod == target.SenderMethod &&
                    TargetID == target.TargetID &&
                    TargetMethod == target.TargetMethod
                );
            }
        }

        // Most methods and event can be automatically used in UiLayoutEditor by attributes,
        // but some system methods should be predefined because of specific.
        // Normally you don't need to care about it. Just bit of explanation.
        public const string PREDEFINED_METHOD_ANIMATION_PLAY = "__animation.Play";
        public const string PREDEFINED_EVENT_ANIMATION_ON_COMPLETE = "__animation.OnComplete";

        [UiLayoutEvent]
        public event Action OnStart;

        /// <summary>
        /// List of layout elements attached to this layout.
        /// </summary>
        [SerializeField]
        public UiLayoutPreset[] Presets = new UiLayoutPreset[0];

        /// <summary>
        /// List of actions registered on this layout.
        /// </summary>
        [SerializeField]
        public UiAction[] Actions = new UiAction[0];



        public void AddPreset(UiLayoutPreset preset)
        {
            Presets = Presets.Push(preset);
        }

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
                    Actions[i].SenderMethod == senderMethod &&
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

        public void ActionExecute(UiAction uiAction)
        {
            UiLayoutElement[] elements = GetComponentsInChildren<UiLayoutElement>();

            for(int i = 0; i < elements.Length; i++)
            {
                if(elements[i].UniqueID == uiAction.TargetID)
                {
                    UtilityReflection.ExecuteMethod(elements[i], uiAction.TargetMethod);
                }
            }

            for(int i = 0; i < Presets.Length; i++)
            {
                if (Presets[i].Prefab.UniqueID == uiAction.TargetID)
                {
                    if(Presets[i].Instance == null)
                    {
                        Presets[i].Instantiate(RectTransform);
                    }

                    UtilityReflection.ExecuteMethod(Presets[i].Instance, uiAction.TargetMethod);
                }
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public void ActionProcess(string senderID)
        {
            StackTrace stackTrace = new StackTrace(new StackFrame(1));
            string methodName = stackTrace.GetFrame(0).GetMethod().Name;
            
            for(int i = 0; i < Actions.Length; i++)
            {
                if(Actions[i].SenderID == senderID && Actions[i].SenderMethod == methodName)
                {
                    ActionExecute(Actions[i]);
                }
            }
        }

        protected override void Start()
        {
            base.Start();

            for(int i = 0; i < Presets.Length; i++)
            {
                //SetupPreset(Presets[i]);
            }

            ActionProcess(UniqueID);

            OnStart.InvokeIfNotNull();
        }

        private void SetupPreset(UiLayoutPreset preset)
        {
            /*
            GameObject gameObject = Resources.Load<GameObject>(preset.PrefabPath);
            UiLayoutElement layoutElement = gameObject.GetComponent<UiLayoutElement>();

            for (int n = 0; n < layoutElement.SignalsShow.Length; n++)
            {
                Debug.LogError("!!!" + layoutElement.SignalsShow[n] + " / " + gameObject);

                Signals.AddListener
                (
                    layoutElement.SignalsShow[n],
                    () =>
                    {
                        Debug.LogError("OnSignal");
                        Instantiate<UiLayoutElement>(layoutElement, RectTransform);
                    }
                );
            }
            */
        }

        /// <summary>
        /// Disables all mouse events.
        /// </summary>
        [UiLayoutMethod]
        public void MouseDisable()
        {
        }

        /// <summary>
        /// Enables all mouse events if it was disabled before.
        /// </summary>
        [UiLayoutMethod]
        public void MouseEnable()
        {
        }
    }
}