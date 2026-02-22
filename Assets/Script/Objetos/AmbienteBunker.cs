using UnityEngine;

public class AmbienteBunker : MonoBehaviour
{
    [Header("Fontes de Áudio")]
    public AudioSource fonteSuspense;
    public AudioSource fonteAmbienteBunker; // Som metálico/vazio
    public AudioSource fonteGoteira;

    [Header("Volumes")]
    [Range(0f, 1f)] public float volumeSuspense = 0.4f;
    [Range(0f, 1f)] public float volumeBunker = 0.3f;
    [Range(0f, 1f)] public float volumeGoteira = 0.5f;

    [Header("Configuração da Goteira")]
    public AudioClip somPingo;
    public float tempoMinimoPingo = 2f;
    public float tempoMaximoPingo = 5f;
    private float cronometroGoteira;

    void Start()
    {
        cronometroGoteira = Random.Range(tempoMinimoPingo, tempoMaximoPingo);
        
        if (fonteSuspense) fonteSuspense.volume = volumeSuspense;
        if (fonteAmbienteBunker) fonteAmbienteBunker.volume = volumeBunker;
    }

    void Update()
    {
        // Gerenciador da Goteira Aleatória
        cronometroGoteira -= Time.deltaTime;
        if (cronometroGoteira <= 0)
        {
            TocarGoteira();
            cronometroGoteira = Random.Range(tempoMinimoPingo, tempoMaximoPingo);
        }

        // Atualiza volumes em tempo real para ajuste no Inspector
        if (fonteSuspense) fonteSuspense.volume = volumeSuspense;
        if (fonteAmbienteBunker) fonteAmbienteBunker.volume = volumeBunker;
    }

    void TocarGoteira()
    {
        if (fonteGoteira != null && somPingo != null)
        {
            // Toca o pingo com uma leve variação de volume e pitch para parecer real
            fonteGoteira.pitch = Random.Range(0.8f, 1.2f);
            fonteGoteira.PlayOneShot(somPingo, volumeGoteira);
        }
    }
}