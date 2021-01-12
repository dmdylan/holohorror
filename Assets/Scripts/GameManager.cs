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

    public IntValue playerKeys;
    public IntValue numberOfLocks;
    [SerializeField] private TextMeshProUGUI keyCount = null;
    [SerializeField] private TextMeshProUGUI lockCount = null;

    #region Start, Update, End calls
    private void Awake() => instance = this;

    // Start is called before the first frame update
    void Start()
    {
        //TODO: Setup way of auto adujusting keys, locks, and key spawns
        playerKeys.Value = 0;
        numberOfLocks.Value = 3;
        keyCount.text = playerKeys.Value.ToString();
        lockCount.text = numberOfLocks.Value.ToString();
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
    #endregion

    #region Event Methods
    private void Instance_OnOpenALock()
    {
        playerKeys.Value--;
        keyCount.text = playerKeys.Value.ToString();
        numberOfLocks.Value--;
        lockCount.text = numberOfLocks.Value.ToString();
        CheckForWin();
    }

    private void Instance_OnPickUpKey()
    {
        playerKeys.Value++;
        keyCount.text = playerKeys.Value.ToString();
    }
    #endregion

    //TODO: Setup actual win
    void CheckForWin()
    {
        if (numberOfLocks.Value == 0)
            Debug.Log("You win!");
    }
}
