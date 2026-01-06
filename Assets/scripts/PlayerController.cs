using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float baseSpeed = 3f;
    [SerializeField] private float currentSpeed = 3f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float depressedSpeedMultiplier = 0.4f;
    [SerializeField] private float depressedJumpMultiplier = 0.5f;
    
    [Header("Serotonin Boost")]
    [SerializeField] private float boostSpeedMultiplier = 1.5f;
    [SerializeField] private float boostDuration = 5f;
    private float boostTimer = 0f;
    private bool isBoosted = false;
    
    [Header("Dash Ability")]
    [SerializeField] private float dashForce = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private bool isDashing = false;
    
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    
    [Header("Ruminating Enemies")]
    [SerializeField] private int ruminatingCount = 0;
    [SerializeField] private float ruminatingSlowdown = 0.15f;
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private float horizontalInput;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (rb == null)
        {
            Debug.LogError("❌ Falta Rigidbody2D en el Player!");
        }
        
        if (groundCheck == null)
        {
            Debug.LogError("❌ Falta asignar GroundCheck en el Inspector!");
        }
        
        ApplyDepressionState();
        
        Debug.Log("✓ PlayerController iniciado - Usa WASD o Flechas para moverte, Espacio para saltar, J para Dash");
    }
    
    void Update()
    {
        // INPUT COMPATIBLE - Usa teclas directamente
        horizontalInput = 0f;
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput = 1f;
        }
        
        // Salto
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) 
            && isGrounded && !isDashing)
        {
            Jump();
        }
        
        // Dash
        if ((Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.LeftShift)) 
            && dashCooldownTimer <= 0 && !isDashing)
        {
            StartDash();
        }
        
        UpdateTimers();
        
        // Flip sprite
        if (horizontalInput > 0)
            spriteRenderer.flipX = false;
        else if (horizontalInput < 0)
            spriteRenderer.flipX = true;
    }
    
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
        if (!isDashing)
        {
            float moveSpeed = CalculateCurrentSpeed();
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }
    }
    
    void Jump()
    {
        float actualJumpForce = jumpForce;
        
        if (!isBoosted)
        {
            actualJumpForce *= depressedJumpMultiplier;
        }
        
        actualJumpForce *= (1f - (ruminatingCount * ruminatingSlowdown));
        
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, actualJumpForce);
        Debug.Log("¡Salto!");
    }
    
    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;
        
        float dashDirection = spriteRenderer.flipX ? -1f : 1f;
        rb.linearVelocity = new Vector2(dashDirection * dashForce, rb.linearVelocity.y);
        
        if (ruminatingCount > 0)
        {
            RemoveRuminating(1);
        }
        
        Debug.Log("¡Dash!");
    }
    
    void UpdateTimers()
    {
        if (isBoosted)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0)
            {
                isBoosted = false;
                ApplyDepressionState();
                Debug.Log("Boost de serotonina terminado");
            }
        }
        
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
            }
        }
        
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }
    
    float CalculateCurrentSpeed()
    {
        float speed = currentSpeed;
        speed *= (1f - (ruminatingCount * ruminatingSlowdown));
        return Mathf.Max(speed, 0.5f);
    }
    
    void ApplyDepressionState()
    {
        currentSpeed = baseSpeed * depressedSpeedMultiplier;
    }
    
    public void CollectSerotonin()
    {
        isBoosted = true;
        boostTimer = boostDuration;
        currentSpeed = baseSpeed * boostSpeedMultiplier;
        
        Debug.Log("¡Serotonina recolectada! Velocidad restaurada por 5 segundos.");
    }
    
    public void AddRuminating()
    {
        ruminatingCount++;
        Debug.Log($"Rumiante pegado. Total: {ruminatingCount}");
    }
    
    public void RemoveRuminating(int amount)
    {
        ruminatingCount = Mathf.Max(0, ruminatingCount - amount);
        Debug.Log($"Rumiante removido. Total: {ruminatingCount}");
    }
    
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
