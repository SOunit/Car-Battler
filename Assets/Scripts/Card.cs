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

    public bool inHand;

    public int handPosition;

    private HandController theHC;

    private bool isSelected;

    private Collider theCol;

    public LayerMask whatIsDesktop;

    // Start is called before the first frame update
    void Start()
    {
        SetupCard();

        theHC = FindObjectOfType<HandController>();
        theCol = GetComponent<Collider>();
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

        if (isSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, whatIsDesktop))
            {
                MoveToPoint(hit.point + new Vector3(0f, 2f, 0f),
                Quaternion.identity);
            }

            if (Input.GetMouseButtonDown(1))
            {
                ReturnToHand();
            }
        }
    }

    public void MoveToPoint(Vector3 pointToMoveTo, Quaternion rotToMatch)
    {
        targetPoint = pointToMoveTo;
        targetRot = rotToMatch;
    }

    private void OnMouseOver()
    {
        if (inHand)
        {
            MoveToPoint(theHC.cardPositions[handPosition] +
            new Vector3(0f, 1f, .5f),
            Quaternion.identity);
        }
    }

    private void OnMouseExit()
    {
        if (inHand)
        {
            MoveToPoint(theHC.cardPositions[handPosition],
            theHC.minPos.rotation);
        }
    }

    private void OnMouseDown()
    {
        if (inHand)
        {
            isSelected = true;
            theCol.enabled = false;
        }
    }

    public void ReturnToHand()
    {
        isSelected = false;
        theCol.enabled = true;

        MoveToPoint(theHC.cardPositions[handPosition], theHC.minPos.rotation);
    }
}
