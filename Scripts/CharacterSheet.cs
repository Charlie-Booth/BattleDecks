using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterSheet : MonoBehaviour
{
    //has all the fields on the character sheet
    public TMP_Text CharacterName;
    public TMP_Text Class;
    public TMP_Text Race;
    public TMP_Text Health;
    public TMP_Text exp;
    public TMP_Text AverageDamage;
    public Image Icon;
    public TMP_InputField inputField;
    public CharacterManager CM;
    public Slider healthBar;

    private void Start()
    {
        CM = GameObject.FindGameObjectWithTag("GameManager").gameObject.GetComponent<CharacterManager>();
    }
    public void ChangePlayerName()// used to change the players name from an input field
    {
        CM.characters[CM.CurrentCard].name = inputField.text;
        CharacterName.text = CM.characters[CM.CurrentCard].name;
    }
}
