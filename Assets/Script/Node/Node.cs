using UnityEngine;
using System.Collections.Generic;

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
}
