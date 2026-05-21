using System;
using System.Collections.Generic;
using Gameplay;
using Unity.Cinemachine;
using UnityEngine;

namespace Viewing {
    public class CameraManager : MonoBehaviour {
        [SerializeField] private Camera cam;
        [SerializeField] private Transform playerParent;
        [SerializeField] private float defaultRadius = 7f;
        private CinemachineTargetGroup _group;
        public static event Action<Vector3, float> OnCameraMovement;

        private Vector3 _lastPos;
        private float _lastSize;

        private void Awake() {
            TryGetComponent(out _group);
            _lastPos = cam.transform.position;
            _lastSize = cam.orthographicSize;
        }

        private void OnEnable() {
            GameManager.OnGameStart += AttachPlayers;
        }

        private void OnDisable() {
            GameManager.OnGameStart -= AttachPlayers;
        }

        private void AttachPlayers(RaceInfo raceInfo) {
            _group.Targets = new();
            foreach (Transform child in playerParent) {
                var target = new CinemachineTargetGroup.Target {
                    Object = child,
                    Weight = 1,
                    Radius = defaultRadius
                };
                _group.Targets.Add(target);
            }
        }

        private void LateUpdate() {
            Vector3 newPos = cam.transform.position;
            float newSize = cam.orthographicSize;
            OnCameraMovement?.Invoke(newPos - _lastPos, newSize);
            _lastPos = newPos;
            _lastSize = newSize;
        }
    }
}
