using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AppCore;
using Gameplay.Abilities;
using Gameplay.Abilities.Abilities;
using Gameplay.Environment;
using PlayerSelection;
using UnityEngine;

namespace Gameplay {
    public class Coconut : MonoBehaviour {
        [Header("General Physics Settings")]
        [SerializeField] private float minXSpeed = 3f;
        [SerializeField] private float maxSpeed = 10f;
        [SerializeField] private float maxSpeedEnforcementPerSecond = 1f;
        [SerializeField] private float minSpeedEnforcementPerSecond = 1f;
        [SerializeField] private float firstPlaceMaxSpeedDebuff = 1f;
        [Header("Gravity Settings")]
        [SerializeField] private float airGravity = 1.5f;
        [SerializeField] private float groundedGravity = 0.9f;
        [SerializeField] private float notGroundedBoost = 1f;
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
        [SerializeField] private float inAirAngularVelocityMax = 1f;
        [SerializeField] private float angularVelocityMaxEnforcementPerSecond = 2f;
        [Header("Links")]
        [SerializeField] private CoconutCustomizer coconutCustomizer;

        [Header("Ability settings")]
        [SerializeField] private float squishTime = .5f;

        [Header("Debug")]
        [SerializeField] private GameObject slowIndicator;
        [SerializeField] private GameObject fastIndicator;
        
        public Rigidbody2D Rigidbody { get; private set; }
        public CircleCollider2D Collider { get; private set; }

        private Vector2 _groundNormal = Vector2.up;
        private bool _jumpBufferActive = false;

        public bool UsingAbility { get; private set; }  = false;

        private float raycastAngleDifference;
        
        private Coroutine _jumpCoroutine;

        public float? MaxSpeedIncreaseOverride = null;

        public bool squished = false;
        public bool dead = false;
        public List<Box> slowedEffects = new List<Box>();
        
        public int PlayerID { get; private set; }

        [Header("Do not edit pls yay")]
        [field: SerializeField] public bool Grounded { get; private set; }  = false;
        [field: SerializeField] public int place { get; private set; } = 0;
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
            Rigidbody = GetComponent<Rigidbody2D>();
            Collider = GetComponent<CircleCollider2D>();
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
                float raycastDistance = (Collider.radius) *Collider.transform.lossyScale.magnitude + groundCastAddition;
                RaycastHit2D hit = Physics2D.Raycast(
                    Rigidbody.position,
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
                Grounded = true;
            } else {
                Grounded = false;
            }
            
            if (Grounded && _jumpBufferActive) {
                Jump();
            }
            
            Rigidbody.gravityScale = Grounded ? groundedGravity : airGravity;
        }
        
        private void ApplySpeedConstraints() {
            // Velocity
            if (Rigidbody.linearVelocityX < minXSpeed && !squished && !dead) {
                Rigidbody.linearVelocityX += minSpeedEnforcementPerSecond;
                slowIndicator.SetActive(true);
            } else {
                slowIndicator.SetActive(false);
            }
            if (Rigidbody.linearVelocityX < 0) Rigidbody.linearVelocityX = 0;
            
            float usedMaxSpeed = maxSpeed;
            if (MaxSpeedIncreaseOverride.HasValue) usedMaxSpeed += MaxSpeedIncreaseOverride.Value;
            if (place == 0) usedMaxSpeed -= firstPlaceMaxSpeedDebuff;
            if (Rigidbody.linearVelocityX > usedMaxSpeed) {
                Rigidbody.linearVelocityX -= maxSpeedEnforcementPerSecond * Time.deltaTime;
                fastIndicator.SetActive(true);
            } else {
                fastIndicator.SetActive(false);
            }
            
            
            // Angular velocity
            if (!Grounded && Rigidbody.angularVelocity < inAirAngularVelocityMax) {
                Rigidbody.angularVelocity += angularVelocityMaxEnforcementPerSecond * Time.deltaTime;
            }
        }

        private float GetSlowEffects() {
            return slowedEffects.Sum(x => x.slowSpeed);
        }

        private void HandleJumpInput(int id) {
            if (id != PlayerID) return;
            
            if (!Grounded) {
                if (_jumpCoroutine != null) StopCoroutine(_jumpCoroutine);
                _jumpCoroutine = StartCoroutine(SaveJumpInput());
                return;
            }

            Jump();
        }

        private void Jump() {
            Vector2 baseForce = _groundNormal * jumpForce;
            float forwardDifference = Rigidbody.linearVelocityX - baseForce.x;
            if (forwardDifference < 0) forwardDifference = 0;
            Vector2 adjustedForce = baseForce + new Vector2(forwardDifference * forwardMomentumPreserved, 0);
            Rigidbody.linearVelocity = adjustedForce;
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
                IsUsingAbility(typeof(EnlargeAbility)) &&
                otherCoconut.IsUsingAbility(typeof(EnlargeAbility))) {
                
                otherCoconut.StartCoroutine(Squish(otherCoconut));
            }
        }

        private IEnumerator Squish(Coconut other) {
            if (squished) {
                yield break;
            }
            squished = true;
            Rigidbody.linearVelocityX = 0;
            Physics2D.IgnoreCollision(Collider, other.Collider, true);
            
            yield return new WaitForSeconds(squishTime);
            
            Physics2D.IgnoreCollision(Collider, other.Collider, false);
            squished = false;
        }

        public void SetCurrentPlace(int place) {
            this.place = place;
        }

        public bool IsUsingAbility(Type abilityType) {
            return UsingAbility && currentHeldAbility.GetType() == abilityType;
        }

        public void ActiveRB(bool b) {
            Rigidbody.bodyType = b ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
        }
    }
}
