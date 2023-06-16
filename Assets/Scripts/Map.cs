using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Map : MonoBehaviour
{
    public List<Territory> territories;
    public List<Continent> continents;

    void Awake()
    {
        GetTerritories();
    }

    public void GetTerritories()
    {
        territories = new List<Territory>(GetComponentsInChildren<Territory>());
        continents = new List<Continent>(GetComponentsInChildren<Continent>());
    }
}
