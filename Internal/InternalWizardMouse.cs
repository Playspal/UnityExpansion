using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExpansionInternal
{
    public class InternalWizardMouse
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        public void SetPosition(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}