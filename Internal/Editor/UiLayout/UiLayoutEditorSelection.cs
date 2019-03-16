using System;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using UnityExpansion.UI.Layout;
using UnityExpansion.UI.Layout.Processor;
using UnityExpansion.Utilities;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class UiLayoutEditorSelection
    {
        public event Action OnChanged;

        public UiLayout Target { get; private set; }

        public UiLayoutProcessor TargetProcessor { get; private set; }

        public InternalUiLayoutData Data { get; private set; }

        public List<RectTransform> Containers = new List<RectTransform>();

        private GameObject _selection = null;

        public UiLayoutEditorSelection()
        {
            Update();
        }

        public void Update()
        {
            if (Selection.activeGameObject != _selection)
            {
                if(Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<UiLayout>() == null)
                {
                    Target = null;
                    TargetProcessor = null;

                    Data = null;
                }
                else
                {
                    Target = Selection.activeGameObject.GetComponent<UiLayout>();
                    TargetProcessor = UtilityReflection.GetMemberValue(Target, "_processor") as UiLayoutProcessor;
                    Data = Target.gameObject.GetOrAddComponent<InternalUiLayoutData>();
                    //Data.hideFlags = HideFlags.HideInInspector;
                }

                _selection = Selection.activeGameObject;

                OnChanged.InvokeIfNotNull();
            }

            Containers.Clear();

            if(Target != null)
            {
                AddContainer(Target.gameObject);

                for (int i = 0; i < Target.transform.childCount; i++)
                {
                    AddContainer(Target.transform.GetChild(i).gameObject);
                }
            }
        }

        public UiLayoutProcessorPreset ProcessorPresetCreate(UiLayoutElement element)
        {
            UiLayoutProcessorPreset preset = new UiLayoutProcessorPreset(element);

            // TODO:
            preset.Container = Target.RectTransform;

            TargetProcessor.Presets.Add(preset);

            return preset;
        }

        public UiLayoutProcessorPreset ProcessorPresetFind(string id)
        {
            return TargetProcessor.Presets.Find(x => x.ID == id);
        }

        /// <summary>
        /// Check is the element already added to processor.
        /// </summary>
        /// <returns>True if element is unique</returns>
        public bool ProcessorPresetUniqueCheck(UiLayoutElement element)
        {
            return TargetProcessor.Presets.Find(x => x.Prefab == element) == null;
        }

        /// <summary>
        /// Creates new edict in case the same one is not exists.
        /// </summary>
        public void ProcessorEdictCreate(string senderID, string senderEvent, string handlerID, string handlerMethod)
        {
            UiLayoutProcessorEdict edict = TargetProcessor.Edicts.Find
            (
                x =>
                x.SenderID == senderID &&
                x.SenderEvent == senderEvent &&
                x.HandlerID == handlerID &&
                x.HandlerMethod == handlerMethod
            );

            if (edict == null)
            {
                TargetProcessor.Edicts.Add
                (
                    new UiLayoutProcessorEdict(senderID, senderEvent, handlerID, handlerMethod)
                );
            }
        }

        /// <summary>
        /// Removes all specified edicts.
        /// </summary>
        public void ProcessorEdictRemove(string senderID, string senderEvent, string handlerID, string handlerMethod)
        {
            TargetProcessor.Edicts.RemoveAll
            (
                x =>
                x.SenderID == senderID &&
                x.SenderEvent == senderEvent &&
                x.HandlerID == handlerID &&
                x.HandlerMethod == handlerMethod
            );
        }

        private void AddContainer(GameObject gameObject)
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                Containers.Add(rectTransform);
            }
        }
    }
}