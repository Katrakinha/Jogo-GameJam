using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Variáveis globais para o final do jogo
    public static bool temIsqueiro = false;
    public static bool temGasolina = false;

    private void Awake()
    {
        // Garante que o GameManager não seja destruído ao mudar de fase
        int numGameManagers = FindObjectsByType<GameManager>(FindObjectsSortMode.None).Length;
        if (numGameManagers > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}