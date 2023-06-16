using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnPhaseIndicatorHUD : MonoBehaviour
{
    public TMP_Text turnPhaseText;
    public GameObject deployBar;
    public GameObject attackBar;
    public GameObject fortifyBar;
    public TMP_Text turnOwnerIndicator;
    
    void Update()
    {
        switch (GameManager.gameManager.TurnState)
        {
            case TurnState.Deploy:
                {
                    turnPhaseText.text = "Deploy Phase";
                    deployBar.SetActive(true);
                    attackBar.SetActive(false);
                    fortifyBar.SetActive(false);
                    break;
                }
            case TurnState.Attack:
                {
                    turnPhaseText.text = "Attack Phase";
                    deployBar.SetActive(false);
                    attackBar.SetActive(true);
                    fortifyBar.SetActive(false);
                    break;
                }
            case TurnState.Fortify:
                {
                    turnPhaseText.text = "Fortify Phase";
                    deployBar.SetActive(false);
                    attackBar.SetActive(false);
                    fortifyBar.SetActive(true);
                    break;
                }
        }

        turnOwnerIndicator.text = GameManager.gameManager.PlayerPlaying.color.name + "'s Turn";
    }
}
