using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int currentHealth;

    public int

            attackPower,
            manaCost;

    public TMP_Text

            healthText,
            attackText,
            manaCostText;

    // Start is called before the first frame update
    void Start()
    {
        healthText.text = currentHealth.ToString();
        attackText.text = attackPower.ToString();
        manaCostText.text = manaCost.ToString();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
