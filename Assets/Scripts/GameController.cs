using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public enum CardLayout
{
    TwoByTwo,
    TwoByThree,
    ThreeByFour,
    FourByFive,
    FiveBySix
}

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Sprite[] cardFronts;
    [SerializeField] private GameObject cardParent;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip flipSound;
    [SerializeField] private AudioClip matchSound;
    [SerializeField] private AudioClip mismatchSound;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private Text scoreText;
    [SerializeField] private CardLayout selectedLayout;

    private List<Card> cards;
    private Card firstCard;
    private Card secondCard;
    private int score;
    private bool canFlipCards = true;
    private Queue<GameObject> cardPool;

    private Dictionary<CardLayout, (int rows, int columns)> layoutMapping = new Dictionary<CardLayout, (int, int)>
    {
        { CardLayout.TwoByTwo, (2, 2) },
        { CardLayout.TwoByThree, (2, 3) },
        { CardLayout.ThreeByFour, (3, 4) },
        { CardLayout.FourByFive, (4, 5) },
        { CardLayout.FiveBySix, (5, 6) }
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        cards = new List<Card>();
        cardPool = new Queue<GameObject>();
        InitializeCardsByLayout();
        //score = SaveLoadSystem.LoadProgress(); //I dont know if we need the score to be presistent 
        UpdateScoreDisplay();
    }

    private void InitializeCardsByLayout()
    {
        if (!layoutMapping.TryGetValue(selectedLayout, out var layout))
        {
            Debug.LogError("Selected layout is not valid.");
            return;
        }

        ResetScore();
        InitializeCards(layout.rows, layout.columns);
    }

    public void HandleCardFlip(Card card)
    {
        if (!canFlipCards || card == firstCard || card == secondCard || card.IsMatched())
        {
            return;
        }

        PlaySound(flipSound);

        if (firstCard == null)
        {
            firstCard = card;
        }
        else if (secondCard == null)
        {
            secondCard = card;
            canFlipCards = false; // Disable further card flips
            StartCoroutine(EvaluateCardMatch());
        }
    }

    private IEnumerator EvaluateCardMatch()
    {
        yield return new WaitForSeconds(1);

        if (firstCard.GetFrontSprite() == secondCard.GetFrontSprite())
        {
            HandleMatch();
        }
        else
        {
            HandleMismatch();
        }

        firstCard = null;
        secondCard = null;

        canFlipCards = true; // Re-enable card flips after processing
    }

    private void HandleMatch()
    {
        firstCard.MarkAsMatched();
        secondCard.MarkAsMatched();
        score += 10;
        SaveProgress();
        PlaySound(matchSound);
        UpdateScoreDisplay();
    }

    private void HandleMismatch()
    {
        firstCard.Flip();
        secondCard.Flip();
        PlaySound(mismatchSound);
    }

    private void InitializeCards(int rows, int columns)
    {
        ClearExistingCards();

        int totalCards = rows * columns;
        List<Sprite> selectedFronts = new List<Sprite>();

        // Ensure there are enough card fronts to create pairs
        for (int i = 0; i < totalCards / 2; i++)
        {
            selectedFronts.Add(cardFronts[i % cardFronts.Length]);
            selectedFronts.Add(cardFronts[i % cardFronts.Length]);
        }

        selectedFronts = selectedFronts.OrderBy(x => Random.value).ToList();

        float cardWidth = cardPrefab.GetComponent<RectTransform>().rect.width;
        float cardHeight = cardPrefab.GetComponent<RectTransform>().rect.height;

        // Calculate start positions based on parent position
        float startX = -(columns - 1) * cardWidth * 0.5f;
        float startY = (rows - 1) * cardHeight * 0.5f;

        for (int i = 0; i < totalCards; i++)
        {
            int row = i / columns;
            int column = i % columns;

            GameObject cardObj = GetCardFromPool();
            Card card = cardObj.GetComponent<Card>();
            card.SetFrontSprite(selectedFronts[i]);
            cards.Add(card);

            cardObj.transform.localPosition = new Vector3(startX + column * cardWidth, startY - row * cardHeight, 0);

            // Debug logs to check positioning
            Debug.Log($"Card {i} Position: {cardObj.transform.localPosition}");
        }
    }

    private void ClearExistingCards()
    {
        foreach (Transform child in cardParent.transform)
        {
            cardPool.Enqueue(child.gameObject);
            child.gameObject.SetActive(false);
        }
        cards.Clear();
    }

    private GameObject GetCardFromPool()
    {
        if (cardPool.Count > 0)
        {
            GameObject card = cardPool.Dequeue();
            card.SetActive(true);
            return card;
        }
        return Instantiate(cardPrefab, cardParent.transform);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void UpdateScoreDisplay()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    private void ResetScore()
    {
        score = 0;
        UpdateScoreDisplay();
    }

    private void SaveProgress()
    {
        SaveLoadSystem.SaveProgress(score);
    }

    public bool CanFlipCards()
    {
        return canFlipCards;
    }
}
