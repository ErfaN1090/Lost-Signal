using UnityEngine;

public class SignalTask
{
    public NodeV2 From;
    public NodeV2 To;
    public SignalColor Color;

    public SignalTask(NodeV2 from, NodeV2 to, SignalColor color)
    {
        From = from;
        To = to;
        Color = color;
    }
}
