using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class SavedData : ScriptableObject
{
    [System.Serializable]
    public class Character // the same class as the character manager
    {
        public enum Race { Human, Elf, Dwarf }
        public Race race;
        public enum Class { Warrior, Theif, Archer }
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

        public int characterNumber;

        public Color playerColor;
        public Sprite icon;
    } 
    // holds all the values that are needed to be transferred between scenes
    public CharacterSheet characterSheet;
    public List<Character> characters;
    public List<int> cardsPlayed;
    //public List<int> spawnPointsUsed;
    public int currentCard;
    public bool firstTurnDone;
    public int amountOfCharacters;
    public int amountOfEnemies;
    public float HealthReward;
    public int CurrentGold;
    public float expIncrease;
    public float healthIncrease;
    public int goldReward;
}
