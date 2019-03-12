using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    private GameObject _gameManager;

    void Start() {
        _gameManager = GameObject.Find("GameManager");
        _gameManager.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void LoadSingle() {
        _gameManager.GetComponent<GameManager>().LoadTheGame("Game", false);
    }

    public void LoadCoOp() {
        _gameManager.GetComponent<GameManager>().LoadTheGame("Game", true);
    }

    public void LoadMainMenu() {
        _gameManager.GetComponent<GameManager>().LoadMainMenu();
    }

}
