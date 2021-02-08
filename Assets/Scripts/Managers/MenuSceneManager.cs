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
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private Slider sfxSlider = null;
    [SerializeField] private AudioClip[] hiccups = null;
    public DifficultySO difficulty;

    private AudioSource audioSource;
    private string musicVolume = "musicVolume";
    private string sfxVolume = "sfxVolume";
    private string difficultyString = "Difficulty";

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if(PlayerPrefs.HasKey(difficultyString) == false && PlayerPrefs.HasKey(musicVolume) == false && PlayerPrefs.HasKey(sfxVolume) == false)
        {
            PlayerPrefs.SetInt(difficultyString, 4);
            PlayerPrefs.SetFloat(musicVolume, .5f);
            PlayerPrefs.SetFloat(sfxVolume, .5f);
        }

        difficultyDropdown.value = PlayerPrefs.GetInt(difficultyString) -3;
        difficulty.GameDifficulty = (Difficulty)PlayerPrefs.GetInt(difficultyString);
        
        volumeSlider.value = PlayerPrefs.GetFloat(musicVolume);
        sfxSlider.value = PlayerPrefs.GetFloat(sfxVolume);
    }

    private void Update()
    {
        audioSource.volume = PlayerPrefs.GetFloat(musicVolume);
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

    public void SettingsReturnButton()
    {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void SetDifficulty()
    {
        difficulty.GameDifficulty = (Difficulty)difficultyDropdown.value + 3;
        PlayerPrefs.SetInt(difficultyString, ((int)difficulty.GameDifficulty));
    }

    public void SetMusicVolume()
    {
        PlayerPrefs.SetFloat(musicVolume, volumeSlider.value);
    }

    public void SetSFXVolume()
    {
        PlayerPrefs.SetFloat(sfxVolume, sfxSlider.value);
    }

    public void PlayHiccup()
    {
        audioSource.PlayOneShot(hiccups[Random.Range(0, hiccups.Length - 1)], PlayerPrefs.GetFloat(sfxVolume));
    }
}
