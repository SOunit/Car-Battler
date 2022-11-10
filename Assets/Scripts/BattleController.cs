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

    // Start is called before the first frame update
    void Start()
    {
        currentPlayerMaxMana = startingMana;
        FillPlayerMana();

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

                if (currentPlayerMaxMana < maxMana)
                {
                    currentPlayerMaxMana++;
                }

                FillPlayerMana();

                DeckController.instance.DrawMultipleCards (cardsToDrawPerTurn);

                break;
            case TurnOrder.playerCardAttacks:
                CardPointsController.instance.PlayerAttack();
                break;
            case TurnOrder.enemyActive:
                EnemyController.instance.StartAction();
                break;
            case TurnOrder.enemyCardAttacks:
                CardPointsController.instance.EnemyAttack();

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

    public void DamagePlayer(int damageAmount)
    {
        if (playerHealth > 0)
        {
            playerHealth -= damageAmount;

            if (playerHealth <= 0)
            {
                playerHealth = 0;

                // end battle
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
        if (enemyHealth > 0)
        {
            enemyHealth -= damageAmount;

            if (enemyHealth <= 0)
            {
                enemyHealth = 0;

                // end battle
            }

            UIController.instance.SetEnemyHealthText (enemyHealth);

            UIDamageIndicator damageClone =
                Instantiate(UIController.instance.enemyDamage,
                UIController.instance.enemyDamage.transform.parent);
            damageClone.damageText.text = damageAmount.ToString();
            damageClone.gameObject.SetActive(true);
        }
    }
}
