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

    public int

            playerMana,
            enemyMana;

    private int

            currentPlayerMaxMana,
            currentEnemyMaxMana;

    public int startingCardAmount = 5;

    public int cardsToDrawPerTurn = 1;

    public int playerHealth;

    public int enemyHealth;

    public enum TurnOrder
    {
        playerActive,
        playerCardAttacks,
        enemyActive,
        enemyCardAttacks
    }

    public TurnOrder currentPhase;

    public Transform discardPoint;

    public bool battleEnded;

    public float resultScreenDelayTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayerMaxMana = startingMana;
        currentEnemyMaxMana = startingMana;
        FillPlayerMana();
        FillEnemyMana();

        DeckController.instance.DrawMultipleCards (startingCardAmount);

        UIController.instance.SetPlayerHealthText (playerHealth);
        UIController.instance.SetEnemyHealthText (enemyHealth);
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
        playerMana = currentPlayerMaxMana;
        UIController.instance.SetPlayerManaText (playerMana);
    }

    public void SpendEnemyMana(int amountToSpend)
    {
        enemyMana = enemyMana - amountToSpend;

        if (enemyMana < 0)
        {
            enemyMana = 0;
        }

        UIController.instance.SetEnemyManaText (enemyMana);
    }

    public void FillEnemyMana()
    {
        enemyMana = currentEnemyMaxMana;
        UIController.instance.SetEnemyManaText (enemyMana);
    }

    public void AdvanceTurn()
    {
        if (!battleEnded)
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

                    if (currentPlayerMaxMana < maxMana)
                    {
                        currentPlayerMaxMana++;
                    }

                    FillPlayerMana();

                    DeckController.instance.DrawMultipleCards (
                        cardsToDrawPerTurn
                    );

                    break;
                case TurnOrder.playerCardAttacks:
                    CardPointsController.instance.PlayerAttack();
                    break;
                case TurnOrder.enemyActive:
                    if (currentEnemyMaxMana < maxMana)
                    {
                        currentEnemyMaxMana++;
                    }

                    FillEnemyMana();

                    EnemyController.instance.StartAction();
                    break;
                case TurnOrder.enemyCardAttacks:
                    CardPointsController.instance.EnemyAttack();

                    break;
                default:
                    break;
            }
        }
    }

    public void EndPlayerTurn()
    {
        UIController.instance.endTurnButton.SetActive(false);
        UIController.instance.drawCardButton.SetActive(false);
        AdvanceTurn();
    }

    public void DamagePlayer(int damageAmount)
    {
        if (playerHealth > 0 || !battleEnded)
        {
            playerHealth -= damageAmount;

            if (playerHealth <= 0)
            {
                playerHealth = 0;

                EndBattle();
            }

            UIController.instance.SetPlayerHealthText (playerHealth);

            UIDamageIndicator damageClone =
                Instantiate(UIController.instance.playerDamage,
                UIController.instance.playerDamage.transform.parent);
            damageClone.damageText.text = damageAmount.ToString();
            damageClone.gameObject.SetActive(true);
        }
    }

    public void DamageEnemy(int damageAmount)
    {
        if (enemyHealth > 0 || !battleEnded)
        {
            enemyHealth -= damageAmount;

            if (enemyHealth <= 0)
            {
                enemyHealth = 0;

                EndBattle();
            }

            UIController.instance.SetEnemyHealthText (enemyHealth);

            UIDamageIndicator damageClone =
                Instantiate(UIController.instance.enemyDamage,
                UIController.instance.enemyDamage.transform.parent);
            damageClone.damageText.text = damageAmount.ToString();
            damageClone.gameObject.SetActive(true);
        }
    }

    void EndBattle()
    {
        battleEnded = true;

        HandController.instance.EmptyHand();

        if (enemyHealth <= 0)
        {
            UIController.instance.battleResultText.text = "YOU WON!";

            foreach (var point in CardPointsController.instance.enemyCardPoints)
            {
                if (point.activeCard != null)
                {
                    point
                        .activeCard
                        .MoveToPoint(discardPoint.position,
                        point.activeCard.transform.rotation);
                }
            }
        }
        else
        {
            UIController.instance.battleResultText.text = "YOU LOST!";

            foreach (var point in CardPointsController.instance.playerCardPoints
            )
            {
                if (point.activeCard != null)
                {
                    point
                        .activeCard
                        .MoveToPoint(discardPoint.position,
                        point.activeCard.transform.rotation);
                }
            }
        }

        StartCoroutine(ShowResultCo());
    }

    IEnumerator ShowResultCo()
    {
        yield return new WaitForSeconds(resultScreenDelayTime);

        UIController.instance.battleEndScreen.SetActive(true);
    }
}
