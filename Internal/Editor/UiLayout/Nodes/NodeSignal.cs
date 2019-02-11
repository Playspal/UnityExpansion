using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeSignal : Node
    {
        public readonly NodeConnectorInput InputDispatch;
        public readonly NodeConnectorOutput OutputOnReceive;

        public readonly NodeBlockHeader BlockHeader;

        public string Signal { get; private set; }

        public NodeSignal(EditorLayout layout) : base(layout, 200, 70)
        {
            BlockHeader = new NodeBlockHeader(layout, this);
            BlockHeader.SetParent(this);

            InputDispatch = new NodeConnectorInput(layout, this, "Dispatch");
            InputDispatch.SetParent(this);
            InputDispatch.Y = 42;

            OutputOnReceive = new NodeConnectorOutput(layout, this, "OnReceive");
            OutputOnReceive.SetParent(this);
            OutputOnReceive.Y = 42;
        }

        public void SetSignal(string signal)
        {
            Signal = signal;
            BlockHeader.SetTitle(InternalSignalsFile.GetSignalName(Signal));
            OutputOnReceive.SetData(signal);
        }
    }
}