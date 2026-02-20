using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [Header("Movimentação")]
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("Referências")]
    [SerializeField] private Transform flashlightTransform; 
    [SerializeField] private Light2D flashlightLight;      
    [SerializeField] private Light2D playerGlow;           

    [Header("Configurações: Lanterna Suave")]
    [Range(0.01f, 0.5f)] public float rotationSmoothness = 0.1f;
    private float currentVelocity; 

    [Header("Configurações: Modo Normal")]
    public float normalAngle = 60f;
    public float normalRadius = 5f;
    public float normalIntensity = 1.2f;

    [Header("Configurações: Modo Focado")]
    public float focusedAngle = 20f;
    public float focusedRadius = 12f;
    public float focusedIntensity = 2f;

    private bool isFlashlightOn = true;
    private bool isFocused = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; 
        
        // Ajuste inicial para a luz ser sólida até a borda
        if (flashlightLight != null) {
            flashlightLight.falloffIntensity = 0.1f; // Quase 0 faz a borda ficar nítida
        }
    }

    void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        RotateFlashlightSmooth();
        HandleFlashlightInput();
    }

    void FixedUpdate() => rb.linearVelocity = moveInput * moveSpeed;

    void RotateFlashlightSmooth()
    {
        if (flashlightTransform == null) return;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = (Vector2)mousePos - (Vector2)flashlightTransform.position;
        float targetAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        float smoothedAngle = Mathf.SmoothDampAngle(flashlightTransform.eulerAngles.z, targetAngle, ref currentVelocity, rotationSmoothness);
        flashlightTransform.rotation = Quaternion.Euler(0, 0, smoothedAngle);
    }

    void HandleFlashlightInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isFlashlightOn = !isFlashlightOn;
            flashlightLight.enabled = isFlashlightOn;
        }

        if (isFlashlightOn)
        {
            isFocused = Input.GetMouseButton(0);

            // Transição de valores
            float targetAngle = isFocused ? focusedAngle : normalAngle;
            float targetRadius = isFocused ? focusedRadius : normalRadius;
            float targetIntensity = isFocused ? focusedIntensity : normalIntensity;

            flashlightLight.pointLightOuterAngle = targetAngle;
            flashlightLight.pointLightInnerAngle = targetAngle * 0.4f; // Suaviza levemente o centro
            flashlightLight.pointLightOuterRadius = targetRadius;
            flashlightLight.intensity = targetIntensity;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (flashlightTransform == null) return;
        DrawFlashlightGizmo(flashlightTransform.position, flashlightTransform.up, normalAngle, normalRadius, Color.yellow);
        DrawFlashlightGizmo(flashlightTransform.position, flashlightTransform.up, focusedAngle, focusedRadius, Color.cyan);
    }

    private void DrawFlashlightGizmo(Vector3 position, Vector3 direction, float angle, float radius, Color color)
    {
        Gizmos.color = color;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-angle * 0.5f, Vector3.forward);
        Quaternion rightRayRotation = Quaternion.AngleAxis(angle * 0.5f, Vector3.forward);
        Vector3 leftRayDirection = leftRayRotation * direction;
        Vector3 rightRayDirection = rightRayRotation * direction;

        Gizmos.DrawRay(position, leftRayDirection * radius);
        Gizmos.DrawRay(position, rightRayDirection * radius);

        int segments = 20;
        Vector3 previousPoint = position + leftRayDirection * radius;
        for (int i = 1; i <= segments; i++)
        {
            float currentAngle = -angle * 0.5f + (angle / segments) * i;
            Vector3 nextPoint = position + (Quaternion.AngleAxis(currentAngle, Vector3.forward) * direction) * radius;
            Gizmos.DrawLine(previousPoint, nextPoint);
            previousPoint = nextPoint;
        }
    }
}