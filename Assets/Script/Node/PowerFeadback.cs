using UnityEngine;
using DG.Tweening;
public class PowerFeadback : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Node node;
    private SignalColor lastColor = SignalColor.None;

    [Header("Wire's Sprite")]
    public Sprite Off;
    public Sprite Blue;
    public Sprite Green;
    public Sprite Red;
    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        node = this.GetComponent<Node>();
    }
    public void Refresh()
    {
        if (node.isSource)
            return;
        if (!node.isWire)
            return;
        if (lastColor == node.currentcolor)
            return;
        if (lastColor == SignalColor.None && node.currentcolor != SignalColor.None)
        {
            changeSprite();
            transform.DOKill();

            transform.localScale = Vector3.one * 0.8f;

            transform.DOScale(1.12f, 0.20f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    transform.DOScale(1f, 0.17f)
                        .SetEase(Ease.InQuad);
                });
        }
        else if (lastColor != SignalColor.None && node.currentcolor == SignalColor.None)
        {
            changeSprite();
        }
        else
        {
            changeSprite();
        }
        lastColor = node.currentcolor;
    }
    private void changeSprite()
    {
        switch (node.currentcolor)
        {
            case SignalColor.None:
                spriteRenderer.sprite = Off;
                break;
            case SignalColor.Blue:
                spriteRenderer.sprite = Blue;
                break;
            case SignalColor.Red:
                spriteRenderer.sprite = Red;
                break;
            case SignalColor.Green:
                spriteRenderer.sprite = Green;
                break;
        }
    }
}
