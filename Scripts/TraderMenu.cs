using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TraderMenu : MonoBehaviour
{
    public Item[] ListOfItems;
    public GameObject[] buttons;

    public CharacterManager CM;
    public CardManager cardManager;
    public int CurrentItem;
    public bool lockedBuyingItem = false;// used so the player cant accidentally buy another item or leave the trader without getting their item

    private void Start()
    {
        CM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CharacterManager>();
        cardManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CardManager>();
    }

    public void BuyItem1()// buys the first item and will set the amount of buttons necessary for the amount of characters to active, each of these is a button function so they are individual
    {
        if (CM.AmountOfGold >= buttons[0].GetComponent<ButtonValues>().item.cost && !lockedBuyingItem)
        {
            for (int i = 0; i < CM.amountOfCharacters; i++)
            {
                buttons[0].GetComponent<ButtonValues>().PlayerButtons[i].SetActive(true);
            }
            FindObjectOfType<AudioManager>().Play("BuyItem");
            buttons[0].GetComponent<ButtonValues>().PlayerButtonsEmptyGO.SetActive(true);
            CurrentItem = 0;
            lockedBuyingItem = true;
        }
    }
    public void BuyItem2()
    {
        if (CM.AmountOfGold >= buttons[1].GetComponent<ButtonValues>().item.cost && !lockedBuyingItem)
        {
            for (int i = 0; i < CM.amountOfCharacters; i++)
            {
                buttons[1].GetComponent<ButtonValues>().PlayerButtons[i].SetActive(true);
            }
            FindObjectOfType<AudioManager>().Play("BuyItem");
            buttons[1].GetComponent<ButtonValues>().PlayerButtonsEmptyGO.SetActive(true);
            CurrentItem = 1;
            lockedBuyingItem = true;
        }
    }
    public void BuyItem3()
    {
        if (CM.AmountOfGold >= buttons[2].GetComponent<ButtonValues>().item.cost && !lockedBuyingItem)
        {
            for (int i = 0; i < CM.amountOfCharacters; i++)
            {
                buttons[2].GetComponent<ButtonValues>().PlayerButtons[i].SetActive(true);
            }
            FindObjectOfType<AudioManager>().Play("BuyItem");
            buttons[2].GetComponent<ButtonValues>().PlayerButtonsEmptyGO.SetActive(true);
            CurrentItem = 2;
            lockedBuyingItem = true;
        }
    }
    public void ContinueButton()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            for (int j = 0; j < buttons[i].gameObject.GetComponent<ButtonValues>().PlayerButtons.Count; j++)
            {
                buttons[i].GetComponent<ButtonValues>().PlayerButtons[j].SetActive(false);
            }
            buttons[i].GetComponent<ButtonValues>().PlayerButtonsEmptyGO.SetActive(false);
            buttons[i].SetActive(true);
        }
        cardManager.CardUIActive.SetActive(true);
        cardManager.CardUI.SetActive(false);
        cardManager.ui.SetActive(false);
    }
    public void BuyItemFor1()
    {
        ButtonValues values = buttons[CurrentItem].GetComponent<ButtonValues>();
        switch (values.item.currentItem)// will get the current item and will add the stat increases for the current player
        {
            case Item.Items.Armour:
                CM.characters[0].maxHealth += values.item.maxHealthIncrease; //TODO FINISH THESE
                break;
            case Item.Items.HealthPotion:
                CM.characters[0].currentHealth += values.item.HealthReward;
                break;
            case Item.Items.Weapon:
                CM.characters[0].minDamage += values.item.damageIncrease;
                CM.characters[0].maxDamage += values.item.damageIncrease;
                break;
        }
        CM.AmountOfGold -= values.item.cost;
        lockedBuyingItem = false;
        buttons[CurrentItem].SetActive(false);
    }
    public void BuyItemFor2()
    {
        ButtonValues values = buttons[CurrentItem].GetComponent<ButtonValues>();
        switch (values.item.currentItem)
        {
            case Item.Items.Armour:
                CM.characters[1].maxHealth += values.item.maxHealthIncrease; //TODO FINISH THESE
                break;
            case Item.Items.HealthPotion:
                CM.characters[1].currentHealth += values.item.HealthReward;
                break;
            case Item.Items.Weapon:
                CM.characters[1].minDamage += values.item.damageIncrease;
                CM.characters[1].maxDamage += values.item.damageIncrease;
                break;
        }
        CM.AmountOfGold -= values.item.cost;
        lockedBuyingItem = false;
        buttons[CurrentItem].SetActive(false);
    }
    public void BuyItemFor3()
    {
        ButtonValues values = buttons[CurrentItem].GetComponent<ButtonValues>();
        switch (values.item.currentItem)
        {
            case Item.Items.Armour:
                CM.characters[2].maxHealth += values.item.maxHealthIncrease;
                break;
            case Item.Items.HealthPotion:
                CM.characters[2].currentHealth += values.item.HealthReward;
                break;
            case Item.Items.Weapon:
                CM.characters[2].minDamage += values.item.damageIncrease;
                CM.characters[2].maxDamage += values.item.damageIncrease;
                break;
        }
        CM.AmountOfGold -= values.item.cost;
        lockedBuyingItem = false;
        buttons[CurrentItem].SetActive(false);
    }
    public void BuyItemFor4()
    {
        ButtonValues values = buttons[CurrentItem].GetComponent<ButtonValues>();
        switch (values.item.currentItem)
        {
            case Item.Items.Armour:
                CM.characters[3].maxHealth += values.item.maxHealthIncrease; //TODO FINISH THESE
                break;
            case Item.Items.HealthPotion:
                CM.characters[3].currentHealth += values.item.HealthReward;
                break;
            case Item.Items.Weapon:
                CM.characters[3].minDamage += values.item.damageIncrease;
                CM.characters[3].maxDamage += values.item.damageIncrease;
                break;
        }
        CM.AmountOfGold -= values.item.cost;
        buttons[CurrentItem].GetComponent<ButtonValues>().PlayerButtonsEmptyGO.SetActive(false);
        buttons[CurrentItem].SetActive(false);
        lockedBuyingItem = false;
    }

}
