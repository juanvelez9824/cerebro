using UnityEngine;

public class SerotoninOrb : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float floatAmplitude = 0.3f;
    [SerializeField] private float floatSpeed = 2f;
    
    [Header("Collection")]
    [SerializeField] private GameObject collectionEffectPrefab;
    [SerializeField] private AudioClip collectSound;
    
    private Vector3 startPosition;
    
    void Start()
    {
        startPosition = transform.position;
    }
    
    void Update()
    {
        // Rotación continua
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        
        // Flotación suave
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Llamar al método de boost en el jugador
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.CollectSerotonin();
                
                // Efecto de partículas (si existe)
                if (collectionEffectPrefab != null)
                {
                    Instantiate(collectionEffectPrefab, transform.position, Quaternion.identity);
                }
                
                // Sonido (si existe)
                if (collectSound != null)
                {
                    AudioSource.PlayClipAtPoint(collectSound, transform.position);
                }
                
                // Destruir el orbe
                Destroy(gameObject);
            }
        }
    }
}
