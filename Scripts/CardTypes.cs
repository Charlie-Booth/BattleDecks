using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardTypes : ScriptableObject
{
    //data for the card type
    public enum CardType { Trader, Enemies, RandomEncounter, NewCharacter , EndCard  }
    public CardType eventType;
    public NewCharacter[] newCharacters;
    public int AmountOfEnemies;
    public string Title;
    public string description;
    public float healthReward;
    public float healthIncrease;
    public float expIncrease;
    public int goldReward;
    public Sprite cardIcon;
}
