using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardScriptableObject cardSO;

    public int currentHealth;

    public int

            attackPower,
            manaCost;

    public TMP_Text

            healthText,
            attackText,
            manaCostText,
            nameText,
            actionDescriptionText,
            loreText;

    public Image

            characterArt,
            bgArt;

    private Vector3 targetPoint;

    private Quaternion targetRot;

    public float

            moveSpeed = 5f,
            rotateSpeed = 540f;

    // Start is called before the first frame update
    void Start()
    {
        SetupCard();
    }

    public void SetupCard()
    {
        currentHealth = cardSO.currentHealth;
        attackPower = cardSO.attackPower;
        manaCost = cardSO.manaCost;

        healthText.text = currentHealth.ToString();
        attackText.text = attackPower.ToString();
        manaCostText.text = manaCost.ToString();

        nameText.text = cardSO.cardName;
        actionDescriptionText.text = cardSO.actionDescription;
        loreText.text = cardSO.cardLore;

        characterArt.sprite = cardSO.characterSprite;
        bgArt.sprite = cardSO.bgSprite;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position =
            Vector3
                .Lerp(transform.position,
                targetPoint,
                moveSpeed * Time.deltaTime);

        transform.rotation =
            Quaternion
                .RotateTowards(transform.rotation,
                targetRot,
                rotateSpeed * Time.deltaTime);
    }

    public void MoveToPoint(Vector3 pointToMoveTo, Quaternion rotToMatch)
    {
        targetPoint = pointToMoveTo;
        targetRot = rotToMatch;
    }
}
