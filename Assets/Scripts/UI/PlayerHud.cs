using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHud : MonoBehaviour
{
    public TMP_Text territoriesCounter;
    public TMP_Text troopsCounter;
    public Image avatarBackground;
    public GameObject turnArrow;
    public GameObject typeOfPlayerPanel;
    public TMP_Text typeOfPlayerText;

    [HideInInspector] public Player owner;

    void Start()
    {
        Color color = owner.color.color;
        color.a = 0.7f;
        avatarBackground.color = color;

        UpdateTypeOfPlayer();
    }

    void Update()
    {
        UpdateTroopsCounter();
    }

    void UpdateTroopsCounter()
    {
        territoriesCounter.text = owner.territoryCount.ToString();
        troopsCounter.text = owner.troopCount.ToString();
        turnArrow.SetActive(owner.isPlaying);
    }

    void UpdateTypeOfPlayer()
    {
        switch (owner.typeOfPlayer)
        {
            case TypeOfPlayer.You:
                {
                    typeOfPlayerText.text = "You";
                    typeOfPlayerPanel.SetActive(true);
                    break;
                }
            case TypeOfPlayer.Bot:
                {
                    typeOfPlayerText.text = "Bot";
                    typeOfPlayerPanel.SetActive(true);
                    break;
                }
            case TypeOfPlayer.Opponent:
                {
                    typeOfPlayerPanel.SetActive(false);
                    break;
                }
        }
    }
}
