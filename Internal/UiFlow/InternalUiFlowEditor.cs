using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityExpansionInternal
{
    public class InternalUiFlowEditor : InternalWizard
    {
        public static InternalUiFlowEditor Instance { get; private set; }

        protected override void OnWizardCreate()
        {
            base.OnWizardCreate();
            Debug.LogError("!!!222");
        }

        protected override void OnGUI()
        {
            base.OnGUI();

            EditorGUILayout.HelpBox("123", MessageType.Info);
        }

        /// <summary>
        /// Shows the window.
        /// </summary>
        [MenuItem("Window/UI Flow Editor")]
        public static void ShowWindow()
        {
            Instance = DisplayWizard<InternalUiFlowEditor>("UI Flow Editor", "sdfsdfs", "asdadas");
        }
    }
}
