using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WinLossScreen : MonoBehaviour
{
    public TMP_Text WinLossText;
    public TMP_Text amountOfEnemiesKilledText;
    public TMP_Text GoldOrDeadPlayersText;
    public TMP_Text ButtonText;
    public int enemiesKilled;
    // Start is called before the first frame update
    public SavedData saveFile;
    public PlayerSpawn playerSpawn;
    public CharacterManager characterManager;
    public GameObject winScreen;

    public void Update()
    {
        playerSpawn = GameObject.FindGameObjectWithTag("Level").GetComponent<PlayerSpawn>();
        characterManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CharacterManager>();

    }
    public void Win() // will change text if the player has won
    {
        winScreen.SetActive(true);
        WinLossText.text = "All Enemies Defeated";
        amountOfEnemiesKilledText.text = "Enemies Killed: "+ enemiesKilled.ToString();
        GoldOrDeadPlayersText.text = "Gold Collected: " + saveFile.goldReward.ToString();
        ButtonText.text = "Return To Adventure";
        saveFile.firstTurnDone = true;
        Time.timeScale = 0f;
    }
    public void Loss() // will change text if the player has lost
    {
        winScreen.SetActive(true);
        foreach(GameObject gameObject in playerSpawn.enemies)
        {
            Destroy(gameObject);
        }
        playerSpawn.enemies.Clear();
        playerSpawn.players.Clear();
        WinLossText.text = "All Adventurers Dead";
        amountOfEnemiesKilledText.text = "Enemies Killed: " + enemiesKilled.ToString();
        //GoldOrDeadPlayersText.text = "Fallen Adventurers: " + saveFile.characters[0].name + " " + saveFile.characters[1].name + " " + saveFile.characters[2].name + " " + saveFile.characters[3].name;
        ButtonText.text = "Restart the Adventure";
        saveFile.firstTurnDone = false;
        Time.timeScale = 0f;
    }
    public void GoBackToCardTable() // will bring the player back to the card table and will set new characters in the save file if any have died
    {
        if (saveFile.firstTurnDone)
        {
            saveFile.characters.Clear();
            for (int i = 0; i < characterManager.characters.Count; i++)
            {
                saveFile.characters.Add(new SavedData.Character());
                // all data that is needed in the card table
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
                saveFile.CurrentGold = characterManager.AmountOfGold;
            }
        }
        SceneManager.LoadScene(1);
    }
}
