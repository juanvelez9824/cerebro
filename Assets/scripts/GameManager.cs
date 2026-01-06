using UnityEngine;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Text statusText;
    [SerializeField] private Text serotoninTimerText;
    [SerializeField] private Text ruminatingCountText;
    [SerializeField] private Image dashCooldownBar;
    
    [Header("Level Settings")]
    [SerializeField] private int serotoninOrbsCollected = 0;
    [SerializeField] private int totalSerotoninOrbs = 5;
    [SerializeField] private bool levelCompleted = false;
    
    private PlayerController player;
    private DepressionCameraController cameraController;
    
    public static GameManager Instance { get; private set; }
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Encontrar referencias
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerController>();
        }
        
        cameraController = Camera.main.GetComponent<DepressionCameraController>();
        
        UpdateUI();
    }
    
    void Update()
    {
        UpdateUI();
        
        // Reiniciar nivel
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
        
        // Salir del juego
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    
    void UpdateUI()
    {
        if (statusText != null)
        {
            string status = levelCompleted ? "¡NIVEL COMPLETADO!" : "Recolecta orbes de serotonina";
            statusText.text = status;
        }
        
        // Aquí podrías actualizar más elementos de UI
        // basándote en el estado del jugador
    }
    
    public void OnSerotoninCollected()
    {
        serotoninOrbsCollected++;
        
        if (serotoninOrbsCollected >= totalSerotoninOrbs)
        {
            CompleteLevel();
        }
    }
    
    void CompleteLevel()
    {
        if (!levelCompleted)
        {
            levelCompleted = true;
            Debug.Log("¡Nivel completado! La niebla se despeja...");
            
            // Aquí podrías:
            // - Cambiar la iluminación del nivel
            // - Activar una animación de victoria
            // - Abrir un portal al siguiente nivel
            // - Mostrar estadísticas
        }
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void LoadNextLevel()
    {
        // Cargar el siguiente nivel (Ansiedad)
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
