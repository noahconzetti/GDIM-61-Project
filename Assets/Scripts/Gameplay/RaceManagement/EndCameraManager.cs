using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Gameplay.RaceManagement {
    public class EndCameraManager : MonoBehaviour {
        [SerializeField] private int priority;
        
        private CinemachineCamera _cam;

        private void Awake() {
            TryGetComponent(out _cam);
        }

        private void OnEnable() {
            FinishLineTrigger.OnPlayerFinished += SetCameraPriority;
        }

        private void OnDisable() {
            FinishLineTrigger.OnPlayerFinished -= SetCameraPriority;
        }

        private void SetCameraPriority(Coconut coconut, int place) {
            _cam.Priority = priority;
        }
    }
}