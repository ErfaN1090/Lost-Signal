using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public enum Direction
{
    Up,
    Right,
    Down,
    Left
}
public class Node : MonoBehaviour
{
    public List<Direction> connections;

    public Vector2Int nodePos;

    public bool isSource;
    public bool isReceiver;
    public bool isPortal;
    public Node linkedPortal;

    public bool isPowered;
    private void Start()
    {
        nodePos = Vector2Int.RoundToInt(transform.position);
    }
    private void Update()
    {
        if (isPowered)
        {
            Debug.Log(name + " Powered");
        }
    }
    public void RotateDirection()
    {
        for (int i = 0; i < connections.Count; i++)
        {
            switch (connections[i]) 
            {
                case Direction.Up:
                    connections[i] = Direction.Right;
                    break;
                case Direction.Right:
                    connections[i] = Direction.Down;
                    break;
                case Direction.Down:
                    connections[i] = Direction.Left;
                    break;
                case Direction.Left:
                    connections[i] = Direction.Up;
                    break;
            }

        }
    }
    public Node GetNeighbor(Direction direction)
    {
        Vector2Int targetPos = nodePos;

        switch (direction)
        {
            case Direction.Up:
                targetPos += Vector2Int.up;
                break;

            case Direction.Right:
                targetPos += Vector2Int.right;
                break;

            case Direction.Down:
                targetPos += Vector2Int.down;
                break;

            case Direction.Left:
                targetPos += Vector2Int.left;
                break;
        }

        PuzzleManager puzzleManager =
            FindFirstObjectByType<PuzzleManager>();

        return puzzleManager.GetNodeAt(targetPos);
    }
    public bool HasDirection(Direction direction)
    {
        return connections.Contains(direction);
    }
    public static Direction GetOpposite(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return Direction.Down;

            case Direction.Right:
                return Direction.Left;

            case Direction.Down:
                return Direction.Up;

            case Direction.Left:
                return Direction.Right;
        }

        return Direction.Up;
    }
}
