using UnityEngine;

public class PortaFase : MonoBehaviour
{
    public static bool temChave = false; // Variável global da cena
    public GameObject avisoE;
    private bool pertoDaPorta = false;

    void Start()
    {
        temChave = false; 
        if (avisoE != null) avisoE.SetActive(false);
    }

    void Update()
    {
        if (pertoDaPorta && temChave)
        {
            if (avisoE != null) avisoE.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (avisoE != null) avisoE.SetActive(false);
                Destroy(gameObject); 
            }
        }
        else
        {
            if (avisoE != null) avisoE.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) pertoDaPorta = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) pertoDaPorta = false;
    }
}