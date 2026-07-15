using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
public class NodeRotator : MonoBehaviour
{
    private Camera mainCamera;
    private bool isRotating;
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
                //transform.DOKill();
                if (isRotating)
                    return;
                isRotating = true;
                transform.DORotate(transform.eulerAngles + new Vector3(0, 0, -90), 0.22f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    gameObject.GetComponent<Node>().RotateDirection();
                    FindFirstObjectByType<PuzzleManager>()
                   .UpdatePower();
                    isRotating = false;
                });

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
                //transform.DOKill();
                if (isRotating)
                    return;
                isRotating = true;
                transform.DORotate(transform.eulerAngles + new Vector3(0, 0, -90), 0.22f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    gameObject.GetComponent<Node>().RotateDirection();
                    FindFirstObjectByType<PuzzleManager>()
                   .UpdatePower();
                    isRotating = false;
                });
            }
        }
    }
}
