using UnityEngine;
using UnityEngine.InputSystem;
public class NodeRotator : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // mouse/pc
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos =
                mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            Collider2D hit =
                Physics2D.OverlapPoint(mousePos);

            if (hit != null && hit.gameObject == gameObject)
            {
                transform.Rotate(0, 0, -90);
                gameObject.GetComponent<Node>().RotateDirection();
                FindFirstObjectByType<PuzzleManager>()
               .UpdatePower();
            }
        }

        // touch/android
        if (Touchscreen.current != null &&
    Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(
                Touchscreen.current.primaryTouch.position.ReadValue());

            Collider2D hit = Physics2D.OverlapPoint(touchPos);

            if (hit != null && hit.gameObject == gameObject)
            {
                transform.Rotate(0, 0, -90);

                GetComponent<Node>().RotateDirection();

                FindFirstObjectByType<PuzzleManager>()
                    .UpdatePower();
            }
        }
    }
}
