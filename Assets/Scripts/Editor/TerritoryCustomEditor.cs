using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Territory))]
public class TerritoryCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Add Adjacencies"))
        {
            AddAdjacencies();
        }
    }

    public void AddAdjacencies()
    {
        //Create list of selected territories
        List<Territory> selectedTerritories = new List<Territory>();
        foreach (GameObject gameObject in Selection.gameObjects)
        {
            Territory newTerritory = gameObject.GetComponent<Territory>();
            if (newTerritory != null) selectedTerritories.Add(newTerritory);
        }

        for (int i = 0; i < selectedTerritories.Count - 1; i++)
        {
            for (int j = i + 1; j < selectedTerritories.Count; j++)
            {
                Territory a = selectedTerritories[i];
                Territory b = selectedTerritories[j];

                a.SwapAdjacency(b);
                b.SwapAdjacency(a);
            }
        }
    }
}
