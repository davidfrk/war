using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continent : MonoBehaviour
{
    [HideInInspector] public List<Territory> territories;
    [SerializeField] private Player owner;
    public Player Owner { get => owner; private set => owner = value; }
    public int bonus = 2;

    void Awake()
    {
        GetTerritories();
    }

    public void GetTerritories()
    {
        territories = new List<Territory>(GetComponentsInChildren<Territory>());
    }

    void Start()
    {
        foreach (Territory territory in territories)
        {
            territory.ownershipUpdateCallback += CheckOwnershipCallback;
        }
        CheckOwnership();
    }

    void CheckOwnershipCallback(Territory territory)
    {
        CheckOwnership();
    }

    void CheckOwnership()
    {
        Debug.Log("Check continet's ownership.");
        Player newOwner = territories[0].Owner;
        foreach (Territory territory in territories)
        {
            if (territory.Owner != newOwner)
            {
                Owner = null;
                return;
            }
        }

        Owner = newOwner;
    }
}
