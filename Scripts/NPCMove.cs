using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : Movement
{
    //Episode 1 out of 6(all 6 were used) of the tutorial series to make this found here https://www.youtube.com/watch?v=cX_KrK8RQ2o by Games Programming academy"Unity Tutorial - Tactics Movement - Part 1", uploaded Nov 9 2017
    // Ep 2 https://www.youtube.com/watch?v=cK2wzBCh9cg
    //Ep 3 https://www.youtube.com/watch?v=2NVEqBeXdBk
    //Ep 4 https://www.youtube.com/watch?v=mTNL0EXU7kU
    //Ep 5 https://www.youtube.com/watch?v=W2OQbHeQs7U
    //Ep 6 https://www.youtube.com/watch?v=3M3FcJ4r0GE
    public GameObject target;
    public EnemyController thisEnemy;
    public TurnManagerRedux turnManagerRedux;
    public bool canNowAttackPlayer = false;
    public bool startedCo = false;
    public bool pathCalculated = false;
    public int maxAmountOfRunThroughs;
    public AudioSource weaponSource;
    public AudioClip weaponClip;
   
    // Start is called before the first frame update
    void Start()
    {
        Initialise();

        thisEnemy = this.gameObject.GetComponent<EnemyController>(); // finds the enemy controller 
        turnManagerRedux = GameObject.FindGameObjectWithTag("Level").GetComponent<TurnManagerRedux>();
        maxAmountOfRunThroughs = actions;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward);
        if (!turn)
        {
            AssignCurrentUnit(); // will assign the tile the unit is currently stood ons unit
            return;
        }
        if (actions > 0)
        {
            if (!moving)
            {

                FindNearestTarget(); // finds the nearest player target
                CalculatePath();// calculates a path to the nearst target using A*(for movement)
                

                if (movingSelected)
                {
                    FindSelectableTiles();
                    if (dashing|| actions> 1&& !attackingSelected)
                    {
                        FindDashableTiles();
                    }
                } //FindDashableTiles();

                if (attackingSelected&& maxAmountOfRunThroughs > 0) // added on as this was running through too many times
                {
                    maxAmountOfRunThroughs -= 1;
                    AttackPlayer();
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
                    //StartCoroutine(SelectTileTime());
                }


                actualTargetTile.target = true;
            }
            else
            {

                Move();
            }
        }
        else
        {
            turnManagerRedux.currentCharacter++;
            maxAmountOfRunThroughs = 3;
            EndTurn();
        }
        
    }
    void CalculatePath()
    {
        Tile targetTile = GetTargetTile(target);

        float distance = Vector3.Distance(transform.position, targetTile.transform.position);

        if (distance<=attackRange) // will check if the distance to the target is less than attack range
        {
            attackingSelected = true;
            movingSelected = false;
            //do attack
           
            //FindMeleeTiles();
            
            

        }
        else if(distance > dashMove )// will check if the distance is greater than the dash move
        {
            Debug.Log(distance + "is Greater than" + dashMove);
            movingSelected = true;
            attackingSelected = false;
            float attackRangePlusMove = move + attackRange;
            if(distance <= attackRangePlusMove)// checks if the distance is less the attacking range plus the movement range and will move to attack, otherwise it will dash
            {
                Debug.Log(distance + "is less than" + attackRangePlusMove);
                dashing = false;
                FindSelectableTiles();
                FindPath(targetTile);
            }
            else
            {
                Debug.Log(distance + "is more than" + dashMove);
                dashing = true;
                FindDashableTiles();
                FindPath(targetTile);
            }
        }
        else if (distance <= dashMove)// otherwise it just moves
        {
            Debug.Log(distance + "is Greater than" + dashMove);
            movingSelected = true;
            attackingSelected = false;
            float attackRangePlusMove = move + attackRange;
            dashing = false;
            FindSelectableTiles();
            FindPath(targetTile);
        }
       
        
        

        //actions -= 1;
    }

    void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player"); // puts all the players into an array

        GameObject nearest = null;
        float distance = Mathf.Infinity;
        foreach(GameObject obj in targets)
        {
            float d = Vector3.Distance(transform.position, obj.transform.position); // will work out the distance between the enemy and the player

            if (d < distance) // will run through until it finds the nearest gameobject
            {
                distance = d;
                nearest = obj;
            }
        }

        target = nearest;
    }
    public IEnumerator SelectTileTime(PlayerController player)
    {
       
        player.gameObject.GetComponent<PlayerMovement>().currentTile.GetComponent<Renderer>().material.color = Color.white;
        FindMeleeTiles();// finds tiles it can attack
        yield return new WaitForSeconds(2f);
        canNowAttackPlayer = true;
        
    }
    public void AttackPlayer()
    {
        PlayerController player = target.GetComponent<PlayerController>();
        Debug.Log("Attacking Player");
        
            StartCoroutine(SelectTileTime(player));
            startedCo = true;
        if(player== null) // if there is no player then it will get the nearest player
        {
            FindNearestTarget();
        }
        if (actions > 0) // will attack the player if it has actions left
        {
            thisEnemy.AttackEnemy(player);
        }
        
    }
}
