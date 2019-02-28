using System;

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
                    // TODO: uncomment
                    //Data.hideFlags = HideFlags.HideInInspector;
                }

                _selection = Selection.activeGameObject;

                OnChanged.InvokeIfNotNull();
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
    }
}