using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterManager : MonoBehaviour
{
  [System.Serializable]
  public class Character // class for each character and will hold all the data for each character
    {
        public enum Race {Human, Elf, Dwarf }
        public Race race;
        public enum Class {Warrior, Theif, Archer }
        public Class theClass;
        public string name;

        public NewCharacter charStats;

        public float currentHealth;
        public float exp;
        public float currentExp;
        public int level;
        [Header("HEALTH")]
        public float maxHealth;

        [Header("MOVEMENT")]
        public int moveDistance;

        [Header("ATTACK DISTANCE")]       
        public int attackDistance;

        [Header("DAMAGE VALUES")]    
        public float minDamage;
        public float maxDamage;

        [Header("DASH ")]
        public int DashDistance;

        [Header("ACTIONS")]
        public int actions;

        public Color playerColor;

        public int characterNumber;

        public Sprite icon;

    }

    public List<Character> characters;// creates a list of characters
    public NewCharacter[] listOfRacesAndClasses;
    public int initialAmountOfCharacters;
    public int MaxAmountOfCharacters = 4;
    public int amountOfCharacters;
    public int CurrentCard;
    public int AmountOfGold;
    public SavedData saveFile;
    public TMP_Text GoldText;

    private void Start()
    {
       
        if (!saveFile.firstTurnDone)
        {
            AmountOfGold = 10; // initial amount of gold
            for (int i = 0; i < initialAmountOfCharacters; i++)
            {
                characters.Add(new Character());
            }
            for (int i = 0; i < characters.Count; i++)
            {
                NewCharacter currentCharacter = listOfRacesAndClasses[Random.Range(0, listOfRacesAndClasses.Length)];
                characters[i].name = currentCharacter.Names[Random.Range(0, currentCharacter.name.Length)];
                characters[i].charStats = currentCharacter;
                characters[i].characterNumber = i;
                characters[i].icon = currentCharacter.Icon;
                characters[i].playerColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                switch (currentCharacter.playerClass)
                {
                    case NewCharacter.Class.Archer:
                        characters[i].theClass = Character.Class.Archer;
                        break;
                    case NewCharacter.Class.Warrior:
                        characters[i].theClass = Character.Class.Warrior;
                        break;
                    case NewCharacter.Class.Thief:
                        characters[i].theClass = Character.Class.Theif;
                        break;
                }
                switch (currentCharacter.race)
                {
                    case NewCharacter.Race.Dwarf:
                        characters[i].race = Character.Race.Dwarf;
                        break;
                    case NewCharacter.Race.Elf:
                        characters[i].race = Character.Race.Elf;
                        break;
                    case NewCharacter.Race.Human:
                        characters[i].race = Character.Race.Human;
                        break;

                }

                InitialiseValues(characters[i]);
            }
            
        }
        else
        {
            // will transfer all of the data from the save file to the character manager
            AmountOfGold = saveFile.CurrentGold;
            AmountOfGold += saveFile.goldReward;
            for (int i = 0; i < saveFile.characters.Count; i++)
            {
                if (characters.Count < saveFile.characters.Count)
                {
                    characters.Add(new Character());
                }
                characters[i].name = saveFile.characters[i].name;
                characters[i].charStats = saveFile.characters[i].charStats ;
                characters[i].characterNumber = saveFile.characters[i].characterNumber;
                characters[i].currentHealth = saveFile.characters[i].currentHealth;
                characters[i].exp = saveFile.characters[i].exp;
                characters[i].currentExp = saveFile.characters[i].currentExp;
                characters[i].level = saveFile.characters[i].level;
                characters[i].maxHealth = saveFile.characters[i].maxHealth;
                characters[i].moveDistance = saveFile.characters[i].moveDistance;
                characters[i].attackDistance = saveFile.characters[i].attackDistance;
                characters[i].minDamage = saveFile.characters[i].minDamage;
                characters[i].maxDamage = saveFile.characters[i].maxDamage;
                characters[i].DashDistance = saveFile.characters[i].DashDistance;
                characters[i].actions = saveFile.characters[i].actions;
                characters[i].playerColor = saveFile.characters[i].playerColor;
                characters[i].icon = saveFile.characters[i].icon;
                

                switch (saveFile.characters[i].theClass)
                {
                    case SavedData.Character.Class.Archer:
                        characters[i].theClass = Character.Class.Archer;
                        break;
                    case SavedData.Character.Class.Warrior:
                        characters[i].theClass = Character.Class.Warrior;
                        break;
                    case SavedData.Character.Class.Theif:
                        characters[i].theClass = Character.Class.Theif;
                        break;
                }
                switch (saveFile.characters[i].race)
                {
                    case SavedData.Character.Race.Dwarf:
                        characters[i].race = Character.Race.Dwarf;
                        break;
                    case SavedData.Character.Race.Elf:
                        characters[i].race = Character.Race.Elf;
                        break;
                    case SavedData.Character.Race.Human:
                        characters[i].race = Character.Race.Human;
                        break;

                }

                
            }
        }
    }

    public void Update()
    {
        // Will change the name of the text and will make it so that the player cannot go over the maximum amount of health
        if(GoldText != null)
        {
            GoldText.text = "Gold: " + AmountOfGold.ToString();
        }
        amountOfCharacters = characters.Count;
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].characterNumber = i;
        }
        for (int i = 0; i < characters.Count; i++)
        {
            if(characters[i].currentHealth > characters[i].maxHealth)
            {
                characters[i].currentHealth = characters[i].maxHealth;
            }
        }
    }
    public void InitialiseValues(Character character) // This sets the values for new characters and the base characters when the game starts
    {
        //Gets the base stats of the character
        character.actions = character.charStats.baseActions;
        character.maxHealth = character.charStats.baseMaxHealth;       
        character.minDamage = character.charStats.baseMinDamage;
        character.maxDamage = character.charStats.baseMaxDamage;
        character.attackDistance = character.charStats.baseAttackRange;
        character.moveDistance = character.charStats.baseMovementSpeed;
        character.playerColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f); // sets a random player Colour
        float minDam = character.minDamage / 2;// will get half of the minimum damage
        float maxDam = character.maxDamage / 2;// will get half of the maximum damage
        switch (character.race)// changes stats based on the players race
        {          

            case Character.Race.Dwarf://Permanently Has less move distance but more health
                character.maxHealth = character.maxHealth + WorkOutTwentyPercent(character.maxHealth); // adds on twenty percent of the players base health
                character.moveDistance = character.moveDistance - 1;

                switch (character.theClass)
                {
                    case Character.Class.Archer:

                        character.attackDistance = character.attackDistance * 2;
                        
                        character.minDamage = character.minDamage - 5;
                        character.maxDamage = character.maxDamage - 10;
                        
                        break;
                    case Character.Class.Theif:
                        character.actions = character.actions + 1;
                        
                        break;
                    case Character.Class.Warrior:
                        character.attackDistance = character.attackDistance - 1;
                        character.minDamage = character.minDamage + 10;
                        character.maxDamage = character.maxDamage +15 ;
                        break;
                }
                break;
            case Character.Race.Elf://Elf Permanently has +1 move distance but less health
                character.maxHealth = character.maxHealth - WorkOutTwentyPercent(character.maxHealth);
                character.moveDistance = character.moveDistance + 1;
                switch (character.theClass)
                {
                    case Character.Class.Archer://archer has the base move distance but its attack range is further
                        character.attackDistance = character.attackDistance * 2;
                        character.minDamage = character.minDamage - 10;
                        character.maxDamage = character.maxDamage - 5;
                        break;
                    case Character.Class.Theif://Theif has a greater move speed and has less health
                        character.actions = character.actions + 1;
                        break;
                    case Character.Class.Warrior:// shorter attack range with more health
                        character.attackDistance = character.attackDistance - 1;
                        character.minDamage = character.minDamage + 10;
                        character.maxDamage = character.maxDamage + 15;
                        break;
                }
                break;
            case Character.Race.Human: // Middle of the road Character

                switch (character.theClass)
                {
                    case Character.Class.Archer:
                        character.attackDistance = character.attackDistance * 2;
                        character.minDamage = character.minDamage - 10;
                        character.maxDamage = character.maxDamage - 5;
                        break;
                    case Character.Class.Theif:
                        character.actions = character.actions + 1;
                        break;
                    case Character.Class.Warrior:
                        character.attackDistance = character.attackDistance - 1;
                        character.minDamage = character.minDamage + 10;
                        character.maxDamage = character.maxDamage + 15;
                        break;
                }
                break;
        }
        character.currentHealth = character.maxHealth;
        character.DashDistance = character.moveDistance * 2;
    }

    public float WorkOutTwentyPercent(float fullValue)
    {
        float twentyPercent = fullValue / 5;
        return twentyPercent;
    }
    public float WorkOutFiftyPercent(float fullValue)
    {
        float fiftyPercent = fullValue / 2;
        return fiftyPercent;
    }
}
