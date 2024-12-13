using System.Collections.Generic;

public class LobbyFactory
{
    private readonly List<string> _lobbies = new List<string>();
    
    public string[] Lobbies => _lobbies.ToArray();

    // public TypedLobby Create(string name, LobbyType lobbyType)
    // {
    //     TypedLobby lobby = new TypedLobby(name, lobbyType);
    //     _lobbies.Add(name);
    //
    //     return lobby;
    // }
}