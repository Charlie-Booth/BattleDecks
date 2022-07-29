using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyController : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float minDamage;
    public float maxDamage;
    public bool dead;
    public Movement thisUnitMovement;
    public PlayerSpawn playerSpawn;
    public TurnManagerRedux turnManagerRedux;
    public int numberInManagerList;
    public string enemyName;
    public TMP_Text enemyNameText;
    public Slider healthBar;
    public GameObject weapon;
    public AudioSource death;
    public GameObject particleEffect;
   


    void Start()
    {
        currentHealth = maxHealth;
        thisUnitMovement = this.gameObject.GetComponent<Movement>();
        playerSpawn = GameObject.FindGameObjectWithTag("Level").GetComponent<PlayerSpawn>();
        turnManagerRedux = GameObject.FindGameObjectWithTag("Level").GetComponent<TurnManagerRedux>();
    }


    // Update is called once per frame
    void Update()
    {
        enemyNameText.text = enemyName;
        healthBar.value = calculateHealth();// calculates the enemies health
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die() // will remove the enemy from the level list and remove the enemy from the turn manager
    {
        playerSpawn.enemiesKilled += 1;
        //TurnManager.RemoveUnit(thisUnitMovement);
        if (turnManagerRedux.enemies.Count > 0)
        {
            turnManagerRedux.enemies.Remove(turnManagerRedux.enemies[numberInManagerList]);
        }
        Instantiate(particleEffect, gameObject.transform.position, Quaternion.Euler(-90, 0, 0));
        FindObjectOfType<AudioManager>().Play("Death");
        Destroy(gameObject);
    }
    public void AttackEnemy(PlayerController player) // will attack the player using the enemy controllers damage values 
    {
        PlayerMovement playerMovement = player.gameObject.GetComponent<PlayerMovement>();
        Tile currentTile = playerMovement.currentTile;// gets the Tile the player is currently stood on
        if (thisUnitMovement.actions > 0)
        {
            thisUnitMovement.actions -= 1;// will remove an action from the enemy
            float damage = Random.Range(minDamage, maxDamage);
            currentTile.target = true;
            player.currentHealth -= damage;
            DamageIndicator damageIndicator = Instantiate(player.damageText, player.transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
            CineMachineShake.Instance.ShakeCamera(2f, 0.2f);// shakes the camera
            //damageIndicator.SetDamageText(damage);
            StartCoroutine(player.FlashStart());// makes the player flash
            player.CM.characters[player.characterNumber].currentHealth -= damage;
            
        }
        //CineMachineShake.Instance.ShakeCamera(7f, 0.2f);
    }
    float calculateHealth()
    {
        return currentHealth / maxHealth;
    }
}
