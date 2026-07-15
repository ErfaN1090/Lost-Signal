using UnityEngine;
using System.Collections.Generic;
public enum Direction
{
    Up,
    Right,
    Down,
    Left
}
public enum SignalColor
{
    None,
    Blue,
    Red,
    Green
}
public class NodeV2 : MonoBehaviour
{
    [HideInInspector]
    public bool IsPowered;

    [HideInInspector]
    public bool IsProcessing;
    [Header("Identity")]
    public NodeType nodeType;

    public bool IsSource => nodeType == NodeType.Source;
    public bool IsReceiver => nodeType == NodeType.Receiver;
    public bool IsWire => nodeType == NodeType.Wire;
    public bool IsPortal => nodeType == NodeType.Portal;
    public bool IsConverter => nodeType == NodeType.Converter;

    [Header("Grid")]
    public Vector2Int GridPosition;

    [Header("Connections")]
    public List<Direction> Connections = new();

    [Header("Colors")]
    public SignalColor CurrentColor = SignalColor.None;
    public SignalColor SourceColor = SignalColor.None;
    public SignalColor ReceiverColor = SignalColor.None;
    public SignalColor ConvertedColor = SignalColor.None;

    [Header("Portal")]
    public int PortalID;
    public NodeV2 LinkedPortal;

    [HideInInspector]
    public Dictionary<Direction, NodeV2> Neighbors = new();

    [HideInInspector]
    public List<NodeV2> ConnectedNodes = new();
    public void ResetState()
    {
        CurrentColor = SignalColor.None;
        IsPowered = false;
        IsProcessing = false;
    }
    public void SetColor(SignalColor color)
    {
        CurrentColor = color;
    }

    public void ClearColor()
    {
        CurrentColor = SignalColor.None;
    }
    public bool HasConnection(Direction direction)
    {
        return Connections.Contains(direction);
    }

    public static Direction GetOpposite(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up: return Direction.Down;
            case Direction.Right: return Direction.Left;
            case Direction.Down: return Direction.Up;
            case Direction.Left: return Direction.Right;
        }

        return Direction.Up;
    }
    public void AddNeighbor(Direction direction, NodeV2 node)
    {
        if (Neighbors.ContainsKey(direction))
            Neighbors[direction] = node;
        else
            Neighbors.Add(direction, node);

        if (node != null && !ConnectedNodes.Contains(node))
            ConnectedNodes.Add(node);
    }
}
