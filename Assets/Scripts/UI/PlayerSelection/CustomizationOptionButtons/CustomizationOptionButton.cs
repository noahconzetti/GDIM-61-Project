using System;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerSelection.CustomizationOptionButtons {
    public class CustomizationOptionButton : MonoBehaviour {
        [SerializeField] public int coconutIndex;
        [SerializeField] public int optionIndex;
        [SerializeField] public int optionTypeIndex;

        [SerializeField] public Image content;
        
        public static event Action<int, int, int> OnOptionSelected; // CoconutIndex, OptionTypeIndex, OptionIndex
        
        public void OnThisSelected() {
            OnOptionSelected?.Invoke(coconutIndex, optionTypeIndex, optionIndex);
        }

        public void Init(int coconut, int optionType, int option) {
            coconutIndex = coconut;
            optionTypeIndex = optionType;
            optionIndex = option;
        }
    }
}