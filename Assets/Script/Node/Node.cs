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
public enum SignalColor
{
    None,
    Blue,
    Red,
    Green
}

public class Node : MonoBehaviour
{
    private void Awake()
    {
        coreRenderer = GetComponent<SpriteRenderer>();
    }
    public List<Direction> connections;
    [Header("Position")]
    public Vector2Int nodePos;

    [Header("Colors")]
    public SignalColor currentcolor;
    public SignalColor sourceColor;
    public SignalColor receiverColor;
    public SignalColor convertedColor;


    [Header("Identity")]
    public bool isWire;
    public bool isSource;
    public bool isReceiver;
    public bool isPortal;
    public bool isConverter;

    public Node linkedPortal
        ;
    [Header("Sprites")]
    public SpriteRenderer coreRenderer;
    public Sprite offSprite;
    public Sprite blueSprite;
    public Sprite greenSprite;
    public Sprite redSprite;

    
    public void UpdateSprite()
    {
        //it'll be changed

        switch (currentcolor)
        {
            case SignalColor.Red:
                coreRenderer.sprite = redSprite;
                break;
            case SignalColor.None:
                coreRenderer.sprite = offSprite;
                break;
            case SignalColor.Green:
                coreRenderer.sprite = greenSprite;
                break;
            case SignalColor.Blue:
                coreRenderer.sprite = blueSprite;
                break;
        }  
    }
    private void Start()
    {
        nodePos = Vector2Int.RoundToInt(transform.position);

        if (isSource)
        {
            sourceColor = currentcolor;
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
