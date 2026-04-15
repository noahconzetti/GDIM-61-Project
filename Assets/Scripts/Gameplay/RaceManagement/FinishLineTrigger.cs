using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.RaceManagement {
    public class FinishLineTrigger : MonoBehaviour {
        private List<Coconut> _finishers = new();

        public static event Action<Coconut, int> OnPlayerFinished; //coconut, place
        public static event Action<List<Coconut>> OnStandingsFinalized;

        private int _numFinished = 0;
        private int _numPlayers = 0;

        private void OnEnable() {
            GameManager.OnGameStart += GetStartInfo;
        }

        private void OnDisable() {
            GameManager.OnGameStart -= GetStartInfo;
        }

        private void GetStartInfo(RaceInfo info) {
            _numPlayers = info.Players.Count;
            _numFinished = 0;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.TryGetComponent(out Coconut player)) return;
            
            _finishers.Add(player);
            OnPlayerFinished?.Invoke(player, ++_numFinished);

            if (_numFinished >= _numPlayers) {
                OnStandingsFinalized?.Invoke(_finishers);
            }
        }
    }
}