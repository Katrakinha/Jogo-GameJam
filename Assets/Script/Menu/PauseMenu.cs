using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public static bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Continuar();
            else Pausar();
        }
    }

    public void Continuar()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Volta o tempo ao normal
        isPaused = false;
    }

    public void Pausar()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Congela o jogo
        isPaused = true;
    }

    public void SairDoJogo()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit(); // Só funciona na build final (jogo exportado)
    }
}