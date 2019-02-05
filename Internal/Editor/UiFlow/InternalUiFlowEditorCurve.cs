using UnityEditor;
using UnityEngine;

namespace UnityExpansionInternal.UiFlow
{
    public class InternalUiFlowEditorCurve
    {
        public readonly int XFrom;
        public readonly int XTo;

        public readonly int YFrom;
        public readonly int YTo;

        private Color _color = Color.red;
        private int _thickness = 1;

        public InternalUiFlowEditorCurve(int x1, int y1, int x2, int y2)
        {
            if(x1 < x2)
            {
                XFrom = x1;
                YFrom = y1;

                XTo = x2;
                YTo = y2;
            }
            else
            {
                XFrom = x2;
                YFrom = y2;

                XTo = x1;
                YTo = y1;
            }
        }

        public void SetStyle(int thickness, string color)
        {
            ColorUtility.TryParseHtmlString(color, out _color);
            _thickness = thickness;
        }

        public void SetStyle(int thickness, Color color)
        {
            _thickness = thickness;
            _color = color;
        }

        public void Render()
        {
            Vector3 startPos = new Vector3(XFrom, YFrom, 0);
            Vector3 endPos = new Vector3(XTo, YTo, 0);

            float distance = Vector3.Distance(startPos, endPos);
            float distanceMax = Mathf.Abs(endPos.x - startPos.x);

            distance /= 3f;

            if(distance > distanceMax)
            {
                //distance = distanceMax;
            }


            Vector3 startTan = startPos + Vector3.right * distance;
            Vector3 endTan = endPos + Vector3.left * distance;

            Handles.DrawBezier(startPos, endPos, startTan, endTan, _color, null, _thickness);
        }
    }
}