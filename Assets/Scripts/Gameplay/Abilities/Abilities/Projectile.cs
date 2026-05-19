using System;
using System.Collections;
using UnityEngine;

namespace Gameplay.Abilities.Abilities {
    public class Projectile : MonoBehaviour {
        [SerializeField] private float projectileForce = 1f;
        [SerializeField] private float coconutDeadTime = 1f;
        [SerializeField] private float coconutDeadTimeVsEnlarged = 0;
        private Coconut _target;
        private Rigidbody2D _rb;
        private Animator _animator;
        private Collider2D _collider;

        private void Awake() {
            TryGetComponent(out _rb);
            TryGetComponent(out _animator);
            TryGetComponent(out _collider);
        }

        public void SetTarget(Coconut target) {
            _target = target;
        }

        private void FixedUpdate() {
            Vector2 dirToTarget = (_target.transform.position - transform.position).normalized;
            _rb.linearVelocity = dirToTarget * projectileForce;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.gameObject.TryGetComponent(out Coconut coconut) || coconut != _target) return;
            
            if (coconut.IsUsingAbility(typeof(EnlargeAbility))) {
                coconutDeadTime = coconutDeadTimeVsEnlarged;
            }
            coconut.StartCoroutine(HitByProjectile(coconut));
            _animator.SetTrigger("Hit");
            _collider.enabled = false;
            _rb.linearVelocity = Vector2.zero;
        }

        private IEnumerator HitByProjectile(Coconut c) {
            Rigidbody2D rb = c.GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
            c.DeadList.Add(this);
            yield return new WaitForSeconds(coconutDeadTime);
            c.DeadList.Remove(this);
        }

        public void HitAnimationDone() {
            Destroy(gameObject);
        }
    }
}