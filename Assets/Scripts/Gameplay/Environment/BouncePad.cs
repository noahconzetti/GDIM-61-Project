using AppCore;
using UnityEngine;

namespace Gameplay.Environment {
    public class BouncePad : MonoBehaviour {
        [SerializeField] private Vector2 minBounce = new(2, 5);
        [SerializeField] private float maxYBounce = 15f;
        [SerializeField] private AudioData bounceSound;

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.TryGetComponent(out Coconut coconut)) return;
            if (coconut.Rigidbody.linearVelocityY > 0) return;
            
            Vector2 newVector = coconut.Rigidbody.linearVelocity;
            newVector.y = -newVector.y;
            if (newVector.x < minBounce.x) newVector.x = minBounce.x;
            if (newVector.y < minBounce.y) newVector.y = minBounce.y;
            if (newVector.y > maxYBounce) newVector.y = maxYBounce;
            coconut.Rigidbody.linearVelocity = newVector;
            
            bounceSound.Play();
        }
    }
}
