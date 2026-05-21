using UnityEngine;

namespace Gameplay.Environment {
    public class Ramp : MonoBehaviour
    {
        [SerializeField] private AnimationCurve speedChangeByCurrentSpeed;
        [SerializeField] private LayerMask playerMask;
        
        private void OnCollisionStay2D(Collision2D collision) {
            if (!collision.collider || !collision.collider.TryGetComponent(out Coconut coconut)) return;
            
            float currentSpeed = coconut.Rigidbody.linearVelocityX;
            float changeSpeedMagnitude = speedChangeByCurrentSpeed.Evaluate(currentSpeed);
            
            coconut.Rigidbody.linearVelocityX += changeSpeedMagnitude;
        }
    }
}