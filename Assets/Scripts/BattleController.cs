using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController instance;

    private void Awake()
    {
        instance = this;
    }

    public int

            startingMana = 4,
            maxMana = 12;

    public int playerMana;

    public int startingCardAmount = 5;

    public enum TurnOrder
    {
        playerActive,
        playerCardAttacks,
        enemyActive,
        enemyCardAttacks
    }

    public TurnOrder currentPhase;

    // Start is called before the first frame update
    void Start()
    {
        FillPlayerMana();

        DeckController.instance.DrawMultipleCards (startingCardAmount);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            AdvanceTurn();
        }
    }

    public void SpendPlayerMana(int amountToSpend)
    {
        playerMana = playerMana - amountToSpend;

        if (playerMana < 0)
        {
            playerMana = 0;
        }

        UIController.instance.SetPlayerManaText (playerMana);
    }

    public void FillPlayerMana()
    {
        playerMana = startingMana;
        UIController.instance.SetPlayerManaText (playerMana);
    }

    public void AdvanceTurn()
    {
        currentPhase++;

        if (
            (int) currentPhase >=
            System.Enum.GetValues(typeof (TurnOrder)).Length
        )
        {
            currentPhase = 0;
        }

        switch (currentPhase)
        {
            case TurnOrder.playerActive:
                UIController.instance.endTurnButton.SetActive(true);
                UIController.instance.drawCardButton.SetActive(true);

                FillPlayerMana();

                break;
            case TurnOrder.playerCardAttacks:
                Debug.Log("skipping player card attacks");

                AdvanceTurn();
                break;
            case TurnOrder.enemyActive:
                Debug.Log("skipping enemy action");
                AdvanceTurn();
                break;
            case TurnOrder.enemyCardAttacks:
                Debug.Log("skipping enemy card attacks");
                AdvanceTurn();
                break;
            default:
                break;
        }
    }

    public void EndPlayerTurn()
    {
        UIController.instance.endTurnButton.SetActive(false);
        UIController.instance.drawCardButton.SetActive(false);
        AdvanceTurn();
    }
}
