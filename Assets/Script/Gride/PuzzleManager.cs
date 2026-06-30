using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class PuzzleManager : MonoBehaviour
{
    public List<Node> nodes = new();

    private void Awake()
    {
        FindNodes();
    }
    private void Start()
    {
        UpdatePower();
        Node wire = GetNodeAt(new Vector2Int(1, 0));
        Node receiver = GetNodeAt(new Vector2Int(2, 0));
    }
    public void FindNodes()
    {
        nodes.Clear();

        nodes.AddRange(
            FindObjectsByType<Node>(
                FindObjectsSortMode.None
            )
        );

        Debug.Log($"Found {nodes.Count} nodes");
    }
    public Node GetNodeAt(Vector2Int position)
    {
        foreach (Node node in nodes)
        {
            if (node.nodePos == position)
            {
                return node;
            }
        }

        return null;
    }
    public bool AreConnected(Node nodeA, Node nodeB)
    {
        foreach (Direction dir in nodeA.connections)
        {
            Node neighbor = nodeA.GetNeighbor(dir);

            if (neighbor == nodeB)
            {
                Direction oppositeDir =
                    Node.GetOpposite(dir);

                return nodeB.HasDirection(oppositeDir);
            }
        }

        return false;
    }
    public Node GetSource()
    {
        foreach (Node node in nodes)
        {
            if (node.isSource)
            {
                return node;
            }
        }

        return null;
    }
    public void UpdatePower()
    {
        foreach (Node node in nodes)
        {
            node.currentcolor = SignalColor.None;
            if (node.isWire)
            {
                node.UpdateSprite();
            }
        }
        foreach (Node node in nodes)
        {
            if (!node.isSource)
            {
                continue;
            }

            HashSet<Node> visited = new();

            PowerNode(node, node.sourceColor, visited);
        }

        CheckWin();
    }
    void PowerNode(Node node, SignalColor color, HashSet<Node> visited)
    {
        if (visited.Contains(node))
            return;

        visited.Add(node);

        if (node.currentcolor == SignalColor.None)
        {
            node.currentcolor = color;
            if (node.isWire)
            {
                node.UpdateSprite();
            }
        }
        else if (node.currentcolor != color)
        {
            node.currentcolor = SignalColor.None;
            if (node.isWire)
            {
                node.UpdateSprite();
            }
            return;
        }
        else
        {
            return;
        }

        if (node.isConverter)
        {
            node.currentcolor = node.convertedColor;
            color = node.convertedColor;
            if (node.isWire)
            {
                node.UpdateSprite();
            }
        }

        if (node.isPortal &&
            node.linkedPortal != null &&
            node.linkedPortal.currentcolor == SignalColor.None)
        {
            PowerNode(node.linkedPortal, color, visited);
        }

        foreach (Direction dir in node.connections)
        {
            Node neighbor = node.GetNeighbor(dir);

            if (neighbor == null)
                continue;

            if (!AreConnected(node, neighbor))
                continue;

            PowerNode(neighbor,color, visited);
        }
    }
    public void CheckWin()
    {
        foreach (Node node in nodes)
        {
            if (!node.isReceiver)
                continue;

            if (node.currentcolor != node.receiverColor)
                return;
        }
        CanvasManager.instance.ShowWinScreen();
        Debug.Log("YOU WIN!");
        Time.timeScale = 0f;
    }
}
