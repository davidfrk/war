using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryHighlight : MonoBehaviour
{
    void Start()
    {
        Map map = GetComponent<Map>();
        foreach(Territory territory in map.territories)
        {
            territory.territorySelectionCallback += UpdateHighlight;
        }
    }

    void UpdateHighlight(Territory territory)
    {
        if (territory.Selected) territory.renderer.material.color = Color.magenta;
        else territory.renderer.material.color = territory.Owner.color.color;
    }
}
