using System;
using System.Collections.Generic;
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
            FinishLineTrigger.OnStandingsFinalized += SetCameraPriority;
        }

        private void OnDisable() {
            FinishLineTrigger.OnStandingsFinalized -= SetCameraPriority;
        }

        private void SetCameraPriority(List<Coconut> obj) {
            _cam.Priority = priority;
        }
    }
}