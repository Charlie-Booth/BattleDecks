using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public CharacterManager CM;
    public GameObject PlayerPrefab;
    public GameObject[] enemyPrefabs;
    public WinLossScreen winLoss;
    public List<GameObject> players;
    public List<GameObject> enemies;
    //public Color thisPlayersColor;
    public SavedData saveFile;
    public Transform[] playerSpawnPoints;
    public Transform[] enemySpawnpoints;
    public TurnManagerRedux turnManager;
    public bool everythingSpawned = false;
    public int enemiesKilled;
    

    //public float countDown;
    private float searchCountDown = 1f;
    private float playerSearchCountDown = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
        CM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CharacterManager>();
        winLoss = CM.gameObject.GetComponent<WinLossScreen>();
        saveFile = CM.saveFile;
      
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!everythingSpawned) // if the players and enemies havent been spawned it will spawn them
        {
            Spawn();
        }
        else
        {
            turnManager.startDoingTurns = true; // the turn manager will then find all of the players and enemies and add them to their list
            if (!EnemiesAlive()) // checks to see if all the enemies are dead
            {
                winLoss.enemiesKilled = enemiesKilled;
                winLoss.Win();
            }
            if (!PlayersAlive())// checks to see if all the players are dead
            {
                
                winLoss.enemiesKilled = enemiesKilled;
                winLoss.Loss();
            }
        }
    }
    void SpawnPlayers()
    {
        Debug.Log("attempting to spawn Players");
        for (int i = 0; i < CM.characters.Count; i++) // spawns each of the characters in the character manager and sets their stats and what they look like, 
        {
            //Debug.Log("entered for Loop");
            Transform sp = playerSpawnPoints[i];
            GameObject character = Instantiate(PlayerPrefab, sp.position, Quaternion.identity);
            PlayerController player = character.GetComponent<PlayerController>();
            Movement playerMovement = character.GetComponent<PlayerMovement>();
            
            player.maxDamage = CM.characters[i].maxDamage;
            if(i == 0)
            {
                playerMovement.turn = true;
            }
           
            player.maxHealth = CM.characters[i].maxHealth;
            player.currentHealth = CM.characters[i].currentHealth;
            player.minDamage = CM.characters[i].minDamage;
            player.maxDamage = CM.characters[i].maxDamage;
            player.name = CM.characters[i].name;
            player.charStats = CM.characters[i].charStats;
            player.experianceNeeded = CM.characters[i].exp;
            player.currentEXP = CM.characters[i].currentExp;
            player.characterNumber = CM.characters[i].characterNumber;
            character.GetComponent<MeshRenderer>().material.color = CM.characters[i].playerColor;
            player.thisPlayersColour= CM.characters[i].playerColor;           
            playerMovement.move = CM.characters[i].moveDistance;
            playerMovement.dashMove = CM.characters[i].DashDistance;
            playerMovement.attackRange = CM.characters[i].attackDistance;
            playerMovement.initialActions = CM.characters[i].actions;
            playerMovement.actions = CM.characters[i].actions;
            switch (player.charStats.playerClass)
            {
                case NewCharacter.Class.Archer:
                    playerMovement.archer = true;
                    break;
                case NewCharacter.Class.Thief:
                    playerMovement.Thief = true;
                    break;
                case NewCharacter.Class.Warrior:
                    playerMovement.Warrior = true;
                    break;
            }
            players.Add(character);
            turnManager.players.Add(character);// adds the character to the turn manager list




        }
    }
    void SpawnEnemies()
    {
        for (int i = 0; i <saveFile.amountOfEnemies; i++)// spawns the amount of enemies that was on the previous card from a prefab
        {
            Transform sp = enemySpawnpoints[i];
            GameObject newEnemy = Instantiate(enemyPrefabs[Random.Range(0,enemyPrefabs.Length)], sp.position, Quaternion.Euler(0,180,0));
            enemies.Add(newEnemy);
            turnManager.enemies.Add(newEnemy);
            
        }
    }
    void Spawn() // spawns all the necessary units
    {
        
        SpawnPlayers();
      SpawnEnemies();
            
       
        everythingSpawned = true;
    }

    bool EnemiesAlive()
    {

        searchCountDown -= Time.deltaTime;// checks to see if any enemies are alive every second
        if (searchCountDown <= 0)
        {
            searchCountDown = 1f;// makes the timer loop

            if (GameObject.FindGameObjectWithTag("NPC") == null)// checks to see enemy tags
            {

                return false;// returns false as there are no enemies alive
            }
            else
            {
                //Debug.Log("EnemyFound");
            }
        }
        return true;
    }
    bool PlayersAlive() // checks every second to see if there are any enemies alive on the level
    {
        playerSearchCountDown -= Time.deltaTime;
        if (playerSearchCountDown <= 0)
        {
            playerSearchCountDown = 1f;

            if(GameObject.FindGameObjectWithTag("Player") == null)
            {
                return false;
            }
            else
            {
                //Debug.Log("PlayerFound");
            }
        }
        //Debug.Log("No players");
        return true;
    }
   
}
