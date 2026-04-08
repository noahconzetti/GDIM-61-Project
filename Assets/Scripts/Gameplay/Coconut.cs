using System;
using System.Collections;
using AppCore;
using Gameplay.Abilities;
using UnityEngine;

namespace Gameplay {
    public class Coconut : MonoBehaviour {
        [SerializeField] private int playerID;
        [Header("General Physics Settings")]
        [SerializeField] private float minXSpeed = 3f;
        [SerializeField] private float maxSpeed = 10f;
        [SerializeField] private float maxSpeedEnforcementPerSecond = 1f;
        // [SerializeField] private float maxXSpeed = 1000f;
        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 2;
        [SerializeField] private float forwardMomentumPreserved = 0.9f;
        [Tooltip("This is normalized when jump is called")]
        [SerializeField] private Vector2 jumpDirection;
        [SerializeField] private float jumpBufferTime = .3f;
        [Header("Grounded Settings")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCastDistance = .2f;
        [SerializeField] private int numRaycasts = 10;
        [SerializeField] private float minRaycastAngle = 210;
        [SerializeField] private float maxRaycastAngle = 300;
        
        private Rigidbody2D _rb;

        private bool _grounded = false;
        private Vector2 _groundNormal = Vector2.up;
        private bool _jumpBufferActive = false;
        
        private float raycastAngleDifference;
        
        public int PlayerID => playerID;

        [SerializeField] private AbilityData currentHeldAbility;

        public static event Action<Coconut, AbilityData> OnPickupAbility;
        public static event Action<Coconut, AbilityData> OnUseAbility;
        public static event Action<Coconut> OnJump;

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
            raycastAngleDifference = (maxRaycastAngle - minRaycastAngle) / numRaycasts;
        }

        private void FixedUpdate() {
            UpdateGrounded();
            ApplySpeedConstraints();
        }
        
        private void UpdateGrounded() {
            RaycastHit2D? bestHit = null;
            for (int i = 0; i < numRaycasts; i++) {
                float currentAngle = minRaycastAngle + raycastAngleDifference * i;
                Vector2 currentDirection = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle));
                RaycastHit2D hit = Physics2D.Raycast(
                    _rb.position,
                    currentDirection,
                    groundCastDistance,
                    groundLayer.value);
                if (hit.collider == null) continue;
                if (hit.normal.y < 0) continue;
                if (!bestHit.HasValue || hit.distance < bestHit.Value.distance) {
                    bestHit = hit;
                }
            }

            if (bestHit.HasValue) {
                _groundNormal = bestHit.Value.normal;
                _grounded = true;
            } else {
                _grounded = false;
            }
            
            if (_grounded && _jumpBufferActive) {
                Jump();
            }
        }
        
        private void ApplySpeedConstraints() {
            if (_rb.linearVelocityX < minXSpeed) {
                _rb.linearVelocityX = minXSpeed;
            }
            if (_rb.linearVelocity.magnitude > maxSpeed) {
                float currentMagnitude = _rb.linearVelocity.magnitude;
                float adjustedMagnitude = currentMagnitude - maxSpeedEnforcementPerSecond * Time.deltaTime;
                _rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, adjustedMagnitude);
            }
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
            Vector2 baseForce = _groundNormal * jumpForce;
            float forwardDifference = _rb.linearVelocityX - baseForce.x;
            if (forwardDifference < 0) forwardDifference = 0;
            Vector2 adjustedForce = baseForce + new Vector2(forwardDifference * forwardMomentumPreserved, 0);
            _rb.linearVelocity = adjustedForce;
            _jumpBufferActive = false;
            OnJump?.Invoke(this);
        }

        private IEnumerator SaveJumpInput() {
            _jumpBufferActive = true;
            yield return new WaitForSeconds(jumpBufferTime);
            _jumpBufferActive = false;
        }
        
        private void HandleAbility(int id) {
            if (id != playerID) return;
            if (currentHeldAbility == null) return;
            currentHeldAbility.UseOn(this);
            OnUseAbility?.Invoke(this, currentHeldAbility);
            currentHeldAbility = null;
        }

        public void TryPickupAbility(AbilityData abilityData) {
            if (currentHeldAbility == null) {
                currentHeldAbility = abilityData;
                OnPickupAbility?.Invoke(this, currentHeldAbility);
            }
        }
    }
}
