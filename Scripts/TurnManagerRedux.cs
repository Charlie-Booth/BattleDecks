using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManagerRedux : MonoBehaviour
{
    
    public List<GameObject> players;
    public List<GameObject> enemies;
    public int currentCharacter = 0;
    private int initialCurrentCharacter = 0;
    public int maxCurrentCharacter;
    public bool playersTurn = true; // the players turn is always first
    public bool enemiesTurn;
    public bool startDoingTurns;
    public int amountOfPlayers;
    public int amountOfEnemies;

    private void Update()
    {
        
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerController>().numberInManagerList = i; // sets the players number in the manager list equal to the current number
            
        }
        
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].GetComponent<EnemyController>().numberInManagerList = i; // the same is applied here
            }
        
        amountOfEnemies = enemies.Count;
        amountOfPlayers = players.Count;
        if (startDoingTurns)
        {
            if (playersTurn&&!enemiesTurn)
            {
                maxCurrentCharacter = players.Count; // gets the max amount of characters in this teams turn
                if (currentCharacter < amountOfPlayers)
                {
                    players[currentCharacter].GetComponent<Movement>().turn = true; // will set the current players turn to true
                }
                if(currentCharacter>= amountOfPlayers) // will swap the turns
                {
                    //SwapTurns();
                    currentCharacter = 0;
                    playersTurn = false;
                    enemiesTurn = true;
                }
                
            }
            else if (enemiesTurn&&!playersTurn)// enemies turn
            {
                maxCurrentCharacter = enemies.Count;
                if (currentCharacter < amountOfEnemies)
                {
                    enemies[currentCharacter].GetComponent<Movement>().turn = true;
                }
                if(currentCharacter >= amountOfEnemies)
                {
                    //SwapTurns();
                    currentCharacter = 0;
                    enemiesTurn = false;
                    playersTurn = true;
                    
                    //SwapTurns();
                }
               
            }
            
        }
    }
    public void SwapTurns()
    {
        if (playersTurn)
        {
            currentCharacter = initialCurrentCharacter;
            /*for (int i = 0; i < players.Count; i++)
            {
                players[i].GetComponent<Movement>().turn = false;
            }*/
            enemiesTurn = true;
            playersTurn = false;
        }
        if (enemiesTurn)
        {
            currentCharacter = initialCurrentCharacter;
           /* for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].GetComponent<Movement>().turn = false;
            }*/
            playersTurn = true;
            enemiesTurn = false;
        }
    }
}
