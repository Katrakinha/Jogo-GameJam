using UnityEngine;

public class Chave : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PortaFase.temChave = true; // Avisa a porta que pegou a chave
            Destroy(gameObject);   // Some com a chave
        }
    }
}