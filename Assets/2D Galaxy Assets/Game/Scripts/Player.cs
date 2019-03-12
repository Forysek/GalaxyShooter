using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public bool isPlayerOne;
    public bool isPlayerTwo;

    public bool canTripleShot = false;
    public bool hasShield = false;
    public int lives = 3;


    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private GameObject _shieldPrefab;

    [SerializeField]
    private GameObject _explosionPrefab;

    private GameManager _gameManager;

    private UIManager _uiManager;

    private SpawnManager _spawnManager;

    private AudioSource _audioSource;

    [SerializeField]
    private GameObject[] _engines;

    private int _hitCount;

    [SerializeField]
    private float _fireRate = 0.25f;

    private float _canFire;

    [SerializeField]
    private float _speed = 3.0f;

    private KeyCode _movementUp;
    private KeyCode _movementDown;
    private KeyCode _movementLeft;
    private KeyCode _movementRight;

    public KeyCode movementUp => _movementUp;
    public KeyCode movementDown => _movementDown;
    public KeyCode movementLeft => _movementLeft;
    public KeyCode movementRight => _movementRight;

    void Start() {
        _uiManager = GameObject.Find("GameManager").GetComponentInChildren<UIManager>();
        _spawnManager = GameObject.Find("GameManager").GetComponentInChildren<SpawnManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if(_uiManager != null) {
            _uiManager.UpdateLives(lives, isPlayerTwo);
        }

        _audioSource = GetComponent<AudioSource>();
        SetUpMovementKeys();
    }

    void Update() {
        if(isPlayerOne) {
            Movement();

            if(Input.GetKeyDown(KeyCode.Space)) {
                Shoot();
            }
        }

        if(isPlayerTwo) {
            Movement();

            if(Input.GetKeyDown(KeyCode.Keypad0)) {
                Shoot();
            }
        }
    }

    private void Shoot() {
        if(Time.time > _canFire) {
            _audioSource.Play();
            if(canTripleShot) {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                _canFire = Time.time + _fireRate;
            } else {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.88f, 0), Quaternion.identity);
                _canFire = Time.time + _fireRate;
            }
        }
    }

    private void Movement() {
        if(Input.GetKey(_movementUp)) {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }

        if(Input.GetKey(_movementDown)) {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        if(Input.GetKey(_movementLeft)) {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }

        if(Input.GetKey(_movementRight)) {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }

        if(transform.position.y > _gameManager.MaxY) {
            transform.position = new Vector3(transform.position.x, _gameManager.MaxY, 0);
        } else if(transform.position.y < _gameManager.MinY) {
            transform.position = new Vector3(transform.position.x, _gameManager.MinY, 0);
        } else if(transform.position.x > _gameManager.MaxX) {
            transform.position = new Vector3(_gameManager.MinX, transform.position.y, 0);
        } else if(transform.position.x < _gameManager.MinX) {
            transform.position = new Vector3(_gameManager.MaxX, transform.position.y, 0);
        }
    }

    public void TripleShotPowerupOn() {
        canTripleShot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public IEnumerator TripleShotPowerDownRoutine() {
        yield return new WaitForSeconds(4.0f);
        canTripleShot = false;
    }

    public void SpeedUpPowerupOn() {
        _speed *= 1.50f;
        StartCoroutine(SpeedUpPowerDownRoutine());
    }

    public IEnumerator SpeedUpPowerDownRoutine() {
        yield return new WaitForSeconds(4.0f);
        _speed /= 1.50f;
    }

    public void EnableShield() {
        if(!hasShield) {
            hasShield = true;
            _shieldPrefab.SetActive(true);
        }
    }

    public void LifeDown() {
        if(hasShield) {
            hasShield = false;
            _shieldPrefab.SetActive(false);
        } else {
            lives--;
            _uiManager.UpdateLives(lives, isPlayerTwo);
            _hitCount++;

            if(_hitCount == 1) {
                _engines[0].SetActive(true);
            } else if(_hitCount == 2) {
                _engines[1].SetActive(true);
            }

            if(lives < 1) {
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                _gameManager.StopTheGame();

                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "EnemyLaser") {
            Destroy(other.gameObject);
            LifeDown();
        }
    }

    private void SetUpMovementKeys() {
        if(isPlayerOne) {
            _movementUp = KeyCode.W;
            _movementDown = KeyCode.S;
            _movementLeft = KeyCode.A;
            _movementRight = KeyCode.D;
        }

        if(isPlayerTwo) {
            _movementUp = KeyCode.Keypad8;
            _movementDown = KeyCode.Keypad5;
            _movementLeft = KeyCode.Keypad4;
            _movementRight = KeyCode.Keypad6;
        }
    }
}
