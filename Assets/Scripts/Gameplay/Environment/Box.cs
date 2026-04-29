using Gameplay.Abilities.Abilities;
using UnityEngine;

namespace Gameplay.Environment {
    public class Box : MonoBehaviour {
        [SerializeField] public float slowSpeed = 10f;
        private bool _broken = false;
        
        private void OnCollisionEnter2D(Collision2D other) {
            if (_broken || !other.gameObject.TryGetComponent(out Coconut c)) return;
            if (c.IsUsingAbility(typeof(EnlargeAbility))) {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.collider);
            }
            _broken = true;
            Destroy(gameObject);
        }
    }
}