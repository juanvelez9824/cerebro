using UnityEngine;

public class DepressionCameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;
    
    [Header("Camera Settings")]
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
    
    [Header("Depression Effect - Claustrophobia")]
    [SerializeField] private float depressedZoom = 3f; // Más cerca = más claustrofóbico
    [SerializeField] private float normalZoom = 5f;
    [SerializeField] private float currentZoom;
    [SerializeField] private float zoomSpeed = 1f;
    
    [Header("Color Grading - Depression")]
    [SerializeField] private bool useDepressedColors = true;
    [SerializeField] private float desaturation = 0.5f; // 0 = color normal, 1 = blanco y negro
    
    private Camera cam;
    private bool isPlayerBoosted = false;
    
    void Start()
    {
        cam = GetComponent<Camera>();
        
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
        
        // Iniciar con zoom depresivo
        currentZoom = depressedZoom;
        cam.orthographicSize = currentZoom;
    }
    
    void LateUpdate()
    {
        if (player == null) return;
        
        // Seguir al jugador suavemente
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        
        // Ajustar zoom según el estado del jugador
        UpdateZoom();
    }
    
    void UpdateZoom()
    {
        // Verificar si el jugador tiene boost de serotonina
        // (Esto debería conectarse con el PlayerController)
        float targetZoom = isPlayerBoosted ? normalZoom : depressedZoom;
        
        // Transición suave del zoom
        currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSpeed);
        cam.orthographicSize = currentZoom;
    }
    
    public void SetPlayerBoosted(bool boosted)
    {
        isPlayerBoosted = boosted;
    }
    
    // Método alternativo para reducir saturación (requiere Post-Processing Stack)
    // O puedes usar un Material/Shader en la cámara
}