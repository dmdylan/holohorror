using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel = null;
    [SerializeField] private GameObject settingsPanel = null;
    [SerializeField] private TMP_Dropdown difficultyDropdown = null;
    [SerializeField] private Slider soundSlider = null;
    public DifficultySO difficulty;

    private string volume = "Volume";
    private string difficultyString = "Difficulty";

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey(difficultyString) == false && PlayerPrefs.HasKey(volume) == false)
        {
            PlayerPrefs.SetInt(difficultyString, 4);
            PlayerPrefs.SetFloat(volume, .5f);
        }

        difficultyDropdown.value = PlayerPrefs.GetInt(difficultyString) -3;
        difficulty.GameDifficulty = (Difficulty)PlayerPrefs.GetInt(difficultyString);
        
        soundSlider.value = PlayerPrefs.GetFloat(volume);
    }

    private void Update()
    {
        Debug.Log(difficulty.GameDifficulty);
        Debug.Log((int)difficulty.GameDifficulty);
        Debug.Log(PlayerPrefs.GetInt(difficultyString));
        Debug.Log(difficultyDropdown.value);
    }

    public void PlayGameButton()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }

    public void SettingsButton()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    //TODO: Could make it so settings change as on drop down changes
    public void SettingsReturnButton()
    {
        difficulty.GameDifficulty = (Difficulty)difficultyDropdown.value + 3;
        PlayerPrefs.SetInt(difficultyString, ((int)difficulty.GameDifficulty));
        PlayerPrefs.SetFloat(volume, soundSlider.value);
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }
}
