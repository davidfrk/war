using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnState{
    Deploy,
    Attack,
    Fortify,
}

public class GameManager : MonoBehaviour
{
    static public GameManager gameManager;

    public int turn = 0;
    public List<Player> players;
    public Map map;

    private TurnState turnState;
    public TurnState TurnState {
        get => turnState;
        private set{
            turnState = value;
            Debug.Log(TurnState + " Phase.");
        }
    }
    private Player playerPlaying;
    public Player PlayerPlaying { get => playerPlaying; private set => playerPlaying = value; }

    public float ElapsedTurnTime
    {
        get
        {
            return Time.time - turnStartTime;
        }
    }
    private float turnStartTime;
    public float turnDuration = 30.0f;
    private int playerTurnCount = 0;

    void Awake()
    {
        gameManager = this;
    }
    
    void Start()
    {
        CreatePlayers();
        AssignTerritories();

        turn = 1;
        BeginPlayerTurn(players[0]);
    }

    void Update()
    {
        if ((Time.time - turnStartTime) > turnDuration){
            EndTurn();
            return;
        }
    }

    void CreatePlayers()
    {
        players = new List<Player>(GetComponentsInChildren<Player>());
    }

    void AssignTerritories()
    {
        int playersCount = players.Count;
        int territoriesGiven = 0;
        List<Territory> territoriesToAssign = new List<Territory>(map.territories);
        int initialTroops = territoriesToAssign.Count * 2 / playersCount;

        for (int i = 0; i < initialTroops * playersCount; i++)
        {
            Player player = players[i % playersCount];

            if(territoriesToAssign.Count > 0)
            {
                int index = Random.Range(0, territoriesToAssign.Count - 1);
                player.ReceiveTerritory(territoriesToAssign[index]);
                territoriesToAssign.RemoveAt(index);
                territoriesGiven++;
            }
            else
            {
                Territory territory = player.territories[Random.Range(0, player.territories.Count - 1)];
                territory.Troops++;
            }
        }
    }

    void BeginPlayerTurn(Player player)
    {
        PlayerPlaying = player;
        TurnState = TurnState.Deploy;
        PlayerPlaying.ReceiveTroops(BeginTurnTroops(PlayerPlaying));
        turnStartTime = Time.time;
        PlayerPlaying.BeginTurn();

        Debug.Log(PlayerPlaying.name + "'s turn.");
        playerTurnCount++;
    }

    void EndPlayerTurn()
    {
        PlayerPlaying.EndTurn();
    }

    void NextPlayer()
    {
        int nextIndex = (players.IndexOf(playerPlaying) + 1) % players.Count;
        if (nextIndex == 0) turn++;
        BeginPlayerTurn(players[nextIndex]);
    }

    int BeginTurnTroops(Player player)
    {
        //Territorial bonus
        int troopsAmount = Mathf.Max(3, PlayerPlaying.territoryCount / 3);

        //Bonus for playing later
        if (turn == 1)
            troopsAmount += playerTurnCount;

        //Continental Bonuses
        foreach (Continent continent in map.continents)
        {
            if (continent.Owner == player)
            {
                troopsAmount += continent.bonus;
            }
        }

        return troopsAmount;
    }

    public void EndDeployPhase()
    {
        if (PlayerPlaying.troopsToDeploy == 0) TurnState = TurnState.Attack;
    }

    public void EndAttackPhase()
    {
        TurnState = TurnState.Fortify;
    }

    public void EndFortifyPhase()
    {
        EndTurn();
    }

    public void EndTurn()
    {
        EndPlayerTurn();
        NextPlayer();
    }

    public void PlayerDeath(Player player)
    {
        players.Remove(player);
        if (players.Count == 1)
        {
            Debug.Log(players[0].color.name + " won! Congratulations.");
        }
    }
}
