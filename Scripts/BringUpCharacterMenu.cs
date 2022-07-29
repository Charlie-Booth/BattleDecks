using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BringUpCharacterMenu : MonoBehaviour
{
    public CharacterManager characterManager; 
    public GameObject[] CharacterButtons;
    public GameObject[] characterSheets;
    //public List<GameObject> players;
    //public GameObject Level;

    public void SetCharacterSheet()
    {
        //sets each character sheet with the correct stats for each character
        for (int i = 0; i < characterManager.characters.Count; i++) // sets the 
        {
            characterSheets[i].GetComponent<CharacterSheet>().CharacterName.text = characterManager.characters[i].name;
            characterSheets[i].GetComponent<CharacterSheet>().Class.text = "Class: "+characterManager.characters[i].theClass.ToString();
            characterSheets[i].GetComponent<CharacterSheet>().Race.text = "Race: "+ characterManager.characters[i].race.ToString();
            characterSheets[i].GetComponent<CharacterSheet>().Health.text = "Max Health: " + characterManager.characters[i].maxHealth.ToString();
            characterSheets[i].GetComponent<CharacterSheet>().Icon.sprite = characterManager.characters[i].icon;

            //characterSheets[i].GetComponent<CharacterSheet>().exp.text = characterManager.characters[i].currentExp.ToString(); <--- Removed potential levelling system
            float averageDamage = (characterManager.characters[i].minDamage + characterManager.characters[i].maxDamage) / 2;
            characterSheets[i].GetComponent<CharacterSheet>().AverageDamage.text = "Average Damage: "+ averageDamage.ToString();
            characterSheets[i].GetComponent<CharacterSheet>().healthBar.value = calculateHealth(characterManager.characters[i]);
        }

    }
    public void Update()
    {
        //Level = GameObject.FindGameObjectWithTag("Level");
        
        //sets each button active for each character in the count
        for (int i = 0; i < characterManager.characters.Count; i++)
        {
            CharacterButtons[i].SetActive(true);
        }
       // players = Level.GetComponent<PlayerSpawn>().players;
    }

    // Each a button function to open a character sheet
    public void OpenMenu1()
    {
        SetCharacterSheet();
        characterManager.CurrentCard = 0;
        characterSheets[0].SetActive(true);
    }
    public void OpenMenu2()
    {
        SetCharacterSheet();
        characterManager.CurrentCard = 1;
        characterSheets[1].SetActive(true);
    }
    public void OpenMenu3()
    {
        SetCharacterSheet();
        characterManager.CurrentCard = 2;
        characterSheets[2].SetActive(true);
    }
    public void OpenMenu4()
    {
        SetCharacterSheet();
        characterManager.CurrentCard = 3;
        characterSheets[3].SetActive(true);
    }
    // used to find out the health value of a slider 
    float calculateHealth(CharacterManager.Character character)
    {
        return character.currentHealth / character.maxHealth;
    }
}
