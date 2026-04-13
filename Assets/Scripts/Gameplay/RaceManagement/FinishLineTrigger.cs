using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.RaceManagement {
    public class FinishLineTrigger : MonoBehaviour {
        private List<Coconut> _finishers = new();

        public static event Action<Coconut, int> OnPlayerFinished; //coconut, place

        private int _numFinished = 0;
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.TryGetComponent(out Coconut player)) return;
            
            _finishers.Add(player);
            OnPlayerFinished?.Invoke(player, ++_numFinished);
        }
    }
}