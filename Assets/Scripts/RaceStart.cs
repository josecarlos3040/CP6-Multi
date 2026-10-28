using Fusion;
using UnityEngine;

public class RaceStart : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private NetworkObject playerPrefab;

    private async void Start()
    {
        NetworkRunner runner = GetComponent<NetworkRunner>();
        if (playerPrefab == null)
        {
            Debug.LogError("prefab nao configurado");
            return;
        }

        runner.ProvideInput = false;

        var sceneManager =
            GetComponent<NetworkSceneManagerDefault>();

        var result = await runner.StartGame(
            new StartGameArgs
            {
                GameMode = GameMode.Shared,
                SessionName = "Corrida01",
                PlayerCount = 2,
                SceneManager = sceneManager
            }
        );

        if (result.Ok)
        {
            Debug.Log("conectado a corrida");
        }
        else
        {
            Debug.LogError(result.ShutdownReason);
        }


    }

    public void PlayerJoined(PlayerRef jogador)
    {
        if (jogador != Runner.LocalPlayer)
            return;
        bool primeiroJogador =
            Runner.IsSharedModeMasterClient;

        Vector3 posicao = primeiroJogador
            ? new Vector3(-2f, 0.5f, -9f)
            : new Vector3(2f, 0.5f, -9f);

        NetworkObject novoJogador = Runner.Spawn(
            playerPrefab,
            posicao,
            Quaternion.identity
        );

        Runner.SetPlayerObject(jogador, novoJogador);
    }
}