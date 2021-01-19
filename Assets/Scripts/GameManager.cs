using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    [SerializeField] private List<Transform> keySpawnPoints = null;

    public DifficultySO difficulty;
    public Vector3 PlayerPosition => playerPosition;
    public int PlayerKeys => playerKeys;

    #region Start, Update, End calls
    private void Awake() => instance = this;

    // Start is called before the first frame update
    void Start()
    {
        //TODO: Setup way of auto adujusting keys, locks, and key spawns
        playerKeys = 0;
        numberOfLocks = (int)difficulty.GameDifficulty;
        keyCount.text = playerKeys.ToString();
        lockCount.text = numberOfLocks.ToString();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        SpawnKeys((int)difficulty.GameDifficulty, keySpawnPoints);
    }

    private void OnEnable()
    {
        GameEvents.Instance.OnPickUpKey += Instance_OnPickUpKey;
        GameEvents.Instance.OnOpenALock += Instance_OnOpenALock;
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnPickUpKey -= Instance_OnPickUpKey;
        GameEvents.Instance.OnOpenALock -= Instance_OnOpenALock;
    }

    private void Update()
    {
        playerPosition = playerTransform.transform.position;
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

    //TODO: Setup actual win
    void CheckForWin()
    {
        if (numberOfLocks == 0)
            Debug.Log("You win!");
    }
}
