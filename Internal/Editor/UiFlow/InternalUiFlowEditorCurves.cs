using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExpansionInternal.UiFlow
{
    public class InternalUiFlowEditorCurves
    {
        private List<InternalUiFlowEditorCurve> _curvesBackground = new List<InternalUiFlowEditorCurve>();
        private List<InternalUiFlowEditorCurve> _curvesFrontground = new List<InternalUiFlowEditorCurve>();

        public void AddToFrontground(InternalUiFlowEditorCurve curve)
        {
            _curvesFrontground.Add(curve);
        }

        public void RenderBackground()
        {

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