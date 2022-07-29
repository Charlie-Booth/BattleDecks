using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //UNUSED SCRIPT, MAY BE USED IN THE FUTURE
public class CardValues
{
    public bool isActive;

    public string title;
    public string description;
    public int goldReward;
    //create a number of rewards per card. this can be changed, potentially used as a scriptable object
    public int numberOfEnemies;
    public int healthReward;
    public int healthIncrease;
    public int experianceReward;

}
