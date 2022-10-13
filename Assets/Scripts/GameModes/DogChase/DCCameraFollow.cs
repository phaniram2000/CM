using System;
using UnityEngine;

public class DCCameraFollow : MonoBehaviour
{
     private GameObject _player;
     private Vector3 _offset;

     private bool _canFollow;


     private void OnEnable()
     {
          GameEvents.TapToPlay += OnTapToPlay;
     }

     private void OnDisable()
     {
          GameEvents.TapToPlay -= OnTapToPlay;
     }

     
     private void Start()
     {

          _canFollow = false;
          
          _player = GameObject.FindWithTag("Player");
          if (!_player) return;

          _offset = transform.position - _player.transform.position;
     }


     private void LateUpdate()
     {
          if (!_canFollow) return;

          if (!_player) return;
          
         

          transform.position = new Vector3(transform.position.x, transform.position.y,
              _player.transform.position.z + _offset.z);


     }
     
     private void OnTapToPlay()
     {
          _canFollow = true;
     }
}
