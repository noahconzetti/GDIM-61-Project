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
        public static event Action<Vector3> OnCameraMovement;

        private Vector3 _lastPos;

        private void Awake() {
            TryGetComponent(out _group);
            _lastPos = cam.transform.position;
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
            OnCameraMovement?.Invoke(newPos - _lastPos);
            _lastPos = newPos;
        }
    }
}
