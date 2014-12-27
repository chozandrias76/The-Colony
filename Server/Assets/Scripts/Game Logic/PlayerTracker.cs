using System.Collections.Generic;
using UnityEngine;
using NetworkViewID = uLink.NetworkViewID;

public class PlayerTracker : MonoBehaviour
{
	//This class tracks every player that has joined the game.
	//This class allows you to look up players.
    public static PlayerTracker Singleton;
    public List<PlayerCharacter> players = new List<PlayerCharacter>();

    public void OnEnable()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    public void AddPlayer(PlayerCharacter pc)
    {
        if (!players.Contains(pc))
            players.Add(pc);
    }

    public void RemovePlayer(PlayerCharacter pc)
    {
		try
		{
        	players.Remove(pc);
		}
		catch
		{
		}
    }

	public bool FindPlayer(NetworkViewID iD, out PlayerCharacter foundPlayer)
    {
		PlayerCharacter lookingForCharacter = new PlayerCharacter("","");
        if (players.Count != 0)
            foreach (PlayerCharacter player in players)
            {
                if (player.iD == iD.id)
                {
                    foundPlayer = player;
					return true;
                    
                }
            }
		foundPlayer = lookingForCharacter;
        return false;
    }

    private void Awake()
    {
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    private void OnGUI()
    {
    }

    private void OnDrawGizmos()
    {
    }
}