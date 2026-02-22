using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private GameObject cutscenePanel;
    [SerializeField] private VideoPlayer videoPlayer;

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (cutscenePanel != null) cutscenePanel.SetActive(false);

        if (videoPlayer != null)
            videoPlayer.loopPointReached += OnVideoFinished;

    }

    public void PlayCutscene()
    {
        if (cutscenePanel != null) cutscenePanel.SetActive(true);
        if (videoPlayer != null) videoPlayer.Play();

        // Congela o tempo
        Time.timeScale = 0f;    

    }
    private void OnVideoFinished(VideoPlayer vp)
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Para o jogo no editor
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
