using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopsPoopUpHUDSpawner : MonoBehaviour
{
    public TroopsPopUpHUD troopsPopUp;

    void Start()
    {
        foreach( Territory territory in GameManager.gameManager.map.territories)
        {
            territory.troopsPopUpCallback += TroopsPopUp;
        }
    }

    void TroopsPopUp(Territory territory, int change)
    {
        Instantiate(troopsPopUp, territory.transform).Initialise(change);
    }
}
