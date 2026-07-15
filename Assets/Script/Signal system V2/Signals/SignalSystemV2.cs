using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SignalSystemV2 : MonoBehaviour
{
    public static SignalSystemV2 Instance;

    [SerializeField] private float propagationDelay = 0.05f;

    private List<NodeV2> nodes = new();
    private SignalQueue signalQueue = new();

    private Coroutine propagationCoroutine;

    private void Awake()
    {
        Instance = this;
    }
    private NodeV2 GetNodeAt(Vector2Int position)
    {
        foreach (var node in nodes)
        {
            if (node.GridPosition == position)
                return node;
        }

        return null;
    }
    private List<NodeV2> GetSources()
    {
        List<NodeV2> sources = new();

        foreach (var node in nodes)
        {
            if (node.IsSource)
                sources.Add(node);
        }

        return sources;
    }
    public void Initialize(List<NodeV2> levelNodes)
    {
        nodes = levelNodes;

        BuildGraph();
    }

    public void Recalculate()
    {
        //Debug.Log("Recalculate");
        if (propagationCoroutine != null)
            StopCoroutine(propagationCoroutine);

        propagationCoroutine = StartCoroutine(PropagationRoutine());
    }

    private IEnumerator PropagationRoutine()
    {
        //Debug.Log("Propagation Started");
        ResetNodes();

        StartPropagation();

        while (signalQueue.HasTask())
        {
            ProcessNextTask();

            yield return new WaitForSeconds(propagationDelay);
        }

        CheckWin();
    }

    private void BuildGraph()
    {
        //foreach (var node in nodes)
        //{
        //    //Debug.Log($"===== {node.name} =====");
        //    node.Neighbors.Clear();
        //    node.ConnectedNodes.Clear();

        //    foreach (Direction direction in node.Connections)
        //    {
        //        Vector2Int target = node.GridPosition;

        //        switch (direction)
        //        {
        //            case Direction.Up:
        //                target += Vector2Int.up;
        //                break;

        //            case Direction.Right:
        //                target += Vector2Int.right;
        //                break;

        //            case Direction.Down:
        //                target += Vector2Int.down;
        //                break;

        //            case Direction.Left:
        //                target += Vector2Int.left;
        //                break;
        //        }

        //        NodeV2 neighbor = GetNodeAt(target);
        //        Debug.Log($"{node.name} checking {direction} -> {target} | Found = {(neighbor != null ? neighbor.name : "NULL")}");
        //        if (neighbor == null)
        //            continue;

        //        node.AddNeighbor(direction, neighbor);
        //    }
        //    Debug.Log($"{node.name} Neighbors = {node.Neighbors.Count}");
        //}
        Direction[] dirs =
        {
            Direction.Up,
            Direction.Right,
            Direction.Down,
            Direction.Left
        };

        foreach (var node in nodes)
        {
            node.Neighbors.Clear();

            foreach (Direction direction in dirs)
            {
                Vector2Int target = node.GridPosition;

                switch (direction)
                {
                    case Direction.Up:
                        target += Vector2Int.up;
                        break;

                    case Direction.Right:
                        target += Vector2Int.right;
                        break;

                    case Direction.Down:
                        target += Vector2Int.down;
                        break;

                    case Direction.Left:
                        target += Vector2Int.left;
                        break;
                }

                NodeV2 neighbor = GetNodeAt(target);
                //Debug.Log($"{node.name} checking {direction} -> {target} | Found = {(neighbor != null ? neighbor.name : "NULL")}");
                if (neighbor == null)
                    continue;

                node.AddNeighbor(direction, neighbor);
            }
            Debug.Log($"{node.name} Neighbors = {node.Neighbors.Count}");
        }
    }

    private void StartPropagation()
    {
        //Debug.Log("Queue Ready");
        List<NodeV2> sources = GetSources();


        foreach (var source in sources)
        {
            source.SetColor(source.SourceColor);
            source.IsPowered = true;

            //foreach (var neighbor in source.ConnectedNodes)
            //{
            //    signalQueue.Enqueue(
            //        new SignalTask(source, neighbor, source.SourceColor));

            foreach (var pair in source.Neighbors)
            {
                Direction dir = pair.Key;
                NodeV2 neighbor = pair.Value;

                if (!source.HasConnection(dir))
                    continue;

                if (!neighbor.HasConnection(NodeV2.GetOpposite(dir)))
                    continue;

                signalQueue.Enqueue(
                    new SignalTask(source, neighbor, source.SourceColor));
            }
            //}
            //Debug.Log($"{source.name} Connected Count = {source.ConnectedNodes.Count}");
        }

    }

    private void ProcessNextTask()
    {
        SignalTask task = signalQueue.Dequeue();

        NodeV2 node = task.To;
        //Debug.Log($"Processing {task.To.name}");
        if (node == null)
            return;

        if (node.CurrentColor == SignalColor.None)
        {
            node.SetColor(task.Color);
            node.IsPowered = true;
            //Debug.Log($"{node.name} -> {node.CurrentColor}");
        }
        else if (node.CurrentColor != task.Color)
        {
            node.ResetState();
            return;
        }
        else
        {
            return;
        }

        SignalColor outputColor = task.Color;

        if (node.IsConverter)
        {
            outputColor = node.ConvertedColor;
            node.SetColor(outputColor);
        }

        if (node.IsPortal && node.LinkedPortal != null)
        {
            signalQueue.Enqueue(
                new SignalTask(node, node.LinkedPortal, outputColor));
        }
        Debug.Log($"{node.name} Neighbors = {node.Neighbors.Count}");
        //foreach (var next in node.ConnectedNodes)
        //{
        //    if (next == task.From)
        //        continue;

        //    signalQueue.Enqueue(
        //        new SignalTask(node, next, outputColor));
        //}
        foreach (var pair in node.Neighbors)
        {
            Direction dir = pair.Key;
            NodeV2 next = pair.Value;

            if (next == task.From)
                continue;

            if (!node.HasConnection(dir))
                continue;

            if (!next.HasConnection(NodeV2.GetOpposite(dir)))
                continue;

            signalQueue.Enqueue(
                new SignalTask(node, next, outputColor));
        }
    }

    private void ResetNodes()
    {
        signalQueue.Clear();

        foreach (var node in nodes)
            node.ResetState();
    }

    private void CheckWin()
    {

    }
}
