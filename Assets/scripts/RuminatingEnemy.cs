using UnityEngine;

public class RuminatingEnemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float attachRange = 0.8f;
    
    [Header("Behavior")]
    [SerializeField] private bool isAttached = false;
    [SerializeField] private Vector3 attachOffset = new Vector3(0, 0.5f, 0);
    
    private Transform playerTransform;
    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        
        // Buscar al jugador
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerController = player.GetComponent<PlayerController>();
        }
    }
    
    void Update()
    {
        if (isAttached)
        {
            // Seguir al jugador si está pegado
            if (playerTransform != null)
            {
                transform.position = playerTransform.position + attachOffset;
            }
        }
        else
        {
            // Perseguir al jugador si está en rango
            if (playerTransform != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
                
                if (distanceToPlayer <= detectionRange)
                {
                    MoveTowardsPlayer();
                    
                    // Intentar pegarse si está muy cerca
                    if (distanceToPlayer <= attachRange)
                    {
                        AttachToPlayer();
                    }
                }
            }
        }
    }
    
    void MoveTowardsPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
        
        // Flip sprite según dirección
        if (direction.x < 0)
            spriteRenderer.flipX = true;
        else if (direction.x > 0)
            spriteRenderer.flipX = false;
    }
    
    void AttachToPlayer()
    {
        if (!isAttached && playerController != null)
        {
            isAttached = true;
            playerController.AddRuminating();
            
            // Desactivar física
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            
            // Cambiar color para indicar que está pegado
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 0.8f);
            }
            
            Debug.Log("Rumiante se ha pegado al jugador");
        }
    }
    
    public void Detach()
    {
        if (isAttached)
        {
            isAttached = false;
            
            if (playerController != null)
            {
                playerController.RemoveRuminating(1);
            }
            
            // Reactivar física
            rb. bodyType = RigidbodyType2D.Kinematic;
            
            // Empujar hacia atrás
            Vector2 pushDirection = (transform.position - playerTransform.position).normalized;
            rb.AddForce(pushDirection * 5f, ForceMode2D.Impulse);
            
            // Restaurar color
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.white;
            }
            
            // Destruir después de un tiempo (opcional)
            Destroy(gameObject, 2f);
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAttached)
        {
            AttachToPlayer();
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // Visualizar rango de detección
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Visualizar rango de adherencia
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attachRange);
    }
}
