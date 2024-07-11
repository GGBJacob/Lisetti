using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject ContinueButton;
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("UnlockedLevelID"))
            PlayerPrefs.SetInt("UnlockedLevelID", 0);
        if(!PlayerPrefs.HasKey("HighScoreLevel1"))
            PlayerPrefs.SetInt("HighScoreLevel1", 0);
        if(!PlayerPrefs.HasKey("HighScoreLevel2"))
            PlayerPrefs.SetInt("HighScoreLevel2", 0);
        if(!PlayerPrefs.HasKey("HighScoreLevel3"))
            PlayerPrefs.SetInt("HighScoreLevel3", 0);
        if(!PlayerPrefs.HasKey("BestTimeLevel1"))
            PlayerPrefs.SetFloat("BestTimeLevel1", 0);
        if(!PlayerPrefs.HasKey("BestTimeLevel2"))
            PlayerPrefs.SetFloat("BestTimeLevel2", 0);
        if(!PlayerPrefs.HasKey("BestTimeLevel3"))
            PlayerPrefs.SetFloat("BestTimeLevel3", 0);
        if (PlayerPrefs.GetInt("UnlockedLevelID") == 0)
            ContinueButton.GetComponent<Button>().interactable = false;
    }

    public void OnNewGameButtonPressed()
    {
        PlayerPrefs.SetInt("UnlockedLevelID", 0);//ustawia licznik przechowywany w pamiêci na 0 (wykorzystywany miêdzy scenami)
        PlayerPrefs.SetInt("HighScoreLevel1", 0);
        PlayerPrefs.SetInt("HighScoreLevel2", 0);
        PlayerPrefs.SetInt("HighScoreLevel3", 0);
        PlayerPrefs.SetFloat("BestTimeLevel1", 0);
        PlayerPrefs.SetFloat("BestTimeLevel2", 0);
        PlayerPrefs.SetFloat("BestTimeLevel3", 0);
        PlayerPrefs.SetInt("ActiveLevel", 0);//u¿ywane do sprawdzenia który poziom jest teraz w³¹czony (zapobiega sytuacji w której gracz przejdzie jeden poziom dwa razy i odblokuje kolejny)
        SceneManager.LoadSceneAsync("Tutorial");
    }

    public void OnContinueButtonPressed()
    {
        SceneManager.LoadSceneAsync("LevelSelector");
    }

    public void OnLevel1ButtonPressed()
    {
        SceneManager.LoadSceneAsync("Level 1");
    }

    public void OnExitToDesktopButtonPressed()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {
    }
}
