using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Abilities.Abilities {
    public class Bomb : MonoBehaviour {
        [SerializeField] private float explosionRadius = 2f;
        [SerializeField] private Vector2 explosionHitDir = new(.3f, 5f);
        [SerializeField] private float explosionHitTime = 1f;
        [SerializeField] private LayerMask destroyLayer;
        private Coconut _ignore;
        private bool _exploded = false;
        
        public void Init(Coconut ignore) {
            _ignore = ignore;
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (_exploded) return;
            if (other.gameObject.TryGetComponent(out Coconut hitPlayer) && hitPlayer != _ignore) {
                Explode();
            }
        }

        private void Explode() {
            List<Collider2D> results = new List<Collider2D>();
            Physics2D.OverlapCircle(transform.position, explosionRadius, new ContactFilter2D(), results);

            foreach (Collider2D collision in results) {
                if (collision.TryGetComponent(out Coconut hitPlayer)) {
                    StartCoroutine(HitPlayer(hitPlayer));
                } else {
                    if (collision.gameObject.layer == destroyLayer) { 
                        Destroy(collision.gameObject);
                    }
                }
            }

            _exploded = true;
            GetComponent<Collider2D>().enabled = false;
        }

        private IEnumerator HitPlayer(Coconut player) {
            player.DeadList.Add(this);
            player.GetComponent<Rigidbody2D>().linearVelocity = explosionHitDir;
            yield return new WaitForSeconds(explosionHitTime);
            player.DeadList.Remove(this);
        }
    }
}