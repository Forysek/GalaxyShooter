using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [SerializeField]
    public static GameManager instance = null;

    public bool isCoOpMode = false;
    public bool gameOver = true;

    [SerializeField]
    private GameObject _singlePlayer;

    [SerializeField]
    private GameObject _coOpPlayers;

    private GameObject _players;

    private SpawnManager _spawnManager;

    private UIManager _uiManager;

    private const float _maxY = 3.90f;
    private const float _minY = -3.90f;
    private const float _maxX = 8.80f;
    private const float _minX = -8.80f;

    public float MaxY => _maxY;
    public float MinY => _minY;
    public float MaxX => _maxX;
    public float MinX => _minX;

    void Awake() {

        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        _uiManager = transform.GetChild(0).gameObject.GetComponent<UIManager>();
        _spawnManager = transform.GetChild(1).gameObject.GetComponent<SpawnManager>();
    }

    void Update() {
        if(gameOver) {
            _uiManager.CheckForBestScore();

            if(Input.GetKeyDown(KeyCode.Space)) {
                ResetTheGame();
                if(isCoOpMode) {
                    _players = Instantiate(_coOpPlayers, Vector3.zero, Quaternion.identity);
                } else {
                    Instantiate(_singlePlayer, Vector3.zero, Quaternion.identity);
                }
                gameOver = false;

                _spawnManager.StartSpawning();

                _uiManager.ResetScore();
                _uiManager.GameOverHide();
            } else if(Input.GetKeyDown(KeyCode.Escape)) {
                LoadMainMenu();
            }
        }
    }

    public void StopTheGame() {
        _spawnManager.StopSpawning();

        Destroy(_players);

        _uiManager.GameOverShow();
        _uiManager.ResumeGame();

        gameOver = true;
    }

    public void ResetTheGame() {
        _uiManager.ResetScore();
    }

    public void LoadTheGame(string sceneName, bool isCoop) {
        transform.GetChild(0).gameObject.SetActive(true);
        isCoOpMode = isCoop;
        ResetTheGame();
        SceneManager.LoadScene(sceneName);
        _uiManager.OnGameLoad();
    }

    public void LoadMainMenu() {
        StopTheGame();
        transform.GetChild(0).gameObject.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
}