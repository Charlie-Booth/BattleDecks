using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] Levels;// an array of levels to choose from
    public Transform LevelSp;
    public SavedData saveFile;
    public CharacterManager characterManager;
    public WinLossScreen winLossScreen;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        LoadLevel();
        for (int i = 0; i < saveFile.amountOfCharacters; i++)
        {
            characterManager.characters.Add(new CharacterManager.Character());// creates a new character in the character manager 
        }
        for (int i = 0; i < characterManager.characters.Count; i++) //will get the new characters stats from the save file and add them to the character managar
        {
            characterManager.characters[i].actions = saveFile.characters[i].actions;
            characterManager.characters[i].name = saveFile.characters[i].name;
            characterManager.characters[i].charStats = saveFile.characters[i].charStats;
            characterManager.characters[i].currentHealth = saveFile.characters[i].currentHealth;
            characterManager.characters[i].exp = saveFile.characters[i].exp;
            characterManager.characters[i].currentExp = saveFile.characters[i].currentExp;
            characterManager.characters[i].level = saveFile.characters[i].level;
            characterManager.characters[i].maxHealth = saveFile.characters[i].maxHealth;
            characterManager.characters[i].moveDistance = saveFile.characters[i].moveDistance;
            characterManager.characters[i].attackDistance = saveFile.characters[i].attackDistance;
            characterManager.characters[i].minDamage = saveFile.characters[i].minDamage;
            characterManager.characters[i].maxDamage = saveFile.characters[i].maxDamage;
            characterManager.characters[i].DashDistance = saveFile.characters[i].DashDistance;
            characterManager.characters[i].characterNumber = saveFile.characters[i].characterNumber;
            characterManager.characters[i].playerColor = saveFile.characters[i].playerColor;
            switch (saveFile.characters[i].theClass)
            {
                case SavedData.Character.Class.Archer:
                    characterManager.characters[i].theClass = CharacterManager.Character.Class.Archer;
                    break;
                case SavedData.Character.Class.Theif:
                    characterManager.characters[i].theClass = CharacterManager.Character.Class.Theif;
                    break;
                case SavedData.Character.Class.Warrior:
                    characterManager.characters[i].theClass = CharacterManager.Character.Class.Warrior;
                    break;
            }
            switch (saveFile.characters[i].race)
            {
                case SavedData.Character.Race.Dwarf:
                    characterManager.characters[i].race = CharacterManager.Character.Race.Dwarf;
                    break;
                case SavedData.Character.Race.Elf:
                    characterManager.characters[i].race = CharacterManager.Character.Race.Elf;
                    break;
                case SavedData.Character.Race.Human:
                    characterManager.characters[i].race = CharacterManager.Character.Race.Human;
                    break;

            }
            characterManager.AmountOfGold = saveFile.CurrentGold;
        }

        void LoadLevel()// will pick a random level from the list, adds another procedural element into the game
        {
            int r = Random.Range(0, Levels.Length);
            Instantiate(Levels[r], LevelSp.position, Quaternion.identity);
        }

        FindObjectOfType<AudioManager>().Play("BattleMusic");
    }
}
