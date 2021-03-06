﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class UiLayoutEditorCurves
    {
        private List<UiLayoutEditorCurve> _curvesBackground = new List<UiLayoutEditorCurve>();
        private List<UiLayoutEditorCurve> _curvesFrontground = new List<UiLayoutEditorCurve>();

        public void AddToFrontground(UiLayoutEditorCurve.Type type, int x1, int y1, int x2, int y2, int thickness, Color color)
        {
            UiLayoutEditorCurve curve = new UiLayoutEditorCurve(type, x1, y1, x2, y2);
            curve.SetStyle(thickness, color);

            AddToFrontground(curve);
        }

        public void AddToBackground(UiLayoutEditorCurve.Type type, int x1, int y1, int x2, int y2, int thickness, Color color)
        {
            UiLayoutEditorCurve curve = new UiLayoutEditorCurve(type, x1, y1, x2, y2);
            curve.SetStyle(thickness, color);

            AddToBackground(curve);
        }

        public void AddToFrontground(UiLayoutEditorCurve curve)
        {
            _curvesFrontground.Add(curve);
        }

        public void AddToBackground(UiLayoutEditorCurve curve)
        {
            _curvesBackground.Add(curve);
        }

        public void RenderBackground()
        {
            for (int i = 0; i < _curvesBackground.Count; i++)
            {
                _curvesBackground[i].Render();
            }

            _curvesBackground.Clear();
        }

        public void RenderFrontground()
        {
            for(int i = 0; i < _curvesFrontground.Count; i++)
            {
                _curvesFrontground[i].Render();
            }

            _curvesFrontground.Clear();
        }
    }
}