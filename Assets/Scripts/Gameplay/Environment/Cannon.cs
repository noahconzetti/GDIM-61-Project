using System;
using System.Collections;
using System.Collections.Generic;
using AppCore;
using UnityEngine;

namespace Gameplay.Environment {
    public class Cannon : MonoBehaviour {
        [SerializeField] private float waitTime = 0.5f;
        [SerializeField] private float shootForce = 10f;

        [SerializeField] private float rotationAmount = 20f;
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private float coconutCooldownTime = .5f;
        [SerializeField] private AudioData launchAudio;
        
        private Coconut _coconut = null;

        private float _startRotation;

        private Animator _animator;
        private bool _launchAnimDone = false;

        private static float[] _lastLaunchTimes = new float[4];

        private void Awake() {
            TryGetComponent(out _animator);
        }

        private void Start() {
            _startRotation = transform.rotation.eulerAngles.z;
        }

        private void Update() {
            if (!_coconut) {
                RotateCannon();
            }
        }

        private void RotateCannon() {
            float newRotation = _startRotation + Mathf.Sin(Time.time * rotationSpeed) * rotationAmount;
            transform.rotation = Quaternion.Euler(0, 0, newRotation);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (_coconut != null) return;
            if (other.TryGetComponent(out Coconut coconut) && 
                Time.time - _lastLaunchTimes[coconut.PlayerID] > coconutCooldownTime) {
                _coconut = coconut;
                StartCoroutine(ShootPlayer());
            }
        }

        private IEnumerator ShootPlayer() {
            _coconut.transform.position = transform.position;
            Rigidbody2D rb = _coconut.Rigidbody;
            Collider2D collider = _coconut.Collider;
            _animator.SetTrigger("Rumble");
            
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
            collider.enabled = false;
            
            yield return new WaitForSeconds(waitTime);
            
            _animator.SetTrigger("Launch");

            yield return new WaitUntil(() => _launchAnimDone);
            
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = (transform.up * shootForce);
            collider.enabled = true;

            _lastLaunchTimes[_coconut.PlayerID] = Time.time;
            _coconut = null;
            launchAudio.Play();
        }

        public void LaunchAnimDone() {
            _launchAnimDone = true;
        }
    }
}