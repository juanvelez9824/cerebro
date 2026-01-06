using UnityEngine;

public class PlatformerHelper : MonoBehaviour
{
    [Header("Jump Buffer & Coyote Time")]
    [SerializeField] private float jumpBufferTime = 0.2f; // Tiempo para recordar input de salto
    [SerializeField] private float coyoteTime = 0.15f; // Tiempo de gracia después de salir de una plataforma
    
    private float jumpBufferCounter;
    private float coyoteTimeCounter;
    private bool isGrounded;
    
    [Header("Variable Jump Height")]
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        // Jump Buffer: registra el input de salto
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        
        // Coyote Time: da gracia al salir de plataforma
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        
        // Salto con mejor feeling
        ApplyBetterJump();
    }
    
    void ApplyBetterJump()
    {
        // Si está cayendo más rápido (mejor sensación de gravedad)
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        // Si soltó el botón de salto temprano (salto variable)
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    
    public void SetGrounded(bool grounded)
    {
        isGrounded = grounded;
    }
    
    public bool CanJump()
    {
        return (jumpBufferCounter > 0f && coyoteTimeCounter > 0f);
    }
}