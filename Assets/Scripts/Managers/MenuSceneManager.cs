using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class MenuSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel = null;
    [SerializeField] private GameObject settingsPanel = null;
    [SerializeField] private GameObject bestTimesPanel = null;
    [SerializeField] private GameObject mainPanelPositionObject = null;
    [SerializeField] private GameObject loadingPanelPositionObject = null;
    [SerializeField] private TMP_Dropdown difficultyDropdown = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private Slider sfxSlider = null;
    [SerializeField] private Slider sensitivitySlider = null;
    [SerializeField] private AudioClip[] hiccups = null;
    [SerializeField] private TextMeshProUGUI easyTimeOne = null;
    [SerializeField] private TextMeshProUGUI easyTimeTwo = null;
    [SerializeField] private TextMeshProUGUI normalTimeOne = null;
    [SerializeField] private TextMeshProUGUI normalTimeTwo = null;
    [SerializeField] private TextMeshProUGUI hardTimeOne = null;
    [SerializeField] private TextMeshProUGUI hardTimeTwo = null;
    [SerializeField] private TextMeshProUGUI gremlinTimeOne = null;
    [SerializeField] private TextMeshProUGUI gremlinTimeTwo = null;

    public DifficultySO difficulty;
    //private Vector3 mainPanelCameraPosition = new Vector3(3.27f, 2.22f, 2.30f);
    //private Vector3 mainPanelCameraRotation = new Vector3(10, 0, 0);
    //private Vector3 loadingPanelCameraPosition = new Vector3(-0.34f, 2.22f, 1.63f);
    //private Vector3 loadingPanelCameraRotation = new Vector3(10, -90, 0);
    private AsyncOperation loadGameScene;
    private AudioSource audioSource;
    private string playerSensitivity = "sensitivity";
    private string musicVolume = "musicVolume";
    private string sfxVolume = "sfxVolume";
    private string difficultyString = "Difficulty";

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        loadGameScene = new AsyncOperation();
        //mainCamera.SetActive(true);
        //gameLoadingCamera.SetActive(false);

        if (PlayerPrefs.HasKey(difficultyString) == false && PlayerPrefs.HasKey(musicVolume) == false 
            && PlayerPrefs.HasKey(sfxVolume) == false && PlayerPrefs.HasKey(playerSensitivity) == false)
        {
            PlayerPrefs.SetInt(difficultyString, 4);
            PlayerPrefs.SetFloat(musicVolume, .5f);
            PlayerPrefs.SetFloat(sfxVolume, .5f);
            PlayerPrefs.SetFloat(playerSensitivity, 4.5f);
        }

        SetHighScores();

        difficultyDropdown.value = PlayerPrefs.GetInt(difficultyString) - 3;
        difficulty.GameDifficulty = (Difficulty)PlayerPrefs.GetInt(difficultyString);
        
        volumeSlider.value = PlayerPrefs.GetFloat(musicVolume);
        sfxSlider.value = PlayerPrefs.GetFloat(sfxVolume);
        sensitivitySlider.value = PlayerPrefs.GetFloat(playerSensitivity);
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
        Camera.main.transform.position = loadingPanelPositionObject.transform.position;
        Camera.main.transform.rotation = loadingPanelPositionObject.transform.rotation;
    }

    public void ContinueButton()
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

    public void LoadGameBackButton()
    {
        Camera.main.transform.position = mainPanelPositionObject.transform.position;
        Camera.main.transform.rotation = mainPanelPositionObject.transform.rotation;
    }

    public void GoToHighScorePanel()
    {
        mainPanel.SetActive(false);
        bestTimesPanel.SetActive(true);
    }

    public void ReturnFromHighScorePanel()
    {
        bestTimesPanel.SetActive(false);
        mainPanel.SetActive(true);
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

    public void SetSensitivity()
    {
        PlayerPrefs.SetFloat(playerSensitivity, sensitivitySlider.value);
    }

    public void PlayHiccup()
    {
        audioSource.PlayOneShot(hiccups[Random.Range(0, hiccups.Length - 1)], PlayerPrefs.GetFloat(sfxVolume));
    }

    private IEnumerator LoadTheGame()
    {
        loadGameScene = SceneManager.LoadSceneAsync(1);
        loadGameScene.allowSceneActivation = false;

        yield return new WaitUntil(() => loadGameScene.isDone);
    }

    private void SetHighScores()
    {
        if(PlayerPrefs.HasKey($"highscore{Difficulty.Easy}") == false)
        {
            easyTimeOne.text = "---";
            easyTimeTwo.text = "---";
        }
        else
        {
            easyTimeOne.text = PlayerPrefs.GetString($"highscore{Difficulty.Easy}");
            easyTimeTwo.text = PlayerPrefs.GetString($"highscore{Difficulty.Easy}");
        }

        if (PlayerPrefs.HasKey($"highscore{Difficulty.Normal}") == false)
        {
            normalTimeOne.text = "---";
            normalTimeTwo.text = "---";
        }
        else
        {
            normalTimeOne.text = PlayerPrefs.GetString($"highscore{Difficulty.Normal}");
            normalTimeTwo.text = PlayerPrefs.GetString($"highscore{Difficulty.Normal}");
        }

        if (PlayerPrefs.HasKey($"highscore{Difficulty.Hard}") == false)
        {
            hardTimeOne.text = "---";
            hardTimeTwo.text = "---";
        }
        else
        {
            hardTimeOne.text = PlayerPrefs.GetString($"highscore{Difficulty.Hard}");
            hardTimeTwo.text = PlayerPrefs.GetString($"highscore{Difficulty.Hard}");
        }

        if (PlayerPrefs.HasKey($"highscore{Difficulty.Gremlin}") == false)
        {
            gremlinTimeOne.text = "---";
            gremlinTimeTwo.text = "---";
        }
        else
        {
            gremlinTimeOne.text = PlayerPrefs.GetString($"highscore{Difficulty.Gremlin}");
            gremlinTimeTwo.text = PlayerPrefs.GetString($"highscore{Difficulty.Gremlin}");
        }
    }
}
