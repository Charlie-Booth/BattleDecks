using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{ //Episode 1 out of 6(all 6 were used) of the tutorial series to make this found here https://www.youtube.com/watch?v=cX_KrK8RQ2o by Games Programming academy "Unity Tutorial - Tactics Movement - Part 1", uploaded Nov 9 2017
    // Ep 2 https://www.youtube.com/watch?v=cK2wzBCh9cg
    //Ep 3 https://www.youtube.com/watch?v=2NVEqBeXdBk
    //Ep 4 https://www.youtube.com/watch?v=mTNL0EXU7kU
    //Ep 5 https://www.youtube.com/watch?v=W2OQbHeQs7U
    //Ep 6 https://www.youtube.com/watch?v=3M3FcJ4r0GE
    public PlayerController playerController;
    public Movement enemyMove;
    public EnemyController enemyController;
    public GameObject indicatorArrow;
    public TurnManagerRedux turnManagerRedux;
    public bool foundMoveTiles = false;
    public bool foundAttackTiles = false;
    public LayerMask noInteract;

    
    


    // Start is called before the first frame update
    void Start()
    {
        Initialise();
        turnManagerRedux = GameObject.FindGameObjectWithTag("Level").GetComponent<TurnManagerRedux>(); // finds the turn manager
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward);

        if (!turn)// if its not the players turn
        {
            foundMoveTiles = false;// will reset bools
            foundAttackTiles = false;
            indicatorArrow.SetActive(false);// will turn off the indicator arrow if it is the players turn
            AssignCurrentUnit();// will assign the current tile the unit is standing on unit
            //FindOutOfSightTiles();
            return;// stops the update from looping through the rest of the code
        }
        if (turn)
        {
            //FindOutOfSightTiles()
            indicatorArrow.SetActive(true);// sets that it is this players turn
            if(actions <= 0)
            {
                // TurnManager.EndTurn();

                EndTurn();// ends the players turn
                //actions = initialActions;
                turnManagerRedux.currentCharacter++;// will increase the index for the current character
            }
            if (!moving)// if the player isnt moving 
            {
                if (movingSelected)
                {
                    foundAttackTiles = false;// disables finding attack tiles
                    if (!foundMoveTiles)
                    {

                        RemoveSelectableTiles();// removes any old selectable tiles (stops framerate running slowly)
                        RemoveAttackTiles();// removes any attack tiles
                        for (int i = 0; i < tiles.Length; i++)
                        {
                            tiles[i].GetComponent<Tile>().attackingSelected = false;// tells all tiles in the array that the player is now not attacking( this is because attacking causes different behaviour)
                        }
                        FindSelectableTiles();//finds any selectable tiles
                        if (actions > 1)// will run through if the player has the actions to be able to dash
                        {
                            FindDashableTiles();
                        }
                        
                        foundMoveTiles = true;// stops the codr from looping
                    }


                    

                }
                if (attackingSelected)// works the same as above but is the inverse
                {
                    RemoveSelectableTiles();
                    foundMoveTiles = false;
                    if (!foundAttackTiles)
                    {
                        RemoveSelectableTiles();
                        RemoveDashableTiles();
                        for (int i = 0; i < tiles.Length; i++)
                        {
                            tiles[i].GetComponent<Tile>().attackingSelected = true;
                        }
                        FindMeleeTiles();
                        
                        foundAttackTiles = true;
                        
                    }


                }


                CheckMouse();// constantly checks the mouse to see if the player has made an input
            }
            else
            {
                foundMoveTiles = false;// resets finding tiles
                foundAttackTiles = false;
                Move();// player will move
            }
        }
        
       
    }

    void CheckMouse()
    {
        if (Input.GetMouseButtonUp(0)) // checks what the player is hitting 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 1000f ,~noInteract))
            {
                if(hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();
                    if (t.selectable)// will move to the tile
                    {
                        dashing = false;
                        //move to target
                        MoveToTile(t);
                        //actions -= 1;
                    }
                    if (t.dashSelectable)// will dash to the tile
                    {
                        dashing = true;
                        MoveToTile(t);
                        //actions -= 2;
                    }
                    if (t.attackSelectable) // will attack the unit currently stood on the attack tile
                    {
                        Debug.Log(t);
                        dashing = false;
                        StoreTile(t);
                        if (t.unit != null)
                        {
                            t.target = true;
                        }
                        playerController.DoDamage(enemyController);// player deals damage to the enemy controller
                        if (Warrior)
                        {
                            FindObjectOfType<AudioManager>().Play("SwordSwing"); // plays an audio clip
                        }
                        if (archer)
                        {
                            FindObjectOfType<AudioManager>().Play("BowShoot");
                        }
                        if (Thief)
                        {
                            FindObjectOfType<AudioManager>().Play("DaggerSound");
                        }
                        
                        
                       
                        //playerController.DoDamage(t.enemyController);
                        Debug.Log("HitAttack");
                        
                        //enemyController = enemyMove.gameObject.GetComponent<EnemyController>();
                        if (t.unit != null)
                        {
                            dashing = false;
                            t.target = true;
                            
                            //playerController.DoDamage(enemyController);
                            //actions -= 1;
                        }
                       
                    }
                    
                }
                if(hit.collider.tag == "NPC")// ADDED DUE TO FEEDBACK: players can now also select the enemy on an attacking tile to deal damage to him
                {
                    Movement enemy = hit.collider.GetComponent<Movement>();
                    enemyController = hit.collider.GetComponent<EnemyController>();
                    if (enemy.currentTile.attackSelectable)
                    {
                        playerController.DoDamage(enemyController);
                        if (Warrior)
                        {
                            FindObjectOfType<AudioManager>().Play("SwordSwing");
                        }
                        if (archer)
                        {
                            FindObjectOfType<AudioManager>().Play("BowShoot");
                        }
                        if (Thief)
                        {
                            FindObjectOfType<AudioManager>().Play("DaggerSound");
                        }
                    }
                }
                if (hit.collider.tag == "Player") //unused but may use this in the future for extra abilites
                {
                    Debug.Log("HitPlayer");
                    Movement player = hit.collider.GetComponent<Movement>();
                    //TurnManager.ChangeTurn(player);// close but no cigar
                    

                }
            }
        }

    }
    public void StoreTile(Tile t) //necessary conversion
    {
        enemyController = t.enemyController;
        enemyMove = t.unit;
    }
}
