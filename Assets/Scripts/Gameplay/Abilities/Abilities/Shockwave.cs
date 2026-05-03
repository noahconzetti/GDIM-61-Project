using System.Collections;
using UnityEngine;

namespace Gameplay.Abilities.Abilities {
    public class Shockwave : MonoBehaviour {
        private float hitDownTime;
        private Coconut ignore;
        
        public void Init(float hitTime, Coconut ignoreCoconut) {
            hitDownTime = hitTime;
            ignore = ignoreCoconut;
        }
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.TryGetComponent(out Coconut coconut) && coconut != ignore) {
                coconut.StartCoroutine(HitCoconut(coconut));
            }
        }

        private IEnumerator HitCoconut(Coconut coconut) {
            coconut.DeadList.Add(this);
            coconut.Rigidbody.linearVelocity = Vector2.zero;
            yield return new WaitForSeconds(hitDownTime);
            coconut.DeadList.Remove(this);
        }

        public void DestroyObject() {
            Destroy(gameObject);
        }
    }
}