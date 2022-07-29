using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardEvent : MonoBehaviour
{

    public CardTypes CardTypeScriptable;
    public string buttonDescription;
    public CharacterManager characterManager;
    public CardManager cardManager;
    public TraderMenu traderMenu;
    public GameObject TraderMenuUi;
    public SavedData saveFile;
    public GameObject cardUI;
    public GameObject CardBackGroundUI;
    //public GameObject WinScreenUi;
    
    private void Start()
    {
        characterManager = GameObject.FindGameObjectWithTag("GameManager").gameObject.GetComponent<CharacterManager>();
        cardManager = GameObject.FindGameObjectWithTag("GameManager").gameObject.GetComponent<CardManager>();
        saveFile =  GameObject.FindGameObjectWithTag("GameManager").gameObject.GetComponent<CharacterManager>().saveFile;
        TraderMenuUi = cardManager.ui;
        traderMenu = cardManager.traderMenu;
        cardUI = cardManager.CardUIActive;
        CardBackGroundUI = cardManager.CardUI;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void ButtonCheck() // Changes the name on the button of the card depending on the card type
    {
        switch (CardTypeScriptable.eventType)
        {
            case CardTypes.CardType.Enemies:
                buttonDescription = "Load Level";
                break;
            case CardTypes.CardType.NewCharacter:
                if (characterManager.characters.Count < characterManager.MaxAmountOfCharacters)
                {
                    buttonDescription = "Add To Party";
                }
                else
                {
                    buttonDescription = "Party Full Continue";
                }
                break;
            case CardTypes.CardType.RandomEncounter:
                buttonDescription = "Continue";
                break;
            case CardTypes.CardType.EndCard:
                buttonDescription = "Restart the adventure";
                break;
            case CardTypes.CardType.Trader:
                buttonDescription = "Talk To Trader";
                break;

        }
    }

    public void CardContinue() // Changes Values and events based on a button press
    {
        switch (CardTypeScriptable.eventType) 
        {
            case CardTypes.CardType.Enemies:

                for (int i = 0; i < characterManager.characters.Count; i++)// will transfer data to the save file between scenes
                {
                    if(saveFile.characters.Count< characterManager.characters.Count)// will add a character if the current save file doesnt have enough characters(Mainly used if there is only 1 character left)
                    {
                        saveFile.characters.Add(new SavedData.Character());
                    }
                    saveFile.characters[i].actions = characterManager.characters[i].actions;
                    saveFile.characters[i].attackDistance = characterManager.characters[i].attackDistance;
                    saveFile.characters[i].charStats = characterManager.characters[i].charStats;
                    saveFile.characters[i].currentExp = characterManager.characters[i].currentExp;
                    saveFile.characters[i].currentHealth = characterManager.characters[i].currentHealth;
                    saveFile.characters[i].maxHealth = characterManager.characters[i].maxHealth;
                    saveFile.characters[i].DashDistance = characterManager.characters[i].DashDistance;
                    saveFile.characters[i].exp = characterManager.characters[i].exp;
                    saveFile.characters[i].name = characterManager.characters[i].name;
                    saveFile.characters[i].level = characterManager.characters[i].level;
                    saveFile.characters[i].maxDamage = characterManager.characters[i].maxDamage;
                    saveFile.characters[i].minDamage = characterManager.characters[i].minDamage;
                    saveFile.characters[i].moveDistance = characterManager.characters[i].moveDistance;
                    saveFile.characters[i].characterNumber = characterManager.characters[i].characterNumber;
                    saveFile.characters[i].playerColor = characterManager.characters[i].playerColor;
                    saveFile.characters[i].icon = characterManager.characters[i].icon;

                    switch (characterManager.characters[i].race)
                    {
                        case CharacterManager.Character.Race.Dwarf:
                            saveFile.characters[i].race = SavedData.Character.Race.Dwarf;
                            break;
                        case CharacterManager.Character.Race.Elf:
                            saveFile.characters[i].race = SavedData.Character.Race.Elf;
                            break;
                        case CharacterManager.Character.Race.Human:
                            saveFile.characters[i].race = SavedData.Character.Race.Human;
                            break;
                    }
                    switch (characterManager.characters[i].theClass)
                    {
                        case CharacterManager.Character.Class.Archer:
                            saveFile.characters[i].theClass = SavedData.Character.Class.Archer;
                            break;
                        case CharacterManager.Character.Class.Warrior:
                            saveFile.characters[i].theClass = SavedData.Character.Class.Warrior;
                            break;
                        case CharacterManager.Character.Class.Theif:
                            saveFile.characters[i].theClass = SavedData.Character.Class.Theif;
                            break;
                    }
                    saveFile.amountOfCharacters = characterManager.amountOfCharacters;
                }
                if (!saveFile.firstTurnDone)// at the start of the run it will add all the current cards numbers to an array in the save file and will pick them when returning
                {
                    for (int i = 0; i < cardManager.CardsInPlayNumber.Count; i++)
                    {
                        //saveFile.cardsPlayed.Remove(saveFile.cardsPlayed[i]);
                        saveFile.cardsPlayed.Add(cardManager.CardsInPlayNumber[i]);
                    }
                }
                saveFile.currentCard = cardManager.currentCard;
                saveFile.amountOfEnemies = CardTypeScriptable.AmountOfEnemies;
                saveFile.goldReward = CardTypeScriptable.goldReward;
                saveFile.CurrentGold = characterManager.AmountOfGold;
                saveFile.firstTurnDone = true;
                CardBackGroundUI.SetActive(false);
                //HERE IS WHERE WE DO ALL THE LOADING 
                
                SceneManager.LoadScene(2);// will load the battle scene
                break;
            case CardTypes.CardType.NewCharacter:
                if (characterManager.characters.Count < characterManager.MaxAmountOfCharacters) // Checks to see if there are already the maximum amount of characters 
                {
                    CharacterManager.Character character = new CharacterManager.Character();// creates a new character and will set their stats and names
                    //characterManager.characters.Add(new CharacterManager.Character());
                    //int latestCreatedCharacter = characterManager.characters.Count;
                    NewCharacter newCharacter = CardTypeScriptable.newCharacters[Random.Range(0, CardTypeScriptable.newCharacters.Length)];
                    character.name = newCharacter.Names[Random.Range(0, newCharacter.Names.Length)];
                    character.icon = newCharacter.Icon;
                    switch (newCharacter.race)
                    {
                        case NewCharacter.Race.Human:
                            character.race = CharacterManager.Character.Race.Human;
                            break;
                        case NewCharacter.Race.Dwarf:
                            character.race = CharacterManager.Character.Race.Dwarf;
                            break;

                        case NewCharacter.Race.Elf:
                            character.race = CharacterManager.Character.Race.Elf;
                            break;
                          
                    }
                    switch (newCharacter.playerClass)
                    {
                        case NewCharacter.Class.Archer:
                            character.theClass = CharacterManager.Character.Class.Archer;
                            break;
                        case NewCharacter.Class.Warrior:
                           character.theClass = CharacterManager.Character.Class.Warrior;
                            break;
                        case NewCharacter.Class.Thief:
                           character.theClass = CharacterManager.Character.Class.Theif;
                            break;
                    }
                    character.charStats = newCharacter;
                    characterManager.InitialiseValues(character);
                    characterManager.characters.Add(character);
                    CardBackGroundUI.SetActive(false);
                }
                else
                {
                    CardBackGroundUI.SetActive(false);// turns off the card background
                }
                break;
            case CardTypes.CardType.EndCard:
                //saveFile.firstTurnDone = false; // weird error here
                SceneManager.LoadScene(0);// restarts the game
                break;
            case CardTypes.CardType.Trader: // will set the trader menu to active and will add the items
                cardUI.SetActive(false);
                TraderMenuUi.SetActive(true);

                for (int i = 0; i < traderMenu.buttons.Length; i++)
                {
                    ButtonValues values = traderMenu.buttons[i].GetComponent<ButtonValues>();
                    values.item = traderMenu.ListOfItems[Random.Range(0, traderMenu.ListOfItems.Length)];
                    values.ItemName.text = values.item.itemName;
                    values.itemImage.sprite = values.item.itemPicture;
                    values.Description.text = values.item.Description;
                    values.Cost.text = "Cost: " +values.item.cost.ToString()+ " Gold";
                    values.itemImage.sprite = values.item.itemPicture;
                    
                }
               
                break;

        }
    }
}
