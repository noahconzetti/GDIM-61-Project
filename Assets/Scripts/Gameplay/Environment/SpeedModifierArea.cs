using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Environment {
    public class SpeedModifierArea : MonoBehaviour
    {
        [SerializeField] private AnimationCurve speedChangeByCurrentSpeed;
        [SerializeField] private bool slow = false;
        [SerializeField] private LayerMask playerMask;

        private Collider2D _coll;
        private ContactFilter2D _filter;

        private void Awake() {
            TryGetComponent(out _coll);
            _filter = new ContactFilter2D {
                layerMask = playerMask
            };
        }

        private void FixedUpdate() {
            Collider2D[] colliders = new Collider2D[4];
            _coll.Overlap(_filter, colliders);

            foreach (Collider2D coll in colliders) {
                if (!coll || !coll.TryGetComponent(out Coconut coconut)) continue;
                
                float currentSpeed = coconut.Rigidbody.linearVelocityX;
                float changeSpeedMagnitude = speedChangeByCurrentSpeed.Evaluate(currentSpeed);
                float appliedChange = slow ? -changeSpeedMagnitude : changeSpeedMagnitude;
                
                coconut.Rigidbody.linearVelocityX += appliedChange;
            }
        }
    }
}
