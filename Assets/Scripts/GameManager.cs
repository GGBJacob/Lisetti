using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq.Expressions;
using UnityEngine.Video;
using Unity.VisualScripting;

public enum GameState {GS_PAUSE_MENU, GS_GAME, GS_LEVEL_COMPLETED, GS_GAME_OVER, GS_OPTIONS }

public class GameManager : MonoBehaviour
{
    public bool isImmortal = false;
    public static GameManager instance;
    public GameState currentGameState = GameState.GS_GAME;
    [Header("Canvases")]
    public Canvas inGameCanvas;
    public Canvas PauseMenuCanvas;
    public Canvas levelCompletedCanvas;
    public Canvas optionsCanvas;
    public Canvas GameOverCanvas;
    public GameObject MissingKeysBox;
    //public Canvas CutsceneCanvas;
    //private float cutsceneTime = 5.0f;
    [Header("Text objects")]
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public TMP_Text highScoreText;
    public TMP_Text timerText;
    public TMP_Text enemiesText;
    public TMP_Text qualityText;

    [Header("Arrays")]
    public Image[] keysTab;
    public Image[] missingKeysTab;
    private bool[] keysCollected;
    public Image[] livesTab;
    private const string keyHighScore1= "HighScoreLevel1";
    private const string keyHighScore2= "HighScoreLevel2";
    private const string keyHighScore3= "HighScoreLevel3";
    private const string keyBestTime1 = "BestTimeLevel1";
    private const string keyBestTime2 = "BestTimeLevel2";
    private const string keyBestTime3 = "BestTimeLevel3";
    private int enemiesKilled=0;
    private int lives = 3;
    private int keysFound = 0;
    private int score = 0;
    private float timer = 0f;
    private void Awake()
    {
        //if (CutsceneCanvas != null)
        //    CutsceneCanvas.enabled = false;
        instance = this;
        //if (PlayerPrefs.GetInt("ActiveLevel") != 0)
        //{
            MissingKeysBox.SetActive(false);
            keysCollected = new bool[keysTab.Length];
            Array.Fill(keysCollected, false);
            for (int i = 0; i < keysTab.Length; i++)
            {
                keysTab[i].color = new Color(keysTab[i].color.r, keysTab[i].color.g, keysTab[i].color.b, 0.25f);
            }
            scoreText.text = score.ToString();
            enemiesText.text = enemiesKilled.ToString();
            qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
            livesTab[lives].enabled = false;//zak³adamy tylko 1 dodatkowe ¿ycie!!!!
        //}
        switch(PlayerPrefs.GetInt("ActiveLevel"))//zapisanie najlepszych wyników i czasów (coœ trzeba zmieniæ)
        {
            case 1:
            {
                if (!PlayerPrefs.HasKey(keyHighScore1))
                    PlayerPrefs.SetInt(keyHighScore1, 0);
                if(!PlayerPrefs.HasKey(keyBestTime1))
                    PlayerPrefs.SetInt(keyHighScore1, 0);
                break;
            }
            case 2:
            {
                if (!PlayerPrefs.HasKey(keyHighScore2))
                    PlayerPrefs.SetInt(keyHighScore2, 0);
                if (!PlayerPrefs.HasKey(keyBestTime2))
                    PlayerPrefs.SetInt(keyHighScore2, 0);
                break;
            }
            case 3:
            {
                if (!PlayerPrefs.HasKey(keyHighScore3))
                    PlayerPrefs.SetInt(keyHighScore3, 0);
                if (!PlayerPrefs.HasKey(keyBestTime3))
                    PlayerPrefs.SetInt(keyHighScore3, 0);
                break;
            }
        }
        InGame();
    }

    public void AddKeys(int keyNo)
    {
        keyNo--;
        if (!keysCollected[keyNo])
        {
            keysCollected[keyNo] = true;
            keysTab[keyNo].color = new Color(keysTab[keyNo].color.r, keysTab[keyNo].color.g, keysTab[keyNo].color.b, 1.0f);
            keysFound++;//tego nie by³o XDDDD
        }
    }

    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    void Start()
    {
        //Debug.Log(PlayerPrefs.GetInt("HighScoreLevel1"));
        //Debug.Log(PlayerPrefs.GetInt("BestTimeLevel1"));
    }

    public void AddLife()
    {
        lives++;
        Debug.Log("Added life no: " + lives);
        livesTab[lives-1].enabled = true;
    }

    public void RemoveLife()
    {
        if (!isImmortal)
        {
            if (lives == 0)
                GameOver();
            else
                lives--;
            livesTab[lives].enabled = false;
        }
    }

    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;

        if(currentGameState==GameState.GS_LEVEL_COMPLETED)
        {
            AddPoints(enemiesKilled * 50);
            score -= Mathf.FloorToInt(timer);
            if(score < 0)
                score = 0;
            int highScore;
            float  bestTime;
            switch(PlayerPrefs.GetInt("ActiveLevel"))
            {
                case 1:
                {
                    highScore = PlayerPrefs.GetInt(keyHighScore1);
                    bestTime = PlayerPrefs.GetFloat("BestTimeLevel1");
                    if (highScore < score)
                    {
                        highScore = score;
                        PlayerPrefs.SetInt("HighScoreLevel1", score);
                    }
                    if (timer < bestTime || bestTime == 0)
                    {
                        PlayerPrefs.SetFloat("BestTimeLevel1", timer);
                    }
                    highScoreText.text = "High score: " + highScore;
                    break;
                }
                case 2:
                {
                    highScore = PlayerPrefs.GetInt(keyHighScore2);
                    bestTime = PlayerPrefs.GetFloat("BestTimeLevel2");
                    if (highScore < score)
                    {
                        highScore = score;
                        PlayerPrefs.SetInt("HighScoreLevel2", score);
                    }
                    if (timer < bestTime || bestTime == 0)
                    {
                        PlayerPrefs.SetFloat("BestTimeLevel2", timer);
                    }
                    highScoreText.text = "High score: " + highScore;
                    break;
                }
                case 3:
                {
                    highScore = PlayerPrefs.GetInt(keyHighScore3);
                    bestTime = PlayerPrefs.GetFloat("BestTimeLevel3");
                    if (highScore < score)
                    {
                        highScore = score;
                        PlayerPrefs.SetInt("HighScoreLevel3", score);
                    }
                    if (timer < bestTime || bestTime == 0)
                    {
                        PlayerPrefs.SetFloat("BestTimeLevel3", timer);
                    }
                    highScoreText.text = "High score: " + highScore;
                    break;
                }
            }
            finalScoreText.text = "Your score: " + score;
        }

        PauseMenuCanvas.enabled = (currentGameState == GameState.GS_PAUSE_MENU);

        inGameCanvas.enabled = (currentGameState == GameState.GS_GAME);

        levelCompletedCanvas.enabled = (currentGameState == GameState.GS_LEVEL_COMPLETED);

        optionsCanvas.enabled = (currentGameState == GameState.GS_OPTIONS);

        GameOverCanvas.enabled = (currentGameState == GameState.GS_GAME_OVER);

    }
    
    public void IncreaseEnemiesKilled()
    {
        enemiesKilled++;
        enemiesText.text = enemiesKilled.ToString();
    }

    void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSE_MENU);
        Time.timeScale = 0.0f;
    }

    void InGame()
    {
        SetGameState(GameState.GS_GAME);
        Time.timeScale = 1.0f;
    }

    void Options()
    {
        SetGameState(GameState.GS_OPTIONS);
        Time.timeScale = 0.0f;
    }

    public void IncreaseQuality()
    {
        QualitySettings.IncreaseLevel();
        qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void DecreaseQuality()
    {
        QualitySettings.DecreaseLevel();
        qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void SetVolume(Slider slider)
    {
        AudioListener.volume = slider.value;
    }

    void LevelCompleted()
    {
        SetGameState(GameState.GS_LEVEL_COMPLETED);
        if (PlayerPrefs.GetInt("ActiveLevel") >= PlayerPrefs.GetInt("UnlockedLevelID"))//czy przeszliœmy najnowszy poziom
            PlayerPrefs.SetInt("UnlockedLevelID", PlayerPrefs.GetInt("UnlockedLevelID") + 1);
        switch(PlayerPrefs.GetInt("ActiveLevel"))//zapisanie najlepszego czasu
        {
            case 1:
            {
                if(timer<PlayerPrefs.GetFloat(keyBestTime1))
                {
                    PlayerPrefs.SetFloat(keyBestTime1,timer);
                }
                break;
            }
            case 2:
            {
                if (timer < PlayerPrefs.GetFloat(keyBestTime2))
                {
                    PlayerPrefs.SetFloat(keyBestTime2, timer);
                }
                break;
            }
            case 3:
            {
                if (timer < PlayerPrefs.GetFloat(keyBestTime3))
                {
                    PlayerPrefs.SetFloat(keyBestTime3, timer);
                }
                break;
            }
        }
    }

    void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
    }

    public void CheckWin(AudioSource source,AudioClip finishSound)
    {
        if(keysFound==keysTab.Length)
        {
            if (PlayerPrefs.GetInt("ActiveLevel")==0)
            {
                SceneManager.LoadSceneAsync("LevelSelector");
            }
            AddPoints(lives * 100);
            LevelCompleted();
            source.PlayOneShot(finishSound);
        }
        else
        {
            StartCoroutine(DisplayMissingKeys());
            //if (keysTab.Length - keysFound == 1)
            //    Debug.Log("You need 1 more key");
            //else
            //    Debug.Log("You need " + (keysTab.Length - keysFound) + " more keys!");
        }
    }

        private IEnumerator DisplayMissingKeys()
    {
        MissingKeysBox.SetActive(true);
        for(int i=0;i<keysTab.Length;i++) 
        {
            missingKeysTab[i].gameObject.SetActive(!keysCollected[i]);
        }
        yield return new WaitForSeconds(3.0f);
        MissingKeysBox.SetActive(false);
    }

    public void OnResumeButtonClicked()
    {
        InGame();
    }

    public void OnOptionsButtonClicked()
    {
        Options();
    }

    //private IEnumerator PlayCutscene()
    //{
    //    levelCompletedCanvas.enabled = false;
    //    CutsceneCanvas.gameObject.SetActive(true);
    //    //CutsceneCanvas.enabled = true;

    //    yield return new WaitForSeconds(cutsceneTime);
    //}

    public void OnNextLevelButtonClicked()
    {
        //if (CutsceneCanvas != null)
        //    StartCoroutine(PlayCutscene());
        switch (PlayerPrefs.GetInt("ActiveLevel"))
        {
            case 1:
                PlayerPrefs.SetInt("ActiveLevel", 2);
                SceneManager.LoadSceneAsync("Level 2");
                break;
            case 2:
                PlayerPrefs.SetInt("ActiveLevel", 3);
                SceneManager.LoadSceneAsync("Level 3");
                break;
        }
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    public void OnReturnToMainMenuButtonClicked()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync("MainMenu");
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            timer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);
            int milliseconds = (Mathf.FloorToInt(timer * 1000) % 1000)/10;
            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (currentGameState == GameState.GS_PAUSE_MENU)
                InGame();
            else if (currentGameState == GameState.GS_GAME)
                PauseMenu();
        }
    }
}
