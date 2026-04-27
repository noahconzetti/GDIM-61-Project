using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerSelection.CustomizationOptionButtons {
    public class CustomizationOptionButton : MonoBehaviour {
        [SerializeField] public int coconutIndex;
        [SerializeField] public int optionIndex;
        [SerializeField] public int optionTypeIndex;

        [SerializeField] public Image content;

        private Button _button;

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
            _button.interactable = !newUsedColorIndexes.Contains(optionIndex);
        }

        private void OnDisable() {
            CustomizationManager.OnOptionsUpdated -= HandleOptionsUpdated;
        }

        public void Init(int coconut, int optionType, int option) {
            coconutIndex = coconut;
            optionTypeIndex = optionType;
            optionIndex = option;
        }
    }
}