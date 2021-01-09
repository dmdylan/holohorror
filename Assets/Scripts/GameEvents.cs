using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    static GameEvents instance;

    public static GameEvents Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameEvents>();
            }
            return instance;
        }
    }

    private void Awake() => instance = this;

    public event Action OnPickUpKey;
    public void PickedUpAKey() => OnPickUpKey?.Invoke();

    public event Action OnOpenALock;
    public void OpenedALock() => OnOpenALock?.Invoke();
}
