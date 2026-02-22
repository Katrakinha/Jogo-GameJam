using UnityEngine;

public class ColetavelFinal : MonoBehaviour
{
    // Cria a lista de opções no Inspector
    public enum TipoItem { Isqueiro, Gasolina }
    public TipoItem meuItem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Checa qual opção você escolheu lá no Unity e salva no GameManager
            if (meuItem == TipoItem.Isqueiro)
            {
                GameManager.temIsqueiro = true;
                Debug.Log("Pegou o Isqueiro!");
            }
            else if (meuItem == TipoItem.Gasolina)
            {
                GameManager.temGasolina = true;
                Debug.Log("Pegou a Gasolina!");
            }

            Destroy(gameObject); // Some com o item
        }
    }
}