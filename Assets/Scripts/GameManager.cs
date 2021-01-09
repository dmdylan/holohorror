using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public IntValue playerKeys;
    public IntValue numberOfLocks;
    [SerializeField] private TextMeshProUGUI keyCount = null;

    #region Start, Update, End calls
    // Start is called before the first frame update
    void Start()
    {
        playerKeys.Value = 0;
        keyCount.text = playerKeys.Value.ToString();
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
        throw new System.NotImplementedException();
    }

    private void Instance_OnPickUpKey()
    {
        playerKeys.Value++;
        keyCount.text = playerKeys.Value.ToString();
    }
    #endregion
}
