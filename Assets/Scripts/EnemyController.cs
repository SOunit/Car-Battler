using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;

    private void Awake()
    {
        instance = this;
    }

    public List<CardScriptableObject>
        deckToUse = new List<CardScriptableObject>();

    private List<CardScriptableObject>
        activeCards = new List<CardScriptableObject>();

    public Card cardToSpawn;

    public Transform cardSpawnPoint;

    public enum AIType
    {
        placeFromDeck,
        handRandomPlace,
        handDefensive,
        handAttacking
    }

    public AIType enemyAIType;

    private List<CardScriptableObject>
        cardsInHand = new List<CardScriptableObject>();

    public int startHandSize;

    // Start is called before the first frame update
    void Start()
    {
        SetupDeck();

        if (enemyAIType != AIType.placeFromDeck)
        {
            SetupHand();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetupDeck()
    {
        activeCards.Clear();

        List<CardScriptableObject> tempDeck = new List<CardScriptableObject>();
        tempDeck.AddRange (deckToUse);

        int iteration = 0;
        while (tempDeck.Count > 0 && iteration < 500)
        {
            int selected = Random.Range(0, tempDeck.Count);
            activeCards.Add(tempDeck[selected]);
            tempDeck.RemoveAt (selected);

            iteration++;
        }
    }

    public void StartAction()
    {
        StartCoroutine(EnemyActionCo());
    }

    IEnumerator EnemyActionCo()
    {
        if (activeCards.Count == 0)
        {
            SetupDeck();
        }

        yield return new WaitForSeconds(.5f);

        List<CardPlacePoint> cardPoints = new List<CardPlacePoint>();
        cardPoints.AddRange(CardPointsController.instance.enemyCardPoints);

        int randomPoint = Random.Range(0, cardPoints.Count);
        CardPlacePoint selectedPoint = cardPoints[randomPoint];

        if (
            enemyAIType == AIType.placeFromDeck ||
            enemyAIType == AIType.handRandomPlace
        )
        {
            cardPoints.Remove (selectedPoint);

            while (selectedPoint.activeCard != null && cardPoints.Count > 0)
            {
                randomPoint = Random.Range(0, cardPoints.Count);
                selectedPoint = cardPoints[randomPoint];
                cardPoints.RemoveAt (randomPoint);
            }
        }

        CardScriptableObject selectedCard = null;
        int iterations = 0;

        switch (enemyAIType)
        {
            case AIType.placeFromDeck:
                if (selectedPoint.activeCard == null)
                {
                    Card newCard =
                        Instantiate(cardToSpawn,
                        cardSpawnPoint.position,
                        cardSpawnPoint.rotation);

                    newCard.cardSO = activeCards[0];
                    activeCards.RemoveAt(0);
                    newCard.SetupCard();
                    newCard
                        .MoveToPoint(selectedPoint.transform.position,
                        selectedPoint.transform.rotation);

                    selectedPoint.activeCard = newCard;
                    newCard.assignedPlace = selectedPoint;
                }
                break;
            case AIType.handRandomPlace:
                selectedCard = SelectedCardToPlay();

                if (selectedCard != null)
                {
                    PlayCard (selectedCard, selectedPoint);
                }

                break;
            case AIType.handDefensive:
                break;
            case AIType.handAttacking:
                break;
        }

        yield return new WaitForSeconds(.5f);

        BattleController.instance.AdvanceTurn();
    }

    void SetupHand()
    {
        for (int i = 0; i < startHandSize; i++)
        {
            if (activeCards.Count == 0)
            {
                SetupDeck();
            }

            cardsInHand.Add(activeCards[0]);
            activeCards.RemoveAt(0);
        }
    }

    public void PlayCard(CardScriptableObject cardSO, CardPlacePoint placePoint)
    {
        Card newCard =
            Instantiate(cardToSpawn,
            cardSpawnPoint.position,
            cardSpawnPoint.rotation);

        newCard.cardSO = cardSO;

        newCard.SetupCard();
        newCard
            .MoveToPoint(placePoint.transform.position,
            placePoint.transform.rotation);

        placePoint.activeCard = newCard;
        newCard.assignedPlace = placePoint;

        cardsInHand.Remove (cardSO);

        BattleController.instance.SpendEnemyMana(cardSO.manaCost);
    }

    CardScriptableObject SelectedCardToPlay()
    {
        CardScriptableObject cardToPlay = null;

        List<CardScriptableObject> cardsToPlay =
            new List<CardScriptableObject>();

        foreach (var card in cardsInHand)
        {
            if (card.manaCost <= BattleController.instance.enemyMana)
            {
                cardsToPlay.Add (card);
            }
        }

        if (cardsToPlay.Count > 0)
        {
            int selected = Random.Range(0, cardsToPlay.Count);

            cardToPlay = cardsToPlay[selected];
        }

        return cardToPlay;
    }
}
