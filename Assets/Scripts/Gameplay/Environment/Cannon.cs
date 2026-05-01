using System;
using System.Collections;
using UnityEngine;

namespace Gameplay.Environment {
    public class Cannon : MonoBehaviour {
        [SerializeField] private float waitTime = 0.5f;
        [SerializeField] private float shootForce = 10f;
        private Coconut _coconut = null;
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (_coconut != null) return;
            if (other.TryGetComponent(out Coconut coconut)) {
                _coconut = coconut;
                StartCoroutine(ShootPlayer());
            }
        }

        private IEnumerator ShootPlayer() {
            _coconut.transform.position = transform.position;
            Rigidbody2D rb = _coconut.Rigidbody;
            Collider2D collider = _coconut.Collider;
            
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
            collider.enabled = false;
            
            yield return new WaitForSeconds(waitTime);
            
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = (transform.up * shootForce);
            collider.enabled = true;
        }
    }
}