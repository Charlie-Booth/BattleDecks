using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardManager : MonoBehaviour
{
    public List<GameObject> CardDeck;// holds the values for all the active and inactive cards
    public List<GameObject> CardsInPlay;
    public List<int> CardsInPlayNumber;
    public List<GameObject> cardSpawnPoints;
    public GameObject endCard;

    public GameObject CardUI;
    public GameObject CardUIActive;

    public SavedData Data; // holds the savefile
    public TraderMenu traderMenu;
    
    public GameObject ui;

    public TMP_Text CardText;

    public int spNum;
    public int cardNumber;
    public int currentCard = -1;
    public int amountOfCards;
    public bool spawned = false;


    //TO DO CHANGE THE WAY CARDS SPAWN IN
    private void Start()
    {
        Time.timeScale = 1f;
        FindObjectOfType<AudioManager>().Play("CardTableMusic");
    }
    
    public void PickNextCard() // will pick a random card from the card deck and spawn it at a spawnpoint
    {
        cardNumber = Random.Range(0, CardDeck.Count);

        for (int i = 0; i < CardDeck.Count; i++)
        {
            if (i == cardNumber)
            {
                StartCoroutine(Wait(2f));
                Transform spawnpoint = cardSpawnPoints[spNum].transform;
                GameObject spawnedCard = Instantiate(CardDeck[i], spawnpoint);
                Debug.Log("Removing" + spawnedCard);
                CardsInPlay.Add(spawnedCard);
                CardsInPlayNumber.Add(cardNumber);
                CardDeck.Remove(CardDeck[i]);
                spNum++;
            }
        }
    }

    public void SpawnCards() // will spawn Cards for the amount of card to spawn on the table
    {
        for (int i = 0; i < amountOfCards; i++)
        {

            PickNextCard();
        }
    }
    public void SpawnCardsFromData()// will spawn the cards from the card deck in line with the amount of cards that need spawning
    {
        for (int i = 0; i < Data.cardsPlayed.Count; i++)
        {
            Transform spawnpoint = cardSpawnPoints[spNum].transform;
            GameObject spawnedCard = Instantiate(CardDeck[Data.cardsPlayed[i]],spawnpoint);
            CardsInPlay.Add(spawnedCard);
            CardsInPlayNumber.Add(Data.cardsPlayed[i]);
            CardDeck.Remove(spawnedCard);
            spNum++;
        }
    }

    public void Update()
    {
        
       
        if (!Data.firstTurnDone)
        {
            if (!spawned) //will remove cards and set the first card
            {
                Data.currentCard = -1;
                RemoveLastCards();              
                SpawnCards();
                SetCard();
                Transform sp = cardSpawnPoints[spNum].transform;
                Instantiate(endCard, sp);
                CardsInPlay.Add(endCard);
                spawned = true;
            }
        }
        else // will spawn cards from the ssaved data
        {
            currentCard = Data.currentCard;
            
            if (!spawned)
            {
                SpawnCardsFromData();
                SetCard();
                Transform sp = cardSpawnPoints[spNum].transform;
                Instantiate(endCard, sp);
                CardsInPlay.Add(endCard);
                spawned = true;
                for (int i = 0; i < currentCard; i++)
                {
                    CardsInPlay[i].gameObject.GetComponent<Animator>().SetBool("Flip", true);
                }
            }
        }
    }
    public void RemoveLastCards() // removes the cards from the savefile
    {
        Data.cardsPlayed.Clear();
    }
    public void FlipCard()// flips the card over 
    {
        CardsInPlay[currentCard].gameObject.GetComponent<Animator>().SetBool("Flip", true);
        //CardsInPlay[currentCard].gameObject.transform.rotation = Quaternion.Euler(0, 0, 180);
    }
    public void ChooseNextCardInPlay() //checks each card and will set the current card for the card UI
    {
        currentCard++;
        Data.currentCard++;
        FlipCard();
        CardsInPlay[currentCard].gameObject.GetComponent<CardEvent>().ButtonCheck();
        CardUIActive.GetComponent<CardDescription>().currentCard = CardsInPlay[currentCard].gameObject.GetComponent<CardEvent>();
        CardUI.SetActive(true);
        FindObjectOfType<AudioManager>().Play("CardFlip");
        SetCard();

    }
    public void ButtonPress()// will make the current cards button press
    {
        CardsInPlay[currentCard].GetComponent<CardEvent>().CardContinue();
        
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }
    void SetCard()// sets the card for the UI at the top of the screen
    {
        int visualCurrentCard = currentCard + 1;
        int visualTotalCard = amountOfCards + 1;
        CardText.text =  "Card: " +visualCurrentCard.ToString() + " / " + visualTotalCard.ToString();
    }
}
