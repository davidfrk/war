using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Map))]
public class DisplayAdjacencyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Update"))
        {
            (target as Map).GetTerritories();
        }
    }

    void OnSceneGUI()
    {
        Map map = target as Map;

        foreach(Territory territory in map.territories)
        {
            foreach(Territory neighbour in territory.neighbouringTerritories)
            {
                Handles.DrawLine(territory.transform.position, neighbour.transform.position);
            }
        }
    }
}
