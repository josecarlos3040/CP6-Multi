using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;

public class CorridaPlayer : MonoBehaviour
{
    [SerializeField] private float velocidade = 6f;
    [SerializeField] private float chegada = 10f;

    private NetworkObject objetoRede;
    private void Awake()
    {
        objetoRede = GetComponent<NetworkObject>();
    }

    private void Update()
    {

        if (objetoRede == null ||!objetoRede.HasStateAuthority)
            return;

        CorridaPlayer[] jogadores =
            FindObjectsByType<CorridaPlayer>();
        if (jogadores.Length < 2)
            return;


        foreach (CorridaPlayer outro in jogadores)
        {
            if (outro != this &&
                outro.transform.position.z >= chegada)
                return;
        }

        if (Keyboard.current != null && Keyboard.current.wKey.isPressed &&
            transform.position.z < chegada)
        {
            transform.position +=
                Vector3.forward * velocidade * Time.deltaTime;
        }

        if (transform.position.z > chegada)
        {
            Vector3 posicao = transform.position;
            posicao.z = chegada;
            transform.position = posicao;
        }
    }

    private void OnGUI()
    {
        if (objetoRede == null ||
            !objetoRede.HasStateAuthority)
            return;

        CorridaPlayer[] jogadores =
            FindObjectsByType<CorridaPlayer>();

        string mensagem = "Aguardando jogador...";

        if (jogadores.Length >= 2)
        {
            mensagem = "Segure W para correr!";

            bool venceu = transform.position.z >= chegada;
            bool perdeu = false;

            foreach (CorridaPlayer outro in jogadores)
            {
                if (outro != this &&
                    outro.transform.position.z >= chegada)
                {
                    perdeu = true;
                }
            }

            if (venceu && !perdeu)
                mensagem = "VOCE VENCEU!";
            else if (perdeu && !venceu)
                mensagem = "VOCE PERDEU!";
            else if (venceu && perdeu)
                mensagem = "CORRIDA ENCERRADA!";
        }

        GUI.Box(new Rect(20, 20, 350, 70),mensagem);
    }
}