using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerSelection {
    public class CoconutImageColorMatch : MonoBehaviour {
        [SerializeField] private float saturation = .3f;
        [SerializeField] private CoconutCustomizationViewPanel viewPanel;

        private Image _img;

        private void Awake() {
            TryGetComponent(out _img);
        }

        private void OnEnable() {
            CustomizationManager.OnOptionsUpdated += HandleOptionsUpdated;
        }
        private void OnDisable() {
            CustomizationManager.OnOptionsUpdated -= HandleOptionsUpdated;
        }

        private void HandleOptionsUpdated(PlayerStartData playerStartData, HashSet<int> _) {
            if (playerStartData.PlayerID != viewPanel.playerIndex) return;
            
            Color.RGBToHSV(playerStartData.PlayerColor, out float h, out float s, out float v);
            _img.color = Color.HSVToRGB(h, s * saturation, v);
        }
    }
}