using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityExpansion.Editor;
using UnityExpansion.UI;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class UiLayoutEditorDragAndDrop
    {
        public event Action<UiLayoutElement> OnSuccess;

        private UiLayoutEditor _layoutEditor;
        private EditorLayoutObjectTexture _preview;
        private EditorLayoutObjectText _message;

        private UiLayoutElement _prefab;

        private bool _isValid = false;

        public UiLayoutEditorDragAndDrop(UiLayoutEditor layoutEditor)
        {
            _layoutEditor = layoutEditor;
            _layoutEditor.Mouse.OnDragPrefabsStarted += OnMouseDragPrefabsStarted;
            _layoutEditor.Mouse.OnDragPrefabsUpdated += OnMouseDragPrefabsUpdated;
            _layoutEditor.Mouse.OnDragPrefabsEnded += OnMouseDragPrefabsEnded;
            _layoutEditor.Mouse.OnDragPrefabsCanceled += OnMouseDragPrefabsCanceled;

            _preview = new EditorLayoutObjectTexture(layoutEditor, 300, 50);
            _preview.Fill(new Color(1, 1, 1, 0));
            _preview.DrawBorderTop(1, Color.white);
            _preview.DrawBorderLeft(1, Color.white);
            _preview.SetActive(false);

            _message = new EditorLayoutObjectText(layoutEditor, 500, 20);
            _message.SetAlignment(TextAnchor.UpperLeft);
            _message.SetActive(false);
        }

        private void OnMouseDragPrefabsStarted(UnityEngine.Object[] selectedObjects)
        {
            if (selectedObjects.Length == 1)
            {
                UnityEngine.Object objectReference = selectedObjects[0];
                PrefabType prefabType = PrefabUtility.GetPrefabType(objectReference);

                if (prefabType == PrefabType.Prefab)
                {
                    string prefabPath = AssetDatabase.GetAssetPath(objectReference);

                    GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                    UiLayoutElement layoutElement = gameObject.GetComponent<UiLayoutElement>();

                    if(layoutElement != null)
                    {
                        Match match = Regex.Match
                        (
                            prefabPath,
                            @"Resources/([A-Za-z0-9\-\/.]*).prefab",
                            RegexOptions.IgnoreCase
                        );

                        if(match.Success)
                        {
                            bool isUnique = true;

                            _prefab = layoutElement;

                            if (_layoutEditor.Selection.Target != null)
                            {
                                for (int i = 0; i < _layoutEditor.Selection.Target.Prefabs.Count; i++)
                                {
                                    if (_layoutEditor.Selection.Target.Prefabs[i] == _prefab)
                                    {
                                        isUnique = false;
                                        break;
                                    }
                                }
                            }

                            if (isUnique)
                            {
                                PreviewShow();
                            }
                            else
                            {
                                PreviewShow("This prefab already added to layout");
                            }
                        }
                        else
                        {
                            PreviewShow("Unexpected error");
                        }
                    }
                    else
                    {
                        PreviewShow("Only UiLayoutElement allowed");
                    }
                }
                else
                {
                    PreviewShow("Only prefabs allowed to drag and drop here");
                }
            }
            else
            {
                PreviewShow("Drag and drop for multiple objects is not allowed");
            }
        }

        private void OnMouseDragPrefabsUpdated(UnityEngine.Object[] selectedObjects)
        {
            _preview.X = _layoutEditor.Mouse.X - _layoutEditor.CanvasX;
            _preview.Y = _layoutEditor.Mouse.Y - _layoutEditor.CanvasY;

            _message.X = _layoutEditor.Mouse.X - _layoutEditor.CanvasX + 10;
            _message.Y = _layoutEditor.Mouse.Y - _layoutEditor.CanvasY + 10;
        }

        private void OnMouseDragPrefabsEnded(UnityEngine.Object[] selectedObjects)
        {
            if (_isValid)
            {
                OnSuccess(_prefab);
            }

            PreviewHide();
        }

        private void OnMouseDragPrefabsCanceled()
        {
            PreviewHide();
        }

        private void PreviewShow(string message = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                _preview.SetActive(true);
                _preview.SetAsLastSibling();

                _isValid = true;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            }
            else
            {
                _message.SetActive(true);
                _message.SetText(message);
                _message.SetAsLastSibling();

                _isValid = false;

                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
            }
        }

        private void PreviewHide()
        {
            _preview.SetActive(false);
            _message.SetActive(false);

            _isValid = false;
            _prefab = null;

            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
        }
    }
}