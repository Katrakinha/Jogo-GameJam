using UnityEngine;

public class FogueiraTrigger : MonoBehaviour
{
    [SerializeField] private CutsceneManager cutsceneManager;
    private bool playerPerto = false;

    private void Update()
    {
        // Verifica se o player está perto E apertou E
        if (playerPerto && Input.GetKeyDown(KeyCode.E))
        {
            // Verifica se o player tem os dois itens no GameManager
            if (GameManager.temIsqueiro && GameManager.temGasolina)
            {
                cutsceneManager.PlayCutscene();
            }
            else
            {
                Debug.Log("Você ainda precisa encontrar o isqueiro e a gasolina!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = true;
            Debug.Log("Pressione E para acender a fogueira");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = false;
        }
    }
}