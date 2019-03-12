using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    private Animator _anim;
    private Player _player;

    void Start() {
        _anim = GetComponent<Animator>();
        _player = GetComponent<Player>();
    }

    void Update() {
        if(Input.GetKeyDown(_player.movementLeft)) {
            _anim.SetBool("TurnLeft", true);
        } else if(Input.GetKeyDown(_player.movementRight)) {
            _anim.SetBool("TurnRight", true);
        } else if(Input.GetKeyUp(_player.movementLeft)) {
            _anim.SetBool("TurnLeft", false);
        } else if(Input.GetKeyUp(_player.movementRight)) {
            _anim.SetBool("TurnRight", false);
        }
    }
}
