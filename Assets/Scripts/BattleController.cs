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

    // Start is called before the first frame update
    void Start()
    {
        playerMana = startingMana;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SpendPlayerMana(int amountToSpend)
    {
        playerMana = playerMana - amountToSpend;

        if (playerMana < 0)
        {
            playerMana = 0;
        }
    }
}
