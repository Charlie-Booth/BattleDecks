using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class NewCharacter : ScriptableObject
{ // holds all the stats necessary for a new character
    public enum Race {Human, Elf, Dwarf }
    public enum Class {Warrior, Thief, Archer }
    public Race race;
    public Class playerClass;
    public string[] Names;
    public float baseMaxHealth = 100;
    public int baseMovementSpeed = 3;
    public float baseMinDamage = 10;
    public float baseMaxDamage = 40;
    public int baseAttackRange = 2;
    public int baseActions = 2;
    public Sprite Icon;



}
