using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class PuzzleManager : MonoBehaviour
{
    public List<Node> nodes = new();

    private void Awake()
    {
        FindNode();
    }
    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            FindNode();
        }
    }
    public void FindNode()
    {
        nodes.AddRange(FindObjectsByType<Node>(FindObjectsSortMode.None));

        Debug.Log($"Found {nodes.Count} nodes");
    }
    //public Node GetNodeAt(Vector2Int position)
    //{
    //    foreach (Node node in nodes)
    //    {
    //        if (node.GridPosition == position)
    //            return node;
    //    }

    //    return null;
    //}


}
