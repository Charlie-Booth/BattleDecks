using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonValues : MonoBehaviour
{
    // used to hold all the values of the buttons
    public List<GameObject> PlayerButtons;
    public GameObject PlayerButtonsEmptyGO;
    public TMP_Text ItemName;
    public TMP_Text Description;
    public TMP_Text Cost;
    public Image itemImage;
    public Item item;
}
