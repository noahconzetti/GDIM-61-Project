using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerSelection.CustomizationOptionButtons {
    public class CustomizationOptionButton : MonoBehaviour {
        [SerializeField] public int coconutIndex;
        [SerializeField] public int optionIndex;
        [SerializeField] public int optionTypeIndex;
        [SerializeField] private float desaturationPercentage = 0.3f;

        [SerializeField] public Image content;

        private Button _button;
        private Color _baseColor;

        public static event Action<int, int, int> OnOptionSelected; // CoconutIndex, OptionTypeIndex, OptionIndex

        private void Awake() {
            TryGetComponent(out _button);
        }

        public void OnThisSelected() {
            OnOptionSelected?.Invoke(coconutIndex, optionTypeIndex, optionIndex);
        }

        private void OnEnable() {
            CustomizationManager.OnOptionsUpdated += HandleOptionsUpdated;
        }

        private void HandleOptionsUpdated(PlayerStartData playerUpdated, HashSet<int> newUsedColorIndexes) {
            if (optionTypeIndex != CustomizationManager.OPTION_COLOR) return;
            bool selectable = !newUsedColorIndexes.Contains(optionIndex);
            _button.interactable = selectable;
            Color.RGBToHSV(_baseColor, out float h, out float s, out float v);
            content.color = selectable ? _baseColor : Color.HSVToRGB(h, s * desaturationPercentage, v);
        }

        private void OnDisable() {
            CustomizationManager.OnOptionsUpdated -= HandleOptionsUpdated;
        }

        public void Init(int coconut, int optionType, int option) {
            coconutIndex = coconut;
            optionTypeIndex = optionType;
            optionIndex = option;
        }

        public void SetColor(Color dataColor) {
            _baseColor = dataColor;
            content.color = _baseColor;
        }
    }
}