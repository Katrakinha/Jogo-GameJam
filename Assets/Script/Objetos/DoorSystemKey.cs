using UnityEngine;

public class DoorSystemKey : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite spriteAberta;
    public Sprite spriteFechada;
    public GameObject iconeE;
    
    [Tooltip("Colisor que bloqueia a passagem do jogador")]
    public BoxCollider2D colisorFisico; 

    [Header("Configurações da Porta")]
    public bool precisaDeChave = true;

    private bool perto = false;
    private bool aberta = false;
    private PlayerController playerPertoRef;

    void Start()
    {
        iconeE.SetActive(false);
    }

    void Update()
    {
        if (perto && Input.GetKeyDown(KeyCode.E))
        {
            if (precisaDeChave)
            {
                if (playerPertoRef == null || !playerPertoRef.temChave)
                {
                    Debug.Log("A porta está trancada! Você precisa da chave.");
                    return; 
                }
            }

            aberta = !aberta;
            spriteRenderer.sprite = aberta ? spriteAberta : spriteFechada;
            colisorFisico.enabled = !aberta;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            perto = true;
            iconeE.SetActive(true);
            playerPertoRef = collision.GetComponent<PlayerController>(); // Pega o script do player
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            perto = false;
            iconeE.SetActive(false);
            playerPertoRef = null;
        }
    }
}