using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class NodeRotatorV2 : MonoBehaviour
{
    private Camera mainCamera;
    private bool isRotating;

    private NodeV2 node;

    private void Awake()
    {
        node = GetComponent<NodeV2>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (isRotating)
            return;

        bool clicked = false;
        Vector2 worldPos = Vector2.zero;

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            clicked = true;
            worldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }

        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            clicked = true;
            worldPos = mainCamera.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue());
        }

        if (!clicked)
            return;

        Collider2D hit = Physics2D.OverlapPoint(worldPos);

        if (hit == null || hit.gameObject != gameObject)
            return;

        Rotate();
    }

    private void Rotate()
    {
        isRotating = true;

        transform.DORotate(
            transform.eulerAngles + new Vector3(0, 0, -90),
            0.22f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                RotateConnections();

                FindFirstObjectByType<PuzzleManagerV2>()
                    .RotateFinished();

                isRotating = false;
            });
    }

    private void RotateConnections()
    {
        for (int i = 0; i < node.Connections.Count; i++)
        {
            switch (node.Connections[i])
            {
                case Direction.Up:
                    node.Connections[i] = Direction.Right;
                    break;

                case Direction.Right:
                    node.Connections[i] = Direction.Down;
                    break;

                case Direction.Down:
                    node.Connections[i] = Direction.Left;
                    break;

                case Direction.Left:
                    node.Connections[i] = Direction.Up;
                    break;
            }
        }
    }
}