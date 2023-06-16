using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Territory : MonoBehaviour
{
    public List<Territory> neighbouringTerritories;

    [SerializeField]
    private Player owner;
    public Player Owner
    {
        get => owner;
        set
        {
            owner = value;
            if (owner == null)
            {
                renderer.material.color = Color.white;
            }
            else
            {
                renderer.material.color = owner.color.color;
            }
            ownershipUpdateCallback?.Invoke(this);
        }
    }

    [SerializeField]
    private int troops = 1;
    public int Troops {
        get => troops;
        set {
            int change = value - troops;
            troops = value;
            owner?.TroopsChangedCallback(this, change);
            troopsUpdateCallback?.Invoke(this);
        }
    }

    [SerializeField]
    private bool selected = false;
    public bool Selected
    {
        get => selected;
        set
        {
            selected = value;
            territorySelectionCallback?.Invoke(this);
        }
    }

    public delegate void TerritoryCallback(Territory territory);
    public TerritoryCallback territorySelectionCallback;
    public TerritoryCallback troopsUpdateCallback;
    public TerritoryCallback ownershipUpdateCallback;

    public delegate void TroopsPopUpCallback(Territory territory, int change);
    public TroopsPopUpCallback troopsPopUpCallback;

    [HideInInspector] public new Renderer renderer;

    void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    void Start()
    {
        troopsUpdateCallback?.Invoke(this);
    }

    public void DeployTroops(int amount)
    {
        Troops += amount;
        troopsPopUpCallback?.Invoke(this, amount);
    }

    public void Attack(Territory target, int attackingTroops)
    {
        int attackingForce = Mathf.Min(attackingTroops, troops - 1);
        if (attackingForce < 1) return;

        int attackerLoss, defenderLoss;
        bool attackerVictory = AttackHelper.AttackDiceByDice(attackingForce, target.Troops, out attackerLoss, out defenderLoss);

        if (attackerVictory)
        {
            target.Owner.LoseTerritory(target);
            Troops = 1;

            target.Troops = attackingForce - attackerLoss;
            Owner.ReceiveTerritory(target);
        }
        else
        {
            Troops -= attackerLoss;
            target.Troops -= defenderLoss;
        }

        if (attackerLoss > 0) troopsPopUpCallback?.Invoke(this, -attackerLoss);
        if (defenderLoss > 0) target.troopsPopUpCallback?.Invoke(target, -defenderLoss);
    }

    public bool IsAdjacent(Territory territory)
    {
        return neighbouringTerritories.Contains(territory);
    }

    public bool IsConnected(Territory territory)
    {
        return ConnectedTerritories().Contains(territory);
    }

    public List<Territory> ConnectedTerritories()
    {
        List<Territory> connectedTerritories = new List<Territory>();
        connectedTerritories.Add(this);
        int i = 0;

        while(i < connectedTerritories.Count)
        {
            foreach(Territory newTerritory in connectedTerritories[i].neighbouringTerritories)
            {
                if (newTerritory.Owner == Owner && !connectedTerritories.Contains(newTerritory))
                {
                    connectedTerritories.Add(newTerritory);
                }
            }

            i++;
        }

        Debug.Log(connectedTerritories.Count + " connected territories.");
        return connectedTerritories;
    }

    public void SwapAdjacency(Territory otherTerritory)
    {
        if (neighbouringTerritories.Contains(otherTerritory))
        {
            neighbouringTerritories.Remove(otherTerritory);
        }
        else
        {
            neighbouringTerritories.Add(otherTerritory);
        }
    }
}
