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
    private float sfxVolume;
    private float musicVolume;
    private int numberOfMagnifyingGlassesCollected;
    private int numberOfMagnifyingGlassesNeeded;
    private float ameSpawnTimer;
    [SerializeField] private float ameSpawnTime = 60f;
    private Transform playerTransform;
    private string highScore;
    private string highScoreTime;
    private AudioSource audioSource;
    private bool gameOverNoisePlayed = false;
    public bool isDebug = true;

    [Header("UI Setup")]
    [SerializeField] private TextMeshProUGUI batteryLevel = null;
    [SerializeField] private TextMeshProUGUI magnifyingGlassesNeededText = null;
    [SerializeField] private GameObject gameOverPanel = null;
    [SerializeField] private GameObject victoryPanel = null;
    [SerializeField] private TextMeshProUGUI victoryTimeText = null;
    [SerializeField] private TextMeshProUGUI victoryHighScoreText = null;
    [SerializeField] private TextMeshProUGUI victoryDifficultyText = null;
    [SerializeField] private TextMeshProUGUI gameOverTimeText = null;
    [SerializeField] private TextMeshProUGUI gameOverHighScoreText = null;
    [SerializeField] private TextMeshProUGUI gameOverDifficultyText = null;

    [Header("Prefabs")]
    [SerializeField] private GameObject amePrefab = null;

    [Header("Spawn Points")]
    [SerializeField] private List<Transform> ameSpawnPoints = null;
    [SerializeField] private List<GameObject> magnifyingGlasses = null;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip ameSpawnNoise = null;
    [SerializeField] private AudioClip[] hiccups = null;
    [SerializeField] private AudioClip youWinNoise = null;
    [SerializeField] private AudioClip watchCharged = null;
    [SerializeField] private AudioClip useTimeMachine = null;
    [SerializeField] private AudioClip groundPound = null;

    [Header("End Objective")]
    [SerializeField] private GameObject victoryArea = null;

    [Header("Scriptable Objects")]
    public DifficultySO difficulty;
    public FloatValue flashlightBattery;

    public int PlayerKeys => numberOfMagnifyingGlassesCollected;
    public float SfxVolume => sfxVolume;
    public float MusicVolume => musicVolume;

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
        GameEvents.Instance.OnGameOver += Instance_OnGameOver;
        GameEvents.Instance.OnGameWin += Instance_OnGameWin;
        GameEvents.Instance.OnAllKeysCollected += Instance_OnAllKeysCollected;
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnPickUpKey -= Instance_OnPickUpKey;
        GameEvents.Instance.OnGameOver -= Instance_OnGameOver;
        GameEvents.Instance.OnGameWin -= Instance_OnGameWin;
        GameEvents.Instance.OnAllKeysCollected -= Instance_OnAllKeysCollected;
    }

    private void Update()
    {
        ameSpawnTimer += Time.deltaTime;
        batteryLevel.text = flashlightBattery.Value.ToString("0");

        if(ameSpawnTimer >= ameSpawnTime && difficulty.GameDifficulty == Difficulty.Gremlin)
        {
            ameSpawnTimer -= ameSpawnTime;
            SpawnNewAme();
        }
    }
    #endregion

    #region Misc Methods

    IEnumerator PlayEndNoises()
    {
        audioSource.PlayOneShot(useTimeMachine, SfxVolume);
        yield return new WaitForSeconds(useTimeMachine.length);
        audioSource.PlayOneShot(youWinNoise, SfxVolume);
    }

    IEnumerator PlayGameOverNoise()
    {
        if (gameOverNoisePlayed != false)
            yield break;

        audioSource.PlayOneShot(groundPound, SfxVolume);
        yield return new WaitForSeconds(groundPound.length);
        gameOverNoisePlayed = true;
    }

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
        musicVolume = PlayerPrefs.GetFloat("musicVolume");
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = musicVolume;
        difficulty.GameDifficulty = difficulty.GameDifficulty;
        numberOfMagnifyingGlassesCollected = 0;
        flashlightBattery.Value = 100;
        Time.timeScale = 1;
        highScore = $"highscore{difficulty.GameDifficulty}";
        highScoreTime = $"highscoreTime{difficulty.GameDifficulty}";
        numberOfMagnifyingGlassesNeeded = (int)difficulty.GameDifficulty;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        EnableMagnifyingGlasses((int)difficulty.GameDifficulty, magnifyingGlasses);
    }

    void UISetup()
    {
        magnifyingGlassesNeededText.text = numberOfMagnifyingGlassesNeeded.ToString();
    }

    private void EnableMagnifyingGlasses(int keyCount, List<GameObject> mg)
    {
        List<GameObject> tmpList = new List<GameObject>(mg);

        List<GameObject> newList = new List<GameObject>();

        while (newList.Count < keyCount && tmpList.Count > 0)
        {
            int index = Random.Range(0, tmpList.Count);
            newList.Add(tmpList[index]);
            tmpList.RemoveAt(index);
        }

        foreach(var glass in newList)
        {
            glass.SetActive(true);
        }
    }

    void CheckMGCount()
    {
        if(numberOfMagnifyingGlassesNeeded == 0)
        {
            GameEvents.Instance.AllKeysCollected();
            magnifyingGlassesNeededText.text = "Escape!";
        }
    }

    string GetTimeText()
    {
        int min = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60);
        int sec = Mathf.FloorToInt(Time.timeSinceLevelLoad % 60);
        int mil = Mathf.FloorToInt(Time.timeSinceLevelLoad * 1000) % 1000;
        string niceTime = min.ToString("00") + ":" + sec.ToString("00") + "." + mil.ToString("00");

        return niceTime;
    }

    void HighScoreSetup()
    {
        if (PlayerPrefs.HasKey(highScoreTime))
        {
            if (PlayerPrefs.GetFloat(highScoreTime) > Time.timeSinceLevelLoad)
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
        SceneManager.LoadScene(0);
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }

    public void PlayHiccup()
    {
        audioSource.PlayOneShot(hiccups[Random.Range(0, hiccups.Length - 1)], SfxVolume);       
    }

    #endregion

    #region Event Methods
    private void Instance_OnPickUpKey()
    {
        numberOfMagnifyingGlassesNeeded--;
        magnifyingGlassesNeededText.text = numberOfMagnifyingGlassesNeeded.ToString();
        CheckMGCount();
    }

    private void Instance_OnAllKeysCollected()
    {
        audioSource.PlayOneShot(watchCharged, SfxVolume);
        victoryArea.SetActive(true);
        Debug.Log("victory area open");
    }

    private void Instance_OnGameOver()
    {
        StartCoroutine(PlayGameOverNoise());
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
        gameOverDifficultyText.text = $"Difficulty: {difficulty.GameDifficulty}";
        gameOverTimeText.text = $"Time elapsed: {GetTimeText()}";
        gameOverHighScoreText.text = $"Best time: {ShowHighScore()}";
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
        victoryDifficultyText.text = $"Difficulty: {difficulty.GameDifficulty}";
        victoryTimeText.text = $"You escaped in: {GetTimeText()}";
        victoryHighScoreText.text = $"Best time: {ShowHighScore()}";
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        StartCoroutine(PlayEndNoises());
    }
    #endregion
}
