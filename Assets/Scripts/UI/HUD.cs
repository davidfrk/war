using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public TMP_Text turnCounter;
    public PlayerHud playerHUD;
    public Transform playerHudTransform;
    public Image turnTimerBar;

    void Start()
    {
        foreach (Player player in GameManager.gameManager.players)
        {
            PlayerHud hud = Instantiate(playerHUD, playerHudTransform);
            hud.owner = player;
        }
    }

    void Update()
    {
        turnCounter.text = "Turn: " + GameManager.gameManager.turn;

        //Update TurnTimerBar HUD
        if (GameManager.gameManager.PlayerPlaying != null)
        {
            

            float percentOfTurnTime = GameManager.gameManager.ElapsedTurnTime / GameManager.gameManager.turnDuration;
            percentOfTurnTime = Mathf.Clamp01(percentOfTurnTime);

            turnTimerBar.rectTransform.localScale = new Vector3(percentOfTurnTime, 1.0f, 1.0f);
            turnTimerBar.color = GameManager.gameManager.PlayerPlaying.color.color;
        }
        else
        {
            turnTimerBar.transform.localScale = new Vector3(0.0f, 1.0f, 1.0f);
        }
    }
}
