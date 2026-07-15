using UnityEngine;
using DG.Tweening;
public class PowerFeadback : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Node node;
    [SerializeField] private SignalColor lastColor = SignalColor.None;

    [Header("Wire's Sprite")]
    public Sprite Off;
    public Sprite Blue;
    public Sprite Green;
    public Sprite Red;

    [Header("Reciever's Sprite")]
    public Sprite OffBlue;
    public Sprite OffRed;
    public Sprite OffGreen;
    public Sprite OnBlue;
    public Sprite OnRed;
    public Sprite OnGreen;
    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        node = this.GetComponent<Node>();
    }
    public void Refresh()
    {
        //Debug.Log($"{gameObject.name}  Node={node.name}  Color={node.currentcolor}");
        if (node.isSource)
            return;
        if (lastColor == node.currentcolor)
            return;
        if (node.isWire)
            StartWireAnimation();
        else if (node.isReceiver)
        {
            StartRecieverAnimation();
        }

    }
    private void changeWireSprite()
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
    private void StartWireAnimation()
    {
        if (lastColor == SignalColor.None && node.currentcolor != SignalColor.None)
        {
            changeWireSprite();
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
            changeWireSprite();
        }
        else
        {
            changeWireSprite();
        }
        lastColor = node.currentcolor;
    }
    private void ChangeReceiverSprite()
    {
        bool powered = node.currentcolor == node.receiverColor;

        switch (node.receiverColor)
        {
            case SignalColor.Blue:
                spriteRenderer.sprite = powered ? OnBlue : OffBlue;
                break;

            case SignalColor.Red:
                spriteRenderer.sprite = powered ? OnRed : OffRed;
                break;

            case SignalColor.Green:
                spriteRenderer.sprite = powered ? OnGreen : OffGreen;
                break;
        }
    }
    private void StartRecieverAnimation()
    {
        ChangeReceiverSprite();

        bool turnedOn =
            lastColor != node.receiverColor &&
            node.currentcolor == node.receiverColor;

        if (turnedOn)
        {
            transform.DOKill();

            transform.localScale = Vector3.one * 0.85f;

            transform.DOScale(1.12f, 0.22f)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    transform.DOScale(1f, 0.18f)
                        .SetEase(Ease.OutQuad);
                });

            // TODO
            // Play Sound
            // Play Particle
            // Fade Tile
        }

        lastColor = node.currentcolor;
    }
    public void Initialize()
    {
        if (node.isSource && node.isWire)
            return;
        if (node.isReceiver)
        {
            ChangeReceiverSprite();
        }

        lastColor = node.currentcolor;
    }
}
