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

        Debug.Log(AreConnected(wire, receiver));
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
            node.isPowered = false;
        }

        Node source = GetSource();

        if (source != null)
        {
            PowerNode(source);
        }

        CheckWin();
    }
    void PowerNode(Node node)
    {
        node.isPowered = true;

        foreach (Direction dir in node.connections)
        {
            Node neighbor = node.GetNeighbor(dir);

            if (neighbor == null)
                continue;

            if (neighbor.isPowered)
                continue;

            if (!AreConnected(node, neighbor))
                continue;

            PowerNode(neighbor);
        }
    }
    public void CheckWin()
    {
        foreach (Node node in nodes)
        {
            if (node.isReceiver && node.isPowered)
            {
                Debug.Log("YOU WIN!");
            }
        }
    }
}
