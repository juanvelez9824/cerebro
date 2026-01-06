using UnityEngine;

/// <summary>
/// Script de diagnóstico para verificar que todo está configurado correctamente
/// Adjunta este script al Player y revisa la consola
/// </summary>
public class DiagnosticChecker : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== INICIANDO DIAGNÓSTICO ===");
        
        CheckPlayerComponents();
        CheckInputSystem();
        CheckPhysics();
        CheckGroundDetection();
        CheckTags();
        
        Debug.Log("=== DIAGNÓSTICO COMPLETADO ===");
    }
    
    void CheckPlayerComponents()
    {
        Debug.Log("--- Verificando Componentes del Player ---");
        
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("❌ FALTA Rigidbody2D en el Player!");
        }
        else
        {
            Debug.Log("✓ Rigidbody2D encontrado");
            Debug.Log($"  - Body Type: {rb.bodyType}");
            Debug.Log($"  - Gravity Scale: {rb.gravityScale}");
            Debug.Log($"  - Constraints: {rb.constraints}");
            
            if (rb.bodyType != RigidbodyType2D.Dynamic)
            {
                Debug.LogWarning("⚠️ Rigidbody2D debe ser 'Dynamic'");
            }
        }
        
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError("❌ FALTA Collider2D en el Player!");
        }
        else
        {
            Debug.Log("✓ Collider2D encontrado");
        }
        
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("❌ FALTA PlayerController script en el Player!");
        }
        else
        {
            Debug.Log("✓ PlayerController script encontrado");
        }
        
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite == null)
        {
            Debug.LogWarning("⚠️ Falta SpriteRenderer (el player no será visible)");
        }
        else
        {
            Debug.Log("✓ SpriteRenderer encontrado");
        }
    }
    
    void CheckInputSystem()
    {
        Debug.Log("--- Verificando Sistema de Input ---");
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        Debug.Log($"Input Horizontal actual: {horizontal}");
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || 
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("✓ Teclas de movimiento detectadas");
        }
        
        // Verificar si existe el Input Manager
        try
        {
            Input.GetAxis("Horizontal");
            Debug.Log("✓ Input Manager configurado correctamente");
        }
        catch
        {
            Debug.LogError("❌ Problema con Input Manager");
        }
    }
    
    void CheckPhysics()
    {
        Debug.Log("--- Verificando Física 2D ---");
        
        Debug.Log($"Gravedad 2D: {Physics2D.gravity}");
        
        if (Physics2D.gravity.y > -5f)
        {
            Debug.LogWarning("⚠️ La gravedad es muy débil. Recomendado: -20 o menos");
        }
    }
    
    void CheckGroundDetection()
    {
        Debug.Log("--- Verificando Ground Check ---");
        
        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null)
        {
            // Usar reflexión para acceder a campos privados (solo para debug)
            var field = typeof(PlayerController).GetField("groundCheck", 
                System.Reflection.BindingFlags.NonPublic | 
                System.Reflection.BindingFlags.Instance);
            
            if (field != null)
            {
                Transform groundCheck = field.GetValue(pc) as Transform;
                if (groundCheck == null)
                {
                    Debug.LogError("❌ GroundCheck no está asignado en PlayerController!");
                    Debug.LogError("   SOLUCIÓN: Crea un GameObject hijo llamado 'GroundCheck' y asígnalo");
                }
                else
                {
                    Debug.Log("✓ GroundCheck asignado");
                    Debug.Log($"  Posición: {groundCheck.localPosition}");
                }
            }
        }
    }
    
    void CheckTags()
    {
        Debug.Log("--- Verificando Tags ---");
        
        if (gameObject.tag != "Player")
        {
            Debug.LogError("❌ El GameObject NO tiene el tag 'Player'");
            Debug.LogError("   SOLUCIÓN: Selecciona el Player → Inspector → Tag → Player");
        }
        else
        {
            Debug.Log("✓ Tag 'Player' asignado correctamente");
        }
        
        // Buscar objetos con tag Ground
        GameObject[] grounds = GameObject.FindGameObjectsWithTag("Ground");
        if (grounds.Length == 0)
        {
            Debug.LogWarning("⚠️ No hay objetos con tag 'Ground' en la escena");
            Debug.LogWarning("   Las plataformas necesitan el tag 'Ground'");
        }
        else
        {
            Debug.Log($"✓ Encontradas {grounds.Length} plataformas con tag 'Ground'");
        }
    }
    
    void Update()
    {
        // Diagnóstico en tiempo real
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("=== DIAGNÓSTICO EN TIEMPO REAL ===");
            
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Debug.Log($"Velocity: {rb.linearVelocity}");
                Debug.Log($"Position: {transform.position}");
            }
            
            float input = Input.GetAxisRaw("Horizontal");
            Debug.Log($"Input Horizontal: {input}");
            
            if (input == 0)
            {
                Debug.LogWarning("⚠️ No se detecta input! Presiona A/D o ←/→");
            }
        }
    }
}