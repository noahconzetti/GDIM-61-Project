using System;
using System.Collections;
using AppCore;
using Gameplay.Abilities;
using Gameplay.Abilities.Abilities;
using PlayerSelection;
using UnityEngine;

namespace Gameplay {
    public class Coconut : MonoBehaviour {
        [Header("General Physics Settings")]
        [SerializeField] private float minXSpeed = 3f;
        [SerializeField] private float maxSpeed = 10f;
        [SerializeField] private float maxSpeedEnforcementPerSecond = 1f;
        [Header("Gravity Settings")]
        [SerializeField] private float airGravity = 1.5f;
        [SerializeField] private float groundedGravity = 0.9f;
        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 2;
        [SerializeField] private float forwardMomentumPreserved = 0.9f;
        [Tooltip("This is normalized when jump is called")]
        [SerializeField] private Vector2 jumpDirection;
        [SerializeField] private float jumpBufferTime = .3f;
        [Header("Grounded Settings")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCastAddition = .2f;
        [SerializeField] private int numRaycasts = 10;
        [SerializeField] private float minRaycastAngle = 210;
        [SerializeField] private float maxRaycastAngle = 300;

        [Header("Links")]
        [SerializeField] private CoconutCustomizer coconutCustomizer;

        [Header("Ability settings")]
        [SerializeField] private float squishTime = .5f;
        
        private Rigidbody2D _rb;
        private CircleCollider2D _collider;

        private Vector2 _groundNormal = Vector2.up;
        private bool _jumpBufferActive = false;

        public bool UsingAbility { get; private set; }  = false;

        private float raycastAngleDifference;
        
        private Coroutine _jumpCoroutine;

        public float? MaxSpeedIncreaseOverride = null;

        public bool squished = false;
        
        public int PlayerID { get; private set; }

        [Header("Do not edit pls yay")]
        [SerializeField] private bool _grounded = false;

        [field: SerializeField] public AbilityData currentHeldAbility { get; private set; }

        public static event Action<Coconut, AbilityData> OnPickupAbility;
        public static event Action<Coconut, AbilityData> OnUseAbilityStart;
        public static event Action<Coconut, AbilityData> OnUseAbilityEnd;

        public static event Action<Coconut> OnJump;
        public static event Action<Coconut, Collision2D> OnCollision;
        
        public void Init(PlayerStartData playerStartData) {
            PlayerID = playerStartData.PlayerID;
            coconutCustomizer.SetData(playerStartData);
        }

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
            TryGetComponent(out _collider);
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
                float raycastDistance = (_collider.radius) *_collider.transform.lossyScale.magnitude + groundCastAddition;
                RaycastHit2D hit = Physics2D.Raycast(
                    _rb.position,
                    currentDirection,
                    raycastDistance,
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
            
            _rb.gravityScale = _grounded ? groundedGravity : airGravity;
        }
        
        private void ApplySpeedConstraints() {
            if (_rb.linearVelocityX < minXSpeed && !squished) {
                _rb.linearVelocityX = minXSpeed;
            }
            
            float usedMaxSpeed = maxSpeed;
            if (MaxSpeedIncreaseOverride.HasValue) usedMaxSpeed += MaxSpeedIncreaseOverride.Value;
            if (_rb.linearVelocityX > usedMaxSpeed) {
                _rb.linearVelocityX -= maxSpeedEnforcementPerSecond * Time.deltaTime;
            }
        }
        
        private void HandleJumpInput(int id) {
            if (id != PlayerID) return;
            
            if (!_grounded) {
                if (_jumpCoroutine != null) StopCoroutine(_jumpCoroutine);
                _jumpCoroutine = StartCoroutine(SaveJumpInput());
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
            if (id != PlayerID) return;
            if (currentHeldAbility == null) return;
            if (UsingAbility) return;
            UsingAbility = true;
            OnUseAbilityStart?.Invoke(this, currentHeldAbility);
            
            currentHeldAbility.UseOn(this, EndUseAbility);
        }

        private void EndUseAbility() {
            OnUseAbilityEnd?.Invoke(this, currentHeldAbility);
            currentHeldAbility = null;
            UsingAbility = false;
        }

        public bool TryPickupAbility(AbilityData abilityData) {
            if (currentHeldAbility != null) return false;
            
            currentHeldAbility = abilityData;
            OnPickupAbility?.Invoke(this, currentHeldAbility);
            return true;
        }

        private void OnCollisionEnter2D(Collision2D other) {
            OnCollision?.Invoke(this, other);
            Coconut otherCoconut = other.gameObject.GetComponent<Coconut>();
            
            if (other.contacts[0].normal.x > 0 &&
                otherCoconut != null && 
                otherCoconut.PlayerID != PlayerID && 
                otherCoconut.UsingAbility && 
                otherCoconut.currentHeldAbility.GetType() == typeof(EnlargeAbility)) {
                
                otherCoconut.StartCoroutine(Squish(otherCoconut));
            }
        }

        private IEnumerator Squish(Coconut other) {
            if (squished) {
                yield break;
            }
            squished = true;
            _rb.linearVelocityX = 0;
            Physics2D.IgnoreCollision(_collider, other._collider, true);
            
            yield return new WaitForSeconds(squishTime);
            
            Physics2D.IgnoreCollision(_collider, other._collider, false);
            squished = false;
        }
    }
}
