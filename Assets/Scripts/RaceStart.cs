using Fusion;
using UnityEngine;

public class RaceStart : SimulationBehaviour, IPlayerJoined
{
    public NetworkObject playerPrefab;

    private async void Start()
    {
        NetworkRunner runner = GetComponent<NetworkRunner>();

        var result = await runner.StartGame(new StartGameArgs
            {
                GameMode = GameMode.Shared,
                SessionName = "Corrida01",
                PlayerCount = 2

            }
        );
        if (!result.Ok)
        {
            Debug.LogError(result.ShutdownReason);
        }           
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (player != Runner.LocalPlayer)
            return;

        Vector3 posicao = Runner.IsSharedModeMasterClient
            ? new Vector3(-2, 0.5f, -9)
            : new Vector3(2, 0.5f, -9);

        NetworkObject objeto = Runner.Spawn(playerPrefab, posicao, Quaternion.identity);

        Runner.SetPlayerObject(player, objeto);
    }
}
