using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfPlayer
{
    You,
    Bot,
    Opponent,
}

public class Player : MonoBehaviour
{
    public PlayerColor color;
    public TypeOfPlayer typeOfPlayer;
    public List<Territory> territories;
    public int territoryCount = 0;
    public int troopCount = 0;
    public int troopsToDeploy = 0;
    public bool isAlive = true;
    [HideInInspector]public bool isPlaying = false;

    [HideInInspector] public Territory selectedTerritory;
    [HideInInspector] public Territory targetTerritory;

    public void ReceiveTerritory(Territory territory)
    {
        territory.Owner = this;
        territories.Add(territory);
        territoryCount++;
        troopCount += territory.Troops;
    }

    public void LoseTerritory(Territory territory)
    {
        territory.Owner = null;
        territories.Remove(territory);
        territoryCount--;
        troopCount -= territory.Troops;

        if (territoryCount == 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isAlive = false;
        GameManager.gameManager.PlayerDeath(this);
    }

    public void TroopsChangedCallback(Territory territory, int change)
    {
        troopCount += change;
    }
    
    public void UpdateTroopCount()
    {
        int troops = 0;
        foreach(Territory territory in territories)
        {
            troops += territory.Troops;
        }

        troopCount = troops;
    }

    public void ReceiveTroops(int amount)
    {
        troopsToDeploy += amount;
    }

    public void DeployTroops(int amount, Territory territory)
    {
        if (amount > troopsToDeploy || territory == null || territory.Owner != this)
        {
            Debug.LogError("Error: player " + name + " trying to deploy " + amount + " troops to " + territory + ".");
            return;
        }
        territory.DeployTroops(amount);
        troopsToDeploy -= amount;
    }

    public void BeginTurn()
    {
        isPlaying = true;
    }

    public void EndTurn()
    {
        DeployRemainingTroops();
        Deselect();
        DeselectTarget();
        isPlaying = false;
    }

    public void DeployRemainingTroops()
    {
        if (troopsToDeploy > 0) DeployTroops(troopsToDeploy, territories[Random.Range(0, territoryCount)]);
    }

    void Update()
    {
        if (!isPlaying) return;//is playing and it's you

        switch (GameManager.gameManager.TurnState)
        {
            case TurnState.Deploy:
                {
                    DeployUpdate();
                    break;
                }
            case TurnState.Attack:
                {
                    AttackUpdate();
                    break;
                }
            case TurnState.Fortify:
                {
                    FortifyUpdate();
                    break;
                }
        }
    }

    void Deselect()
    {
        if (selectedTerritory != null)
        {
            selectedTerritory.Selected = false;
            selectedTerritory = null;
        }
    }

    void Select(Territory territory)
    {
        Deselect();

        selectedTerritory = territory;
        if (selectedTerritory != null) selectedTerritory.Selected = true;
    }

    void DeselectTarget()
    {
        if (targetTerritory != null)
        {
            targetTerritory.Selected = false;
            targetTerritory = null;
        }
    }

    void SelectTarget(Territory territory)
    {
        DeselectTarget();

        targetTerritory = territory;
        if (targetTerritory != null) targetTerritory.Selected = true;
    }

    void HandleMyOwnTerritorySelection()
    {
        
    }

    void HandleAdjacentTerritorySelection()
    {
        
    }

    bool ConfirmAction()
    {
        return Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return);
    }

    bool ClickedOnTerritory(out Territory territory)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (CameraController.cameraController.MouseRaycast(out territory))
            {
                return true;
            }
        }

        territory = null;
        return false;
    }

    void DeployUpdate()
    {
        Territory territory;
        if (ClickedOnTerritory(out territory))
        {
            if (territory.Owner == this)
                Select(territory);
        }

        if (ConfirmAction())
        {
            if (selectedTerritory != null)
            {
                DeployTroops(troopsToDeploy, selectedTerritory);

                if (troopsToDeploy == 0)
                {
                    Deselect();
                    GameManager.gameManager.EndDeployPhase();
                }
            }
        }
    }

    void AttackUpdate()
    {
        Territory territory;
        if (ClickedOnTerritory(out territory))
        {
            if (territory.Owner == this)
            {
                DeselectTarget();

                if (territory.Troops > 1)
                    Select(territory);
            }
            else
            {
                if (selectedTerritory != null && selectedTerritory.IsAdjacent(territory))
                {
                    SelectTarget(territory);
                }
            }
        }

        //Enter to attack
        if (ConfirmAction() && selectedTerritory != null && targetTerritory != null)
        {
            Debug.Log(color.name + " attacked " + targetTerritory.Owner.color.name + ".");

            selectedTerritory.Attack(targetTerritory, selectedTerritory.Troops);

            Deselect();
            DeselectTarget();
        }

        if (EndPhaseAction())
        {
            GameManager.gameManager.EndAttackPhase();
        }
    }

    bool EndPhaseAction()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    void FortifyUpdate()
    {
        Territory territory;
        if (ClickedOnTerritory(out territory))
        {
            if (territory.Owner != this)
            {
                Deselect();
                DeselectTarget();
                return;
            }

            if (selectedTerritory == null && territory.Troops > 1)
            {
                Select(territory);
                return;
            }

            if (selectedTerritory != null && targetTerritory == null
                && selectedTerritory.IsConnected(territory))
            {
                SelectTarget(territory);
                return;
            }

            Deselect();
            DeselectTarget();
            return;
        }

        if (ConfirmAction())
        {
            if (selectedTerritory != null && targetTerritory != null)
            {
                int transferedTroops = selectedTerritory.Troops - 1;
                selectedTerritory.Troops = 1;
                targetTerritory.Troops += transferedTroops;
            }
            
            GameManager.gameManager.EndFortifyPhase();
        }

        if (EndPhaseAction())
        {
            GameManager.gameManager.EndFortifyPhase();
        }
    }
}
