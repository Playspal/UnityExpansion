using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityExpansionInternal
{
    public class InternalWizard : ScriptableWizard
    {
        public int WindowWidth { get { return (int)(position.width); } }
        public int WindowHeight { get { return (int)(position.height); } }

        public InternalWizardMouse Mouse { get; private set; }

        protected virtual void OnWizardCreate()
        {
            Debug.LogError("!!!");
            Mouse = new InternalWizardMouse();
            
        }

        protected virtual void OnGUI()
        {
            if (Event.current.type == EventType.Repaint)
            {
                //Mouse.SetPosition(Event.current.mousePosition.x, Event.current.mousePosition.y);
            }
        }
    }
}