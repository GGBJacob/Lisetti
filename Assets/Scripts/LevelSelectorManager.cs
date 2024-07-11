using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectorManager : MonoBehaviour
{
    [SerializeField] GameObject[] Buttons;
    [Header("Level 1")]
    public TMP_Text Level1BestStats;
    [Header("Level 2")]
    public TMP_Text Level2BestStats;
    [Header("Level 3")]
    public TMP_Text Level3BestStats;
    public void Start()
    {
        Debug.Log("Best score: " + PlayerPrefs.GetInt("HighScoreLevel1"));
        Debug.Log("Best time:" + PlayerPrefs.GetInt("BestTimeLevel1"));
        Debug.Log(PlayerPrefs.GetInt("UnlockedLevelID"));
        for(int i = 0; i < Buttons.Length; i++) 
        {
            if(PlayerPrefs.GetInt("UnlockedLevelID") <= i)//sprawdza czy obecny poziom jest odblokowany
                Buttons[i].GetComponent<Button>().interactable = false; //blokuje przycisk jeœli nie
                 else
                Buttons[i].GetComponent<Button>().interactable = true;
        }
            
        //LEVEL 1

        float timer1 = PlayerPrefs.GetFloat("BestTimeLevel1");
        int highScore1 = PlayerPrefs.GetInt("HighScoreLevel1");
        int minutes1 = Mathf.FloorToInt(timer1 / 60F);
        int seconds1 = Mathf.FloorToInt(timer1 - minutes1 * 60);
        int milliseconds1 = (Mathf.FloorToInt(timer1 * 1000) % 1000) / 10;
        string bestScoreFormatted1 = string.Format("Best score: {0}", highScore1);
        string bestTimeFormatted1 = string.Format("Best time: {0}:{1:00}:{2:000}", minutes1, seconds1, milliseconds1);

        string displayText1 = $"{bestScoreFormatted1}\n{bestTimeFormatted1}";

        Level1BestStats.text = displayText1;

        //LEVEL 2

        float timer2 = PlayerPrefs.GetFloat("BestTimeLevel2");
        int highScore2 = PlayerPrefs.GetInt("HighScoreLevel2");
        int minutes2 = Mathf.FloorToInt(timer2 / 60F);
        int seconds2 = Mathf.FloorToInt(timer2 - minutes2 * 60);
        int milliseconds2 = (Mathf.FloorToInt(timer2 * 1000) % 1000) / 10;
        string bestScoreFormatted2 = string.Format("Best score: {0}", highScore2);
        string bestTimeFormatted2 = string.Format("Best time: {0}:{1:00}:{2:000}", minutes2, seconds2, milliseconds2);

        string displayText2 = $"{bestScoreFormatted2}\n{bestTimeFormatted2}";

        Level2BestStats.text = displayText2;

        //LEVEL 3

        float timer3 = PlayerPrefs.GetFloat("BestTimeLevel3");
        int highScore3 = PlayerPrefs.GetInt("HighScoreLevel3");
        int minutes3 = Mathf.FloorToInt(timer3 / 60F);
        int seconds3 = Mathf.FloorToInt(timer3 - minutes3 * 60);
        int milliseconds3 = (Mathf.FloorToInt(timer3 * 1000) % 1000) / 10;
        string bestScoreFormatted3 = string.Format("Best score: {0}", highScore3);
        string bestTimeFormatted3 = string.Format("Best time: {0}:{1:00}:{2:000}", minutes3, seconds3, milliseconds3);

        string displayText3 = $"{bestScoreFormatted3}\n{bestTimeFormatted3}";

        Level3BestStats.text = displayText3;

    }
    public void OnTutorialButtonPressed()
    {
        PlayerPrefs.SetInt("ActiveLevel", 0);//u¿ywane do sprawdzenia który poziom jest teraz w³¹czony (zapobiega sytuacji w której gracz przejdzie jeden poziom dwa razy i odblokuje kolejny)
        SceneManager.LoadSceneAsync("Tutorial");
    }
    public void OnLevel1ButtonPressed()
    {
        PlayerPrefs.SetInt("ActiveLevel", 1);
        SceneManager.LoadSceneAsync("Level 1", LoadSceneMode.Single);
    }
    public void OnLevel2ButtonPressed()
    {
        PlayerPrefs.SetInt("ActiveLevel", 2);
        SceneManager.LoadSceneAsync("Level 2", LoadSceneMode.Single);
    }
    public void OnLevel3ButtonPressed()
    {
        PlayerPrefs.SetInt("ActiveLevel", 3);
        SceneManager.LoadSceneAsync("Level 3", LoadSceneMode.Single);
    }

    public void OnReturnButtonPressed()
    {
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }
}
