using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UIScript : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    private int score = 0;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private Button startButton;
    [SerializeField] private Canvas gameStartCanvas;
    private bool gameInProgress = false;
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private Button restartGameButton;
    // [SerializeField] private Canvas pauseCanvas;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Canvas settingsCanvas;

    [SerializeField] private Button settingsButton;
    [SerializeField] private Button closeSettingsButton;

    [SerializeField] private Button returnToHomeButton;

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioClip backgroundMusicClip;
    private bool isMusicMuted = false;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private TMP_Text musicVolumeText;
    [SerializeField] private AudioMixer audioMixer;
    private float currentMusicVolume = 0.5f;

    private GameInputActions inputActions;

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score.ToString();
    }
    
    private void UpdateHighScore()
    {
        if (score > PlayerPrefs.GetInt("High Score", 0))
        {
            PlayerPrefs.SetInt("High Score", score);
            highScoreText.text = "High Score: " + score.ToString();
        }
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        Debug.Log("EndGame: Showing gameOverCanvas");
        gameOverCanvas.enabled = true;
        UpdateHighScore();
        inputActions.GamePlay.Disable();
        inputActions.UI.TogglePause.Disable();
        inputActions.UI.RestartGame.Enable();
        PauseMusic();
        gameInProgress = false;
        pauseButton.gameObject.SetActive(false);
    }
    
    private void RestartGame()
    {
        Debug.Log("RestartGame called");
        Debug.Log("RestartGame: Hiding gameOverCanvas and pausePanel");
        gameOverCanvas.enabled = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        inputActions.UI.RestartGame.Disable();
        inputActions.UI.TogglePause.Enable();
        inputActions.GamePlay.Enable();
        UnpauseMusic();
        gameInProgress = true;
        pauseButton.gameObject.SetActive(true);
        score = 0;
        scoreText.text = "Score: 0";
    }

    private void StartGame()
    {
        Debug.Log("StartGame: Hiding gameStartCanvas");
        gameStartCanvas.enabled = false;
        Time.timeScale = 1;
        inputActions.GamePlay.Enable();
        inputActions.UI.TogglePause.Enable();
        inputActions.UI.RestartGame.Disable();
        PlayMusic();
        gameInProgress = true;
        pauseButton.gameObject.SetActive(true);
        score = 0;
        scoreText.text = "Score: 0";
    }

    private void ReturnToHome()
    {
        Debug.Log("ReturnToHome: Showing gameStartCanvas, hiding gameOverCanvas, pauseCanvas, settingsCanvas");
        gameStartCanvas.enabled = true;
        gameOverCanvas.enabled = false;
        // pauseCanvas.enabled = false;
        settingsCanvas.enabled = false;
        Time.timeScale = 0;
        inputActions.GamePlay.Disable();
        inputActions.UI.Disable();
        PauseMusic();
        gameInProgress = false;
        pauseButton.gameObject.SetActive(false);
        pausePanel.SetActive(false);
    }

    private void Pause()
    {
        Debug.Log("Pause: Showing pausePanel");
        pausePanel.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        Time.timeScale = 0;
        inputActions.GamePlay.Disable();
    }   

    private void Unpause()
    {
        Debug.Log("Unpause: Hiding pausePanel");
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        inputActions.GamePlay.Enable();
    }

    private void OpenSettings()
    {
        Debug.Log("OpenSettings: Showing settingsCanvas");
        // musicVolumeText.text = currentMusicVolume.ToString("0.00");
        // musicVolumeSlider.value = currentMusicVolume;
        settingsCanvas.enabled = true;
        inputActions.UI.TogglePause.Disable();
    }  

    private void CloseSettings()
    {
        Debug.Log("CloseSettings: Hiding settingsCanvas");
        settingsCanvas.enabled = false;
        inputActions.UI.TogglePause.Enable();
    }   
    
    private void ToggleMusic(bool isOn)
    {
        isMusicMuted = !isOn;
        if (isMusicMuted)
        {
            musicAudioSource.mute = true;
            isMusicMuted = false;
        }
        else
        {
            musicAudioSource.mute = false;
            isMusicMuted = true;
        }
    }

    private void ChangeMusicVolume(float volume)
    {
        currentMusicVolume = volume; 
        musicVolumeText.text = volume.ToString("0.00");
        float minDb = -80f;
        float db;
        if (volume <= 0.0001f)
        {
            db = minDb;
        }
       else
       {
            db = Mathf.Log10(volume) * 20;
       }        
       audioMixer.SetFloat("MusicVolume", db);
    }

    private void PlayMusic()
    {
        musicAudioSource.clip = backgroundMusicClip;
        musicAudioSource.loop = true;
        musicAudioSource.volume = 0.2f;
        musicAudioSource.Play();
    }

    private void PauseMusic()
    {
        musicAudioSource.Pause();
    }

    private void UnpauseMusic()
    {
        musicAudioSource.UnPause();
    }
    
    public bool getGameInProgress()
    {
        return gameInProgress;
    }
    
    void Awake()
    {
        inputActions = new GameInputActions();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ReturnToHome();
        scoreText.text = "Score: 0";
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("High Score", 0).ToString();
        musicVolumeSlider.value = currentMusicVolume;
        musicVolumeText.text = currentMusicVolume.ToString("0.00");
        ChangeMusicVolume(currentMusicVolume);
        gameOverCanvas.enabled = false;
        settingsCanvas.enabled = false;
        pauseButton.gameObject.SetActive(false); 
        pausePanel.SetActive(false);

    }

    void OnEnable()
    {
        startButton.onClick.AddListener(StartGame);
        pauseButton.onClick.AddListener(Pause);
        restartGameButton.onClick.AddListener(RestartGame);
        restartButton.onClick.AddListener(RestartGame);
        resumeButton.onClick.AddListener(Unpause);
        settingsButton.onClick.AddListener(OpenSettings);
        closeSettingsButton.onClick.AddListener(CloseSettings);
        returnToHomeButton.onClick.AddListener(ReturnToHome);
        
        inputActions.UI.TogglePause.performed += ctx =>
        {
            if (pausePanel.activeSelf)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        };

        inputActions.UI.RestartGame.performed += ctx => RestartGame();

        musicToggle.onValueChanged.AddListener(ToggleMusic);
        musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
    }

    void OnDisable()
    {
        startButton.onClick.RemoveListener(StartGame);
        pauseButton.onClick.RemoveListener(Pause);
        restartGameButton.onClick.RemoveListener(RestartGame);
        restartButton.onClick.RemoveListener(RestartGame);
        resumeButton.onClick.RemoveListener(Unpause);
        settingsButton.onClick.RemoveListener(OpenSettings);
        closeSettingsButton.onClick.RemoveListener(CloseSettings);
        returnToHomeButton.onClick.RemoveListener(ReturnToHome);
        
        inputActions.UI.TogglePause.performed -= ctx =>
        {
            if (pausePanel.activeSelf)
            {
                Unpause();
            }
            else
            {
                Pause();
            }   
        };
        inputActions.UI.RestartGame.performed -= ctx => RestartGame();
        musicToggle.onValueChanged.RemoveListener(ToggleMusic);
        musicVolumeSlider.onValueChanged.RemoveListener(ChangeMusicVolume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
