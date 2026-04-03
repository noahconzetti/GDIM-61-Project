using System;
using System.Collections;
using AppCore;
using UnityEngine;

namespace Gameplay {
    public class Coconut : MonoBehaviour {
        [SerializeField] private int playerID;
        [SerializeField] private float jumpForce = 2;
        [Tooltip("This is normalized when jump is called")]
        [SerializeField] private Vector2 jumpDirection;
        [SerializeField] private float jumpBufferTime = .3f;
        [Header("Grounded Settings")]
        [SerializeField] private LayerMask groundLayer;
        // [SerializeField] private Vector2 groundCastRelativeOrigin;
        [SerializeField] private float groundCastDistance = .2f;

        private Vector2 _jumpDirectionNormalized;

        private Rigidbody2D _rb;

        private bool _grounded = false;
        private bool _jumpBufferActive = false;

        private void OnEnable() {
            InputManager.OnPlayerJump += HandleJumpInput;
            InputManager.OnPlayerAbility += HandleAbility;
        }
    
        private void OnDisable() {
            InputManager.OnPlayerJump -= HandleJumpInput;
            InputManager.OnPlayerAbility -= HandleAbility;
        }

        private void Awake() {
            TryGetComponent(out _rb);
            _jumpDirectionNormalized = jumpDirection.normalized;
        }

        private void Update() {
            _grounded = Physics2D.Raycast(
                _rb.position,
                Vector2.down,
                groundCastDistance,
                groundLayer.value);
            if (_grounded && _jumpBufferActive) {
                Jump();
            }
        }

        private void HandleAbility(int id) {
            if (id != playerID) return;
            Debug.Log("Using ability!");
        }

        private void HandleJumpInput(int id) {
            if (id != playerID) return;
            StopAllCoroutines();
            if (!_grounded) {
                StartCoroutine(SaveJumpInput());
                return;
            }

            Jump();
        }

        private void Jump() {
            _rb.linearVelocity = _jumpDirectionNormalized * jumpForce;
            _jumpBufferActive = false;
        }

        private IEnumerator SaveJumpInput() {
            _jumpBufferActive = true;
            yield return new WaitForSeconds(jumpBufferTime);
            _jumpBufferActive = false;
        }
    }
}
