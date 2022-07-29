using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardDescription : MonoBehaviour
{
    
    public TMP_Text EventName;
    public TMP_Text Description;
    public TMP_Text ButtonDescription;
    public CardEvent currentCard;
    public CardManager cardManager;
    public GameObject TraderCard;

    private void Start()
    {
        cardManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CardManager>();
    }


    public void Update()//Sets the description on the card
    {
        EventName.text = currentCard.CardTypeScriptable.Title;
        Description.text = currentCard.CardTypeScriptable.description;
        ButtonDescription.text = currentCard.buttonDescription;
    }
}
