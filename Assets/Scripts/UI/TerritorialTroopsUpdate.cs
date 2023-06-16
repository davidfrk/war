using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TerritorialTroopsUpdate : MonoBehaviour
{
    TMP_Text text;

    void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
        GetComponent<Territory>().troopsUpdateCallback += UpdateTroopsDisplay;
    }

    
    public void UpdateTroopsDisplay(Territory territory)
    {
        text.text = territory.Troops.ToString();
    }
}
