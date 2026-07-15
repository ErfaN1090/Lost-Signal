using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private LevelData currentLevel;
    [SerializeField] private PrefabDataBase prefabDatabase;
    [SerializeField] private GrideManager grideManager;
    private GameObject GetPrefab(LevelObjectType type)
    {
        switch (type)
        {
            case LevelObjectType.Empty:
                return null;
            case LevelObjectType.Source:
                return prefabDatabase.sourcePrefab;
            case LevelObjectType.Receiver:
                return prefabDatabase.receiverPrefab;
            case LevelObjectType.Straight:
                return prefabDatabase.StraightPrefab;
            case LevelObjectType.Corner:
                return prefabDatabase.cornerPrefab;
            case LevelObjectType.ThreeWay:
                return prefabDatabase.threeWayPrefab;
            case LevelObjectType.FourWay:
                return prefabDatabase.fourWayPrefab;
            case LevelObjectType.Portal:
                return prefabDatabase.portalPrefab;
            case LevelObjectType.Converter:
                return prefabDatabase.converterPrefab;
            default:
                return null;
        }
    }
    private void LinkPortals() 
    {
        foreach (Node portal in nodes)
        {
            if (!portal.isPortal)
                continue;

            foreach (Node otherPortal in nodes)
            {
                if (portal == otherPortal)
                    continue;
                if (!otherPortal.isPortal)
                    continue;
                if (portal.portalID == otherPortal.portalID)
                {
                    portal.linkedPortal = otherPortal;
                    break;
                }   
            }
        }
    }

    private void SpawnLevel()
    {
        foreach(LevelObjectData data in currentLevel.objects)
        {
            GameObject prefab = GetPrefab(data.objectType);
            if (prefab == null)
                continue;

            GameObject obj = Instantiate(prefab, 
                new Vector3(data.position.x, data.position.y, 0), 
                Quaternion.Euler(0, 0, -90 * data.rotation));
            Node node = obj.GetComponent<Node>();
            if (node != null)
            {
                for (int i = 0; i < data.rotation; i++)
                {
                    node.RotateDirection();
                }
            }
            node.Initialize(data);
        }
    }
    public List<Node> nodes = new();

    private void Start()
    {
        grideManager.initialize(currentLevel.width, currentLevel.hight);
        SpawnLevel();

        FindNodes();
        foreach (Node node in nodes)
        {
            PowerFeadback feedback = node.GetComponent<PowerFeadback>();

            if (feedback != null)
                feedback.Initialize();
        }
        LinkPortals();

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
        Debug.Log("====== UpdatePower ======");
        foreach (Node node in nodes)
        {
            node.SetColor(SignalColor.None);

            if (!node.isSource)
            {
                continue;
            }

            HashSet<Node> visited = new();

            PowerNode(node, node.sourceColor, visited);
        }
        foreach (Node node in nodes)
        {
            Debug.Log($"{node.name} : {node.currentcolor}");
        }
        foreach (Node node in nodes)
        {
            PowerFeadback feedback = node.GetComponent<PowerFeadback>();

            if (feedback != null)
            {
            
                feedback.Refresh();
            }
        }
        CheckWin();
    }
    void PowerNode(Node node, SignalColor color, HashSet<Node> visited)
    {
        if (visited.Contains(node))
            return;

        visited.Add(node);
        Debug.Log($"{node.name} -> current color: {node.currentcolor} color: {color}");
        if (node.currentcolor == SignalColor.None)
        {
            node.SetColor(color);
        }
        else if (node.currentcolor != color)
        {
            node.SetColor(SignalColor.None);
            return;
        }
        else
        {
            return;
        }

        if (node.isConverter)
        {
            node.SetColor(node.convertedColor);
            color = node.convertedColor;
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
