using AppCore;
using UnityEngine;

namespace Gameplay {
    public class Coconut : MonoBehaviour {
        [SerializeField] private int playerID;
        [SerializeField] private float jumpForce = 2;
        [Tooltip("This is normalized when jump is called")]
        [SerializeField] private Vector2 jumpDirection;

        private Vector2 _jumpDirectionNormalized;

        private Rigidbody2D _rb;

        private void OnEnable() {
            InputManager.OnPlayerJump += HandleJump;
            InputManager.OnPlayerAbility += HandleAbility;
        }
    
        private void OnDisable() {
            InputManager.OnPlayerJump -= HandleJump;
            InputManager.OnPlayerAbility -= HandleAbility;
        }

        private void Awake() {
            TryGetComponent(out _rb);
            _jumpDirectionNormalized = jumpDirection.normalized;
        }

        private void HandleAbility(int id) {
            if (id != playerID) return;
            Debug.Log("Using ability!");
        }

        private void HandleJump(int id) {
            if (id != playerID) return;
            _rb.AddForce(_jumpDirectionNormalized * jumpForce, ForceMode2D.Impulse);
        }
    }
}
