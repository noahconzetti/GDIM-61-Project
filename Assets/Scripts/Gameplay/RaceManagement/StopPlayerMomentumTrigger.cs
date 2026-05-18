using System;
using UnityEngine;

namespace Gameplay.RaceManagement {
    public class StopPlayerMomentumTrigger : MonoBehaviour {
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.TryGetComponent(out Coconut coconut)) {
                coconut.DeadList.Add(this);
                coconut.FinishGame();
            }
        }
    }
}