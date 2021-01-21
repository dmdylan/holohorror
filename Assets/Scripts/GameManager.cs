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

    private int playerKeys;
    private int numberOfLocks;
    private Transform playerTransform;
    private Vector3 playerPosition;
    [SerializeField] private TextMeshProUGUI keyCount = null;
    [SerializeField] private TextMeshProUGUI lockCount = null;
    [SerializeField] private GameObject keyPrefab = null;
    [SerializeField] private GameObject lockPrefab = null;
    [SerializeField] private List<Transform> keySpawnPoints = null;
    [SerializeField] private List<Transform> lockSpawnPointsEasy = null;
    [SerializeField] private List<Transform> lockSpawnPointsNormal = null;
    [SerializeField] private List<Transform> lockSpawnPointsHard = null;
    private GameObject gameEndPanel = null;

    public DifficultySO difficulty;
    public Vector3 PlayerPosition => playerPosition;
    public int PlayerKeys => playerKeys;

    #region Start, Update, End calls
    private void Awake() => instance = this;

    // Start is called before the first frame update
    void Start()
    {
        //TODO: Break these into UI/Game/Misc Setup methods
        playerKeys = 0;
        Time.timeScale = 1;
        numberOfLocks = (int)difficulty.GameDifficulty;
        keyCount.text = playerKeys.ToString();
        lockCount.text = numberOfLocks.ToString();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        gameEndPanel = GameObject.Find("GameEndPanel");
        gameEndPanel.SetActive(false);
        SpawnKeys((int)difficulty.GameDifficulty, keySpawnPoints);
        SpawnLocks((int)difficulty.GameDifficulty);
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
        playerPosition = playerTransform.transform.position;
    }
    #endregion

    private void SpawnKeys(int keyCount, List<Transform> spawnPoints)
    {
        for(int i = 0; i < keyCount; i++)
        {
            Transform spawnLocation = spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
            Instantiate(keyPrefab, spawnLocation.position, spawnLocation.rotation);
            spawnPoints.Remove(spawnLocation);
        }
    }

    private void SpawnLocks(int difficultyLevel)
    {
        if(difficultyLevel == 3)
        {
            foreach(Transform spawn in lockSpawnPointsEasy)
            {
                Instantiate(lockPrefab, spawn.position, spawn.rotation);
            }
        }

        if (difficultyLevel == 4)
        {
            foreach (Transform spawn in lockSpawnPointsNormal)
            {
                Instantiate(lockPrefab, spawn.position, spawn.rotation);
            }
        }

        if (difficultyLevel == 5)
        {
            foreach (Transform spawn in lockSpawnPointsHard)
            {
                Instantiate(lockPrefab, spawn.position, spawn.rotation);
            }
        }
    }

    //TODO: Setup actual win
    void CheckForWin()
    {
        if (numberOfLocks == 0)
            Debug.Log("You win!");
    }

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
    }

    //TODO: Implement lose condition and lose sequence
    private void Instance_OnGameOver()
    {
        Time.timeScale = 0;
        gameEndPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }    
    
    private void Instance_OnGameWin()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
