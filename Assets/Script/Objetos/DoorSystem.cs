using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite spriteAberta;
    public Sprite spriteFechada;
    public GameObject iconeE;
    
    [Tooltip("Colisor que bloqueia a passagem do jogador")]
    public BoxCollider2D colisorFisico; 

    private bool perto = false;
    private bool aberta = false;

    void Start()
    {
        iconeE.SetActive(false);
    }

    void Update()
    {
        if (perto && Input.GetKeyDown(KeyCode.E))
        {
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            perto = false;
            iconeE.SetActive(false);
        }
    }
}