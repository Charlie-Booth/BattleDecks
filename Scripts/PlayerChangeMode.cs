using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerChangeMode : MonoBehaviour
{
    public List<GameObject> players;
    public List<PlayerMovement> playerMovements;
    public GameObject Level;
    public PlayerMovement currentPlayer;
    public bool addedMovement = false;
    public int ranThrough = 0;
    public TMP_Text TurnsText;

    //FINISH THIS TO GET BUTTON CHANGES WORKING
    private void Update()
    {
        Level = GameObject.FindGameObjectWithTag("Level");
        players = Level.GetComponent<PlayerSpawn>().players;
        if (!addedMovement&&players.Count > 0)
        {
            AddMovement();// finds all the player movement scripts
        }
        for (int i = 0; i < playerMovements.Count; i++)
        {
            if (playerMovements[i].turn == true)
            {
                currentPlayer = playerMovements[i]; // finds the current player
            }
        }
        TurnsText.text = "Actions "+ currentPlayer.actions.ToString() + "/" + currentPlayer.initialActions.ToString(); // shows the amount of turns the player has in the top left
    }
    public void AttackingSelect()// button function, resets some character values and changes them to be attacking
    {       
        currentPlayer.movingSelected = false;
        currentPlayer.attackingSelected = true;
        currentPlayer.foundAttackTiles = false;
        currentPlayer.foundMoveTiles = false;
        
        
    }
    public void MovementSelect() //button function, resets some character values and changes them to be moving
    {
        //currentPlayer.moving = true;
        currentPlayer.attackingSelected = false;
        currentPlayer.movingSelected = true;
        currentPlayer.foundMoveTiles = false;
        currentPlayer.foundAttackTiles = false;
        //currentPlayer.moving = false;

    }
    public void Wait()
    {
        currentPlayer.actions -= currentPlayer.actions; // removes all the player actions as they are forfeiting a turn
    }
    public void SelectMovement()
    {
        MovementSelect();
    }
    public void AddMovement()
    {
        for (int i = 0; i < players.Count; i++)
        {
            playerMovements.Add(players[i].gameObject.GetComponent<PlayerMovement>());
        }
        addedMovement = true;
        
        
    }
    public IEnumerator WaitForTimer() // TEST FUNCTION
    {
        currentPlayer.foundMoveTiles = false;

        yield return new WaitForSeconds(0.5f);
       
       
    }
}
