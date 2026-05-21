using System.Collections;
using Gameplay.Abilities.Abilities;
using UnityEngine;

namespace Gameplay.Environment {
    public class Box : MonoBehaviour {
        [SerializeField] private float deadTime = 2f;
        [SerializeField] private float speedPercent = .3f;
        private bool _broken = false;
        
        private void OnCollisionEnter2D(Collision2D other) {
            if (_broken || !other.gameObject.TryGetComponent(out Coconut c)) return;
            if (c.IsUsingAbility(typeof(EnlargeAbility))) {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.collider);
            } else {
                c.StartCoroutine(PlayerDie(c));
            }

            _broken = true;
            Destroy(gameObject);
        }

        private IEnumerator PlayerDie(Coconut c) {
            c.DeadList.Add(this);
            c.Rigidbody.linearVelocity *= speedPercent;
            yield return new WaitForSeconds(deadTime);
            c.DeadList.Remove(this);
        }
    }
}