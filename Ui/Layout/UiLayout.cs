using System;
using System.Collections.Generic;
using System.Diagnostics;

using UnityEngine;
using UnityExpansion.UI.Animation;
using UnityExpansion.Utilities;

namespace UnityExpansion.UI
{
    /// <summary>
    /// Main ui layout class.
    /// Provides functionality of ui flow made in visual editor.
    /// </summary>
    public class UiLayout : UiLayoutObject
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
        public List<UiLayoutElement> Prefabs = new List<UiLayoutElement>();

        /// <summary>
        /// List of actions registered on this layout.
        /// </summary>
        [SerializeField]
        public UiAction[] Actions = new UiAction[0];

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

            OnStart.InvokeIfNotNull();
        }

        protected override void Update()
        {
            base.Update();

            if(Input.GetKeyUp(KeyCode.Space))
            {
                OnStart.InvokeIfNotNull();
            }
        }

        private void SetupElement(UiLayoutObject layoutObject)
        {
            UnityEngine.Debug.LogError("SetupElement > " + layoutObject.name);

            UiAnimation animation = layoutObject.GetComponent<UiAnimation>();

            for (int i = 0; i < Actions.Length; i++)
            {
                if(Actions[i].SenderID == layoutObject.UniqueID)
                {
                    SetupElementEventHandler(layoutObject, Actions[i]);
                }

                if(animation != null)
                {
                    for(int n = 0; n < animation.AnimationClips.Count; n++)
                    {
                        UiAnimationClip animationClip = animation.AnimationClips[n];

                        if (Actions[i].SenderID == layoutObject.UniqueID + "." + animationClip.ID)
                        {
                            SetupAnimationHandler(layoutObject, animation, Actions[i]);
                        }
                    }
                }
            }
            
        }

        private void SetupElementEventHandler(UiLayoutObject layoutObject, UiAction action)
        {
            UtilityReflection.AddEventHandler
            (
                layoutObject,
                action.SenderEvent,
                () =>
                {
                    InvokeMethod(action.TargetID, action.TargetMethod);
                }
            );
        }

        private void SetupAnimationHandler(UiLayoutObject layoutObject, UiAnimation animation, UiAction action)
        {
            UnityEngine.Debug.LogError(" >> SetupAnimationHandler");

            animation.OnComplete += (UiAnimationClip clip) =>
            {
                UnityEngine.Debug.LogError(" >> " + layoutObject.name + " > " + layoutObject.UniqueID + "." + clip.ID);
                if (action.SenderID == layoutObject.UniqueID + "." + clip.ID)
                {
                    InvokeMethod(action.TargetID, action.TargetMethod);
                }
            };
        }

        private void InvokeMethod(string uniqueID, string methodName)
        {
            UiLayoutObject layoutObject = FindObjectInInstances(uniqueID);

            if (layoutObject == null)
            {
                UiLayoutElement layoutPrefab = FindPrefab(uniqueID);
                
                if (layoutPrefab != null)
                {
                    // TODO: use container here instead of "this"
                    layoutObject = Instantiate(layoutPrefab, this);
                }
            }

            if(layoutObject != null)
            {
                UtilityReflection.ExecuteMethod(layoutObject, methodName);
                
            }

            UnityEngine.Debug.LogError("methodName > " + methodName);
        }

        private UiLayoutElement FindPrefab(string uniqueID)
        {
            for (int i = 0; i < Prefabs.Count; i++)
            {
                if (Prefabs[i].UniqueID == uniqueID)
                {
                    return Prefabs[i];
                }
            }

            return null;
        }

        private UiLayoutObject FindObjectInInstances(string uniqueID)
        {
            UiLayoutObject[] layoutObjects = GetComponentsInChildren<UiLayoutObject>();

            for (int i = 0; i < layoutObjects.Length; i++)
            {
                if (layoutObjects[i].UniqueID == uniqueID)
                {
                    return layoutObjects[i];
                }
            }

            return null;
        }

        private UiLayoutElement Instantiate(UiLayoutElement prefab, UiLayoutObject container)
        {
            UiLayoutElement layoutObject = GameObject.Instantiate(prefab, container.RectTransform);

            SetupElement(layoutObject);

            return layoutObject;
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