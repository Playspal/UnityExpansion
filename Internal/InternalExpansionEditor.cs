#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityExpansion;
using UnityExpansion.UI;

namespace UnityExpansionInternal
{
    [CustomEditor(typeof(Expansion))]
    public class InternalExpansionEditor : Editor
    {

        private Editor _editor;

        public override void OnInspectorGUI()
        {
            Expansion expansion = target as Expansion;

            EditorGUILayout.LabelField("It is the main UnityExpansion object. If you have any question read the documentation at aokov.se/expansion/");

            EditorGUILayout.Space();

            if (expansion.LayoutSettings != null)
                {
                    if (_editor == null)
                    {
                        _editor = Editor.CreateEditor(expansion.LayoutSettings);
                    }

                    if (_editor != null)
                    {
                        _editor.OnInspectorGUI();
                    }
                }
           
        }
    }
   
}
#endif