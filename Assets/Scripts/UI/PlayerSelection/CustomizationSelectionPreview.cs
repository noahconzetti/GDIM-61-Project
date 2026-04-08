using System;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerSelection {
    public class CustomizationSelectionPreview : MonoBehaviour {
        [SerializeField] public int playerIndex = 0;
        [SerializeField] private Image coconutBase;
        [SerializeField] private Image coconutHat;

        private void OnEnable() {
            CustomizationManager.OnOptionsUpdated += HandleOptionsUpdated;
        }

        private void OnDisable() {
            CustomizationManager.OnOptionsUpdated -= HandleOptionsUpdated;
        }

        private void HandleOptionsUpdated(CustomizationData newData) {
            if (newData.PlayerIndex != playerIndex) return;
            coconutBase.color = newData.PlayerColor;
            coconutHat.sprite = newData.PlayerHat;
        }
    }
}