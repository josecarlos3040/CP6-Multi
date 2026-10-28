using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;


public class Player : NetworkBehaviour
{
    [Networked] public NetworkBool Chegou { get; set; }
    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority || Chegou)
        { return; }

        int jogadores = 0;

        foreach (var player in Runner.ActivePlayers)
            jogadores++;

        if (jogadores < 2)
            return;

        foreach (var outro in FindObjectsByType<Player>(FindObjectsSortMode.None))
        {
            if (outro != this && outro.Chegou)
                return;
        }
        if (Keyboard.current != null && Keyboard.current.wKey.isPressed)
        {
            transform.position += Vector3.forward * 6f * Runner.DeltaTime;
        }

        if (transform.position.z >= 10f)
        { 
           Chegou = true;
        }
    }
    private void OnGUI()
    {
        if (!HasStateAuthority)
            return;

        bool outroChegou = false;

        foreach (var outro in FindObjectsByType<Player>(FindObjectsSortMode.None))
        {
            if (outro != this && outro.Chegou)
                outroChegou = true;
        }

        string mensagem = "Segure W para correr";

        if (Chegou && !outroChegou)
        {
            mensagem = "Voce Venceu";
        }
        else if (outroChegou && !Chegou)
            mensagem = "Voce Perdeu";

        else if (Chegou && outroChegou)
            mensagem = "Corrida Encerrada";

        GUI.Label(new Rect(20, 20, 350, 50), mensagem);
    }

}
