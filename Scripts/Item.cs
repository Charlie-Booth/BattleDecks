using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public enum Items{ HealthPotion, Weapon, Armour, }
    public Items currentItem;
    public string itemName;
    public string Description;
    public Sprite itemPicture;
    public int cost;
    public float HealthReward;
    public float damageIncrease;
    public float maxHealthIncrease;

}
