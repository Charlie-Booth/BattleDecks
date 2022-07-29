using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// used on the main menu to start and quit the game
public class MainMenu : MonoBehaviour
{
    public SavedData dataFile;
    public void Start()
    {
        FindObjectOfType<AudioManager>().Play("MainMenuMusic");
    }
    public void PlayGame()
    {
        dataFile.firstTurnDone = false;
        dataFile.amountOfCharacters = 2;
        dataFile.characters.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
