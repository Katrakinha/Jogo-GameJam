using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movimentação e Animação")]
    [SerializeField] private float moveSpeed = 5f;
    public Animator animator; 
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isDead = false;

    [Header("Referências da Lanterna")]
    [SerializeField] private Light2D flashlightLight;      
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

    [Header("Status e Inventário")]
    public bool temChave = false;
    public bool isFlashlightOn = true;
    public bool isFocused = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; 
        if (flashlightLight != null) flashlightLight.falloffIntensity = 0.1f; 
    }

    void Update()
    {
        if (isDead) return;

        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        
        if (animator != null)
        {
            // Isso envia o valor para o Animator
            animator.SetFloat("Speed", moveInput.magnitude);
        }

        RotatePlayerToMouse();
        HandleFlashlightInput();
    }

    void FixedUpdate()
    {
        // AJUSTE AQUI: Use .velocity se der erro no linearVelocity
        if (!isDead) rb.linearVelocity = moveInput * moveSpeed;
        else rb.linearVelocity = Vector2.zero;
    }

    void RotatePlayerToMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = (Vector2)mousePos - (Vector2)transform.position;
        float targetAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f; 
        float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetAngle, ref currentVelocity, rotationSmoothness);
        transform.rotation = Quaternion.Euler(0, 0, smoothedAngle);
    }

    void HandleFlashlightInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isFlashlightOn = !isFlashlightOn;
            if (flashlightLight != null) flashlightLight.enabled = isFlashlightOn;
        }

        if (isFlashlightOn && flashlightLight != null)
        {
            isFocused = Input.GetMouseButton(0);
            float targetAngle = isFocused ? focusedAngle : normalAngle;
            float targetRadius = isFocused ? focusedRadius : normalRadius;
            float targetIntensity = isFocused ? focusedIntensity : normalIntensity;

            flashlightLight.pointLightOuterAngle = targetAngle;
            flashlightLight.pointLightInnerAngle = targetAngle * 0.4f; 
            flashlightLight.pointLightOuterRadius = targetRadius;
            flashlightLight.intensity = targetIntensity;
        }
    }

    public bool EstaIluminando(Vector3 posicaoAlvo)
    {
        if (!isFlashlightOn) return false;
        float raioAtual = isFocused ? focusedRadius : normalRadius;
        float anguloAtual = isFocused ? focusedAngle : normalAngle;
        Vector2 direcaoAlvo = (Vector2)posicaoAlvo - (Vector2)transform.position;
        if (direcaoAlvo.magnitude > raioAtual) return false; 
        float angulo = Vector2.Angle(transform.up, direcaoAlvo);
        return angulo <= anguloAtual / 2f; 
    }

    public void Morrer()
    {
        if (isDead) return;
        isDead = true;

        // Trava o peso do personagem e desliga a colisão para o inimigo passar direto
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;

        if (animator != null) animator.SetTrigger("IsDead");
        Invoke("ReiniciarCena", 2f);
    }

    void ReiniciarCena() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Chave"))
        {
            temChave = true;
            Destroy(collision.gameObject); 
        }
    }

    private void OnDrawGizmosSelected()
    {
        DrawFlashlightGizmo(transform.position, transform.up, normalAngle, normalRadius, Color.yellow);
        DrawFlashlightGizmo(transform.position, transform.up, focusedAngle, focusedRadius, Color.cyan);
    }

    private void DrawFlashlightGizmo(Vector3 position, Vector3 direction, float angle, float radius, Color color)
    {
        Gizmos.color = color;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-angle * 0.5f, Vector3.forward);
        Quaternion rightRayRotation = Quaternion.AngleAxis(angle * 0.5f, Vector3.forward);
        Gizmos.DrawRay(position, (leftRayRotation * direction) * radius);
        Gizmos.DrawRay(position, (rightRayRotation * direction) * radius);
    }
}