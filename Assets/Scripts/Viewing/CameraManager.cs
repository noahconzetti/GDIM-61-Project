using System;
using System.Collections.Generic;
using Gameplay;
using Unity.Cinemachine;
using UnityEngine;

namespace Viewing {
    public class CameraManager : MonoBehaviour {
        [SerializeField] private Transform playerParent;
        [SerializeField] private float defaultRadius = 7f;
        private CinemachineTargetGroup _group;

        private void Awake() {
            TryGetComponent(out _group);
        }

        private void OnEnable() {
            GameManager.OnGameStart += AttachPlayers;
        }

        private void OnDisable() {
            GameManager.OnGameStart -= AttachPlayers;
        }

        private void AttachPlayers() {
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
    }
}
