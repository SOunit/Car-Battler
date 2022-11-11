using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    private void Awake()
    {
        instance = this;
    }

    public TMP_Text

            playerHealthText,
            playerManaText,
            enemyHealthText,
            enemyManaText;

    public GameObject manaWarning;

    public float manaWarningTime;

    private float manaWarningCounter;

    public GameObject

            drawCardButton,
            endTurnButton;

    public UIDamageIndicator

            playerDamage,
            enemyDamage;

    public GameObject battleEndScreen;

    public TMP_Text battleResultText;

    public string

            mainMenuScene,
            battleSelectScene;

    public GameObject pauseScreen;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (manaWarningCounter > 0)
        {
            manaWarningCounter -= Time.deltaTime;

            if (manaWarningCounter <= 0)
            {
                manaWarning.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void SetPlayerHealthText(int healthAmount)
    {
        playerHealthText.text = $"Player Health : {healthAmount}";
    }

    public void SetPlayerManaText(int manaAmount)
    {
        playerManaText.text = "Mana : " + manaAmount;
    }

    public void SetEnemyHealthText(int healthAmount)
    {
        enemyHealthText.text = $"Enemy Health : {healthAmount}";
    }

    public void SetEnemyManaText(int manaAmount)
    {
        enemyManaText.text = "Mana : " + manaAmount;
    }

    public void ShowManaWarning()
    {
        manaWarning.SetActive(true);
        manaWarningCounter = manaWarningTime;
    }

    public void DrawCard()
    {
        DeckController.instance.DrawCardForMana();
    }

    public void EndPlayerTurn()
    {
        BattleController.instance.EndPlayerTurn();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene (mainMenuScene);

        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Time.timeScale = 1f;
    }

    public void ChooseNewBattle()
    {
        SceneManager.LoadScene (battleSelectScene);

        Time.timeScale = 1f;
    }

    public void PauseUnpause()
    {
        if (!pauseScreen.activeSelf)
        {
            pauseScreen.SetActive(true);

            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.SetActive(false);

            Time.timeScale = 1f;
        }
    }
}
