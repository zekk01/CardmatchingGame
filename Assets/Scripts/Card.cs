using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class Card : MonoBehaviour
{
    [SerializeField] private Sprite front;
    [SerializeField] private Sprite back;

    private bool isFlipped;
    private bool isMatched;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isFlipped = false;
        isMatched = false;
    }

    private void Start()
    {
        UpdateCardAppearance();
    }

    private void OnMouseDown()
    {
        if (CanFlip() && GameController.Instance.CanFlipCards())
        {
            Flip();
            GameController.Instance.HandleCardFlip(this);
        }
    }

    public void Flip()
    {
        isFlipped = !isFlipped;
        UpdateCardAppearance();
    }

    public void MarkAsMatched()
    {
        isMatched = true;
    }

    public Sprite GetFrontSprite()
    {
        return front;
    }

    public bool CanFlip()
    {
        return !isFlipped && !isMatched;
    }

    private void UpdateCardAppearance()
    {
        spriteRenderer.sprite = isFlipped ? front : back;
    }

    public void SetFrontSprite(Sprite frontSprite)
    {
        front = frontSprite;
        // If the card is already flipped, update the sprite renderer immediately
        if (isFlipped)
        {
            spriteRenderer.sprite = front;
        }
    }

    public bool IsMatched()
    {
        return isMatched;
    }
}
