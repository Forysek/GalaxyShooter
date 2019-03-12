using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _enemyExplosion;

    private GameManager _gameManager;
    private UIManager _uiManager;
    private Player _player;


    [SerializeField]
    private AudioClip _audioClip;

    [SerializeField]
    private float _speed = 1.5f;

    [SerializeField]
    private int _lives = 1;

    void Start() {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("GameManager").GetComponentInChildren<UIManager>();
    }

    void Update() {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        float random = Random.Range(_gameManager.MinX + 1, _gameManager.MaxX - 1);

        if(transform.position.y < _gameManager.MinY - 2) {
            transform.position = new Vector3(random, _gameManager.MaxY + 2, 0);
        }

        if(_gameManager.gameOver) {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            Player player = other.GetComponent<Player>();
            if(player != null) {
                player.LifeDown();
                LifeDown(5);
            }
        }

        if(other.tag == "Laser") {
            Destroy(other.gameObject);
            LifeDown(10);
        }
    }

    public void LifeDown(int points) {
        _lives--;
        if(_lives < 1) {
            Instantiate(_enemyExplosion, transform.position, Quaternion.identity);

            _uiManager.UpdateScore(points);

            AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position, 1.0f);
            Destroy(this.gameObject);
        }
    }
}
