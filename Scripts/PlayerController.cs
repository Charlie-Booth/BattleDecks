using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public GameObject attacker;
    public float maxHealth;
    public float currentHealth;
    public float minDamage;
    public float maxDamage;
    public string name;
    public NewCharacter charStats;
    public float experianceNeeded;
    public float currentEXP;
    public int characterNumber;
    public Movement playerMovement;
    public CharacterManager CM;
    public TurnManagerRedux turnManagerRedux;
    public TMP_Text nameText;
    public Color thisPlayersColour;
    public Slider HealthBar;
    public GameObject particleEffect;
    public int numberInManagerList;
    public GameObject swordAndShield;
    public GameObject BowAndArrow;
    public GameObject Dagger;
    public GameObject ElfEars;
    public GameObject DwarfBeard;
    public Animator animator;
    public GameObject damageText;
    BringUpCharacterMenu canvas;
    MeshRenderer meshrenderer;
    Color origColour;
    float flashTime = 1.5f;
    bool dead = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //initialises key components for the player controller
        playerMovement = this.gameObject.GetComponent<Movement>();
        CM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CharacterManager>();
        turnManagerRedux = GameObject.FindGameObjectWithTag("Level").GetComponent<TurnManagerRedux>();
        meshrenderer = GetComponent<MeshRenderer>();
        origColour = meshrenderer.material.color;
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<BringUpCharacterMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        nameText.text = name;
        HealthBar.value = calculateHealth();
        if (currentHealth <= 0&&!dead)
        {
            Die();
            Instantiate(particleEffect, gameObject.transform.position, Quaternion.Euler(-90, 0, 0));
            gameObject.SetActive(false);
            dead = true;
        }
        // below will set weapons and race attributes to true
        if (charStats.playerClass == NewCharacter.Class.Archer)
        {
            BowAndArrow.SetActive(true);
            Dagger.SetActive(false);
            swordAndShield.SetActive(false);
            animator.SetBool("AttackWithBow", true);
            
        }
        else if (charStats.playerClass == NewCharacter.Class.Warrior)
        {
            BowAndArrow.SetActive(false);
            Dagger.SetActive(false);
            swordAndShield.SetActive(true);
            animator.SetBool("AttackWithSword", true);

        }
        else if(charStats.playerClass == NewCharacter.Class.Thief)
        {
            BowAndArrow.SetActive(false);
            Dagger.SetActive(true);
            swordAndShield.SetActive(false);
            animator.SetBool("AttackWithDagger", true);
            
        }
        if(charStats.race == NewCharacter.Race.Dwarf) //ADDED BECAUSE OF FEEDBACK, players now have race specific looks to more easily distinguish the players troops mid battle
        {
            DwarfBeard.SetActive(true);
            ElfEars.SetActive(false);
        }
        if(charStats.race == NewCharacter.Race.Elf)
        {
            DwarfBeard.SetActive(false);
            ElfEars.SetActive(true);
        }
        if(charStats.race == NewCharacter.Race.Human)
        {
            DwarfBeard.SetActive(false);
            ElfEars.SetActive(false);
        }
    }
    public void TakeDamage()
    {
        //attacker.GetComponent<Movement>().actions -= 1;
        Debug.Log(gameObject + " takenDamage");
        
    }
    public void DoDamage(EnemyController enemy) // will deal damage to the enemy 
    {
        float damage = Random.Range(minDamage, maxDamage);
        if (enemy != null)
        {
            enemy.currentHealth -= damage;
            playerMovement.actions -= 1;
            CineMachineShake.Instance.ShakeCamera(2f, 0.2f);
            StartCoroutine(DamageAnim());

           
        }
        
        Debug.Log("Attacking " + enemy);
    }
    public void Die()// will remove the player from the manager list and turn manager and turn off their character button
    {
        //TurnManager.RemoveUnit(playerMovement);

        CM.characters.Remove(CM.characters[numberInManagerList]);
        canvas.CharacterButtons[numberInManagerList].SetActive(false);
        if (turnManagerRedux.players.Count > 0)
        {
            turnManagerRedux.players.Remove(turnManagerRedux.players[numberInManagerList]);
        }
        FindObjectOfType<AudioManager>().Play("Death");


    }
    IEnumerator DamageAnim() // Test function for possible future animations
    {
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("Attack", false);
        yield break;
    }
    float calculateHealth()
    {
        return currentHealth / maxHealth;
    }
    public IEnumerator FlashStart() // the player will flash red and their original colour when taking damage
    {
        meshrenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        meshrenderer.material.color = origColour;
        yield return new WaitForSeconds(0.3f);
        meshrenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        meshrenderer.material.color = origColour;

    }
    void FlashStop()// will set the player back to their original color
    {
        meshrenderer.material.color = origColour;
    }

}
