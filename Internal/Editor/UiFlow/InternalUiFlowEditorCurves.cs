using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExpansionInternal.UiFlow
{
    public class InternalUiFlowEditorCurves
    {
        private List<InternalUiFlowEditorCurve> _curvesBackground = new List<InternalUiFlowEditorCurve>();
        private List<InternalUiFlowEditorCurve> _curvesFrontground = new List<InternalUiFlowEditorCurve>();

        public void AddToFrontground(InternalUiFlowEditorCurve.Type type, int x1, int y1, int x2, int y2, int thickness, Color color)
        {
            InternalUiFlowEditorCurve curve = new InternalUiFlowEditorCurve(type, x1, y1, x2, y2);
            curve.SetStyle(thickness, color);

            AddToFrontground(curve);
        }

        public void AddToBackground(InternalUiFlowEditorCurve.Type type, int x1, int y1, int x2, int y2, int thickness, Color color)
        {
            InternalUiFlowEditorCurve curve = new InternalUiFlowEditorCurve(type, x1, y1, x2, y2);
            curve.SetStyle(thickness, color);

            AddToBackground(curve);
        }

        public void AddToFrontground(InternalUiFlowEditorCurve curve)
        {
            _curvesFrontground.Add(curve);
        }

        public void AddToBackground(InternalUiFlowEditorCurve curve)
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