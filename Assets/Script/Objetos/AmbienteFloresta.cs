using UnityEngine;

public class AmbienteFloresta : MonoBehaviour
{
    [Header("Fontes de Áudio")]
    public AudioSource fonteVento;
    public AudioSource fonteFolhas;
    public AudioSource fonteGrilos;

    [Header("Configurações de Volume")]
    [Range(0f, 1f)] public float volumeVento = 0.3f;
    [Range(0f, 1f)] public float volumeFolhas = 0.2f;
    [Range(0f, 1f)] public float volumeGrilos = 0.15f;

    void Start()
    {
        // Aplica os volumes iniciais
        AtualizarVolumes();
    }

    void Update()
    {
        // Permite que você ajuste o volume em tempo real enquanto testa
        AtualizarVolumes();
    }

    void AtualizarVolumes()
    {
        if (fonteVento != null) fonteVento.volume = volumeVento;
        if (fonteFolhas != null) fonteFolhas.volume = volumeFolhas;
        if (fonteGrilos != null) fonteGrilos.volume = volumeGrilos;
    }
}