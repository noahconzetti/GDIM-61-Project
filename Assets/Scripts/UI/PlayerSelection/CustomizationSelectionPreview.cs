using System;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerSelection {
    public class CustomizationSelectionPreview : MonoBehaviour {
        [SerializeField] public int playerIndex = 0;
        [SerializeField] private Image coconutBase;
        [SerializeField] private Image coconutHat;

        private Animator _animator;
        
        private void OnEnable() {
            CustomizationManager.OnOptionsUpdated += HandleOptionsUpdated;
        }

        private void OnDisable() {
            CustomizationManager.OnOptionsUpdated -= HandleOptionsUpdated;
        }

        private void Awake() {
            TryGetComponent(out _animator);
        }

        private void HandleOptionsUpdated(PlayerStartData newData) {
            if (newData.PlayerID != playerIndex) return;
            coconutBase.color = newData.PlayerColor;
            if (coconutHat.sprite != newData.PlayerHat) {
                coconutHat.sprite = newData.PlayerHat;
                _animator.SetTrigger("Hat Drop");
            }
        }

        public void Winner() {
            _animator.SetTrigger("Win");
        }

        public void SetData(Coconut standing) {
            CoconutCustomizer coconutCustomizer = standing.GetComponent<CoconutCustomizer>();
            coconutBase.color = coconutCustomizer.data.PlayerColor;
            coconutHat.sprite = coconutCustomizer.data.PlayerHat;
        }
    }
}