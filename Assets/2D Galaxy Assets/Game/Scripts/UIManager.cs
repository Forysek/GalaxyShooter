using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Sprite[] lives;
    public Image livesImageDisplay, player2LivesImageDisplay;
    public Text scoreText, bestScoreText;
    public int score, bestScore;
    public Text gameOverText;

    [SerializeField]
    private GameObject _pauseMenuPanel;

    private GameManager _gameManager;

    private Animator _pauseMenuAnimator;

    void Start() {
        _pauseMenuAnimator = _pauseMenuPanel.GetComponent<Animator>();
        _pauseMenuAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        _gameManager = GetComponentInParent<GameManager>();
    }

    void Update() {
        PauseGame();
    }

    public void OnGameLoad() {
        if(_gameManager.isCoOpMode) {
            bestScore = PlayerPrefs.GetInt("HighScore", 0);
            bestScoreText.text = "Best: " + bestScore;

            player2LivesImageDisplay.enabled = true;
            player2LivesImageDisplay.color = new Color(0.99f, 0.56f, 0, 1);
            livesImageDisplay.color = new Color(0, 0.78f, 0.99f, 1);
            UpdateLives(0, false);
            UpdateLives(0, true);
        } else {
            bestScore = PlayerPrefs.GetInt("CoOpHighScore", 0);
            bestScoreText.text = "Best: " + bestScore;

            player2LivesImageDisplay.enabled = false;
            livesImageDisplay.color = Color.white;
        }
    }

    public void UpdateLives(int currentLives, bool isPlayerTwo) {
        if(_gameManager.isCoOpMode && isPlayerTwo) {
            player2LivesImageDisplay.sprite = lives[currentLives];
        } else {
            livesImageDisplay.sprite = lives[currentLives];
        }
    }

    public void UpdateScore(int points) {
        score += points;
        scoreText.text = "Score: " + score;
    }

    public void CheckForBestScore() {
        if(score > bestScore) {
            bestScore = score;
            if(_gameManager.isCoOpMode) {
                PlayerPrefs.SetInt("HighScore", bestScore);
                bestScoreText.text = "Best: " + bestScore;
            } else {
                PlayerPrefs.SetInt("CoOpHighScore", bestScore);
                bestScoreText.text = "Best: " + bestScore;
            }
        }
    }

    public void ResetScore() {
        score = 0;
        scoreText.text = "Score: 0";
    }

    public void PauseGame() {
        if(Input.GetKeyDown(KeyCode.P)) {
            _pauseMenuPanel.SetActive(true);
            _pauseMenuAnimator.SetBool("isPaused", true);
            Time.timeScale = 0;
        }
    }

    public void ResumeGame() {
        _pauseMenuPanel.SetActive(false);
        _pauseMenuAnimator.SetBool("isPaused", false);
        Time.timeScale = 1;
    }

    public void GameOverShow() {
        gameOverText.enabled = true;
    }

    public void GameOverHide() {
        gameOverText.enabled = false;
    }
}
