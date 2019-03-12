using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    [SerializeField]
    private float _speed = 1.75f;

    [SerializeField]
    private int _powerupId;

    [SerializeField]
    private AudioClip _audioClip;

    private GameManager _gameManager;

    void Start() {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update() {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -6.0f) {
            Destroy(this.gameObject);
        }

        if(_gameManager.gameOver) {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            Player player = other.GetComponent<Player>();
            if(player != null) {
                AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position, 1.0f);

                if(_powerupId == 0) {
                    player.TripleShotPowerupOn();
                } else if(_powerupId == 1) {
                    player.SpeedUpPowerupOn();
                } else if(_powerupId == 2) {
                    player.EnableShield();
                }
            }
            Destroy(this.gameObject);
        }
    }
}
