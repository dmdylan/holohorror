using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    #region Variables
    private int playerKeys;
    private int numberOfLocks;
    private float ameSpawnTimer;
    [SerializeField] private float ameSpawnTime = 60f;
    private Transform playerTransform;
    private string highScore;
    private string highScoreTime;
    private AudioSource audioSource;
    public bool isDebug = true;

    [Header("UI Setup")]
    [SerializeField] private TextMeshProUGUI keyCount = null;
    [SerializeField] private TextMeshProUGUI lockCount = null;
    [SerializeField] private GameObject gameOverPanel = null;
    [SerializeField] private GameObject victoryPanel = null;
    [SerializeField] private TextMeshProUGUI victoryTimeText = null;
    [SerializeField] private TextMeshProUGUI victoryHighScoreText = null;
    [SerializeField] private TextMeshProUGUI gameOverTimeText = null;
    [SerializeField] private TextMeshProUGUI gameOverHighScoreText = null;

    [Header("Prefabs")]
    [SerializeField] private GameObject keyPrefab = null;
    [SerializeField] private GameObject amePrefab = null;

    //TODO: Only need one point for lock/object spawn
    [Header("Spawn Points")]
    [SerializeField] private List<Transform> ameSpawnPoints = null;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip ameSpawnNoise = null;

    [Header("Scriptable Objects")]
    public DifficultySO difficulty;
    public FloatValue flashlightBattery;

    public int PlayerKeys => playerKeys;

    #endregion

    #region Start, Update, End calls
    private void Awake() => instance = this;

    // Start is called before the first frame update
    void Start()
    {
        GameSetup();
        UISetup();
    }

    private void OnEnable()
    {
        GameEvents.Instance.OnPickUpKey += Instance_OnPickUpKey;
        GameEvents.Instance.OnOpenALock += Instance_OnOpenALock;
        GameEvents.Instance.OnGameOver += Instance_OnGameOver;
        GameEvents.Instance.OnGameWin += Instance_OnGameWin;
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnPickUpKey -= Instance_OnPickUpKey;
        GameEvents.Instance.OnOpenALock -= Instance_OnOpenALock;
        GameEvents.Instance.OnGameOver -= Instance_OnGameOver;
        GameEvents.Instance.OnGameWin -= Instance_OnGameWin;
    }

    private void Update()
    {
        ameSpawnTimer += Time.deltaTime;

        if(ameSpawnTimer >= ameSpawnTime && difficulty.GameDifficulty == Difficulty.Gremlin)
        {
            ameSpawnTimer -= ameSpawnTime;
            SpawnNewAme();
        }
    }
    #endregion

    #region Misc Methods

    private void SpawnNewAme()
    {
        Transform newAmeSpawnLocation = null;

        foreach(Transform spawnLocation in ameSpawnPoints)
        {
            if(newAmeSpawnLocation == null)
            {
                newAmeSpawnLocation = spawnLocation;
                continue;
            }

            if(Vector3.Distance(spawnLocation.position, playerTransform.position) >= Vector3.Distance(newAmeSpawnLocation.position, playerTransform.position))
            {
                newAmeSpawnLocation = spawnLocation;
            }
            else
            {
                continue;
            }
        }

        Instantiate(amePrefab, newAmeSpawnLocation.position, newAmeSpawnLocation.rotation);
        audioSource.PlayOneShot(ameSpawnNoise);
    }

    void GameSetup()
    {
        audioSource = GetComponent<AudioSource>();
        difficulty.GameDifficulty = Difficulty.Gremlin;
        playerKeys = 0;
        flashlightBattery.Value = 100;
        Time.timeScale = 1;
        highScore = $"highscore{difficulty.GameDifficulty}";
        highScoreTime = $"highscoreTime{difficulty.GameDifficulty}";
        numberOfLocks = (int)difficulty.GameDifficulty;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //SpawnKeys((int)difficulty.GameDifficulty, keySpawnPoints);
        //SpawnLocks((int)difficulty.GameDifficulty);
    }

    void UISetup()
    {
        keyCount.text = playerKeys.ToString();
        lockCount.text = numberOfLocks.ToString();
    }

    private void SpawnKeys(int keyCount, List<Transform> spawnPoints)
    {
        for(int i = 0; i < keyCount; i++)
        {
            Transform spawnLocation = spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
            Instantiate(keyPrefab, spawnLocation.position, spawnLocation.rotation);
            spawnPoints.Remove(spawnLocation);
        }
    }

    void CheckForWin()
    {
        if (numberOfLocks == 0)
            GameEvents.Instance.GameWin();
    }

    void CheckKeyCount()
    {
        if(playerKeys == numberOfLocks)
        {
            GameEvents.Instance.AllKeysCollected();
        }
    }

    string GetTimeText()
    {
        int min = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60);
        int sec = Mathf.FloorToInt(Time.timeSinceLevelLoad % 60);
        int mil = Mathf.FloorToInt(Time.timeSinceLevelLoad * 1000) % 1000;
        string niceTime = min.ToString("00") + ":" + sec.ToString("00") + "." + mil.ToString("0");

        return niceTime;
    }

    void HighScoreSetup()
    {
        if (PlayerPrefs.HasKey(highScoreTime))
        {
            if (PlayerPrefs.GetFloat(highScoreTime) < Time.timeSinceLevelLoad)
            {
                PlayerPrefs.SetFloat(highScoreTime, Time.timeSinceLevelLoad);
                PlayerPrefs.SetString(highScore, GetTimeText());
            }
            else
                return;
        }
        else
        {
            PlayerPrefs.SetFloat(highScoreTime, Time.timeSinceLevelLoad);
            PlayerPrefs.SetString(highScore, GetTimeText());
        }
    }

    string ShowHighScore()
    {
        string highscore = "";

        if(PlayerPrefs.HasKey(highScore) == false)
        {
            highScore = "";
        }
        else
        {
            highscore = PlayerPrefs.GetString(highScore);
        }

        return highscore;
    }

    #endregion

    #region UI Methods

    public void PlayAgainButton()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(currentScene.name);
        SceneManager.SetActiveScene(currentScene);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");    
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }

    #endregion

    #region Event Methods
    private void Instance_OnOpenALock()
    {
        playerKeys--;
        keyCount.text = playerKeys.ToString();
        numberOfLocks--;
        lockCount.text = numberOfLocks.ToString();
        CheckForWin();
    }

    private void Instance_OnPickUpKey()
    {
        playerKeys++;
        keyCount.text = playerKeys.ToString();
        CheckKeyCount();
    }

    private void Instance_OnGameOver()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
        gameOverTimeText.text = $"Time elapsed: {GetTimeText()}";
        gameOverHighScoreText.text = $"High Score: {ShowHighScore()}";
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }    
    
    //TODO: Could save the time completed for a high score per level
    //TODO: Add you only won because ame was lagging
    private void Instance_OnGameWin()
    {
        HighScoreSetup();
        Time.timeScale = 0;
        victoryPanel.SetActive(true);
        victoryTimeText.text = $"You escaped in: {GetTimeText()}";
        victoryHighScoreText.text = $"High Score: {ShowHighScore()}";
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    #endregion
}
