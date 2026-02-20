using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Alvo")]
    [SerializeField] private Transform target; // Arraste seu Player para cá

    [Header("Configurações")]
    [SerializeField] private float smoothTime = 0.25f; // Tempo de resposta (atraso suave)
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); // Mantém a câmera atrás no eixo Z

    private Vector3 currentVelocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        // Define a posição desejada (posição do player + o desvio do eixo Z)
        Vector3 targetPosition = target.position + offset;

        // Move a câmera de forma suave até o player
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }
}