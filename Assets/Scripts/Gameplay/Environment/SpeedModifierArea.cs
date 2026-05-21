using System;
using System.Collections.Generic;
using TerrainGeneration;
using UnityEngine;

namespace Gameplay.Environment {
    public class SpeedModifierArea : MonoBehaviour
    {
        [SerializeField] private AnimationCurve speedChangeByCurrentSpeed;
        [SerializeField] private bool slow = false;
        [SerializeField] private LayerMask playerMask;
        [SerializeField] private TerrainSurfacePatch terrainPatch;
        
        private Collider2D _coll;
        private ContactFilter2D _filter;

        private void Awake() {
            TryGetComponent(out _coll);
            _filter = new ContactFilter2D {
                layerMask = playerMask,
                useLayerMask = true
            };
        }

        private void OnEnable() {
            TerrainManager.OnTerrainGenerationComplete += UpdateTerrainPatch;
        }
        private void OnDisable() {
            TerrainManager.OnTerrainGenerationComplete -= UpdateTerrainPatch;
        }

        private void UpdateTerrainPatch(Vector2 _) {
            terrainPatch.Generate((Vector2)_coll.bounds.min + new Vector2(0, _coll.bounds.max.y), _coll.bounds.max, slow);
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
