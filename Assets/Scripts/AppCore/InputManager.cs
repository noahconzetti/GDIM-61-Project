using System;
using UnityEngine;

namespace AppCore {
    public class InputManager : MonoBehaviour {
        public static event Action<int> OnPlayerJump;
        public static event Action<int> OnPlayerAbility;
    
        private PlayerInputs playerInputs;

        private void OnEnable() {
            playerInputs = new PlayerInputs();
            playerInputs.Enable();
        
            playerInputs.CoconutControls.P1Jump.performed += _ => OnPlayerJump?.Invoke(1);
            playerInputs.CoconutControls.P1Ability.performed += _ => OnPlayerAbility?.Invoke(1);
            playerInputs.CoconutControls.P2Jump.performed += _ => OnPlayerJump?.Invoke(2);
            playerInputs.CoconutControls.P2Ability.performed += _ => OnPlayerAbility?.Invoke(2);
        }

        private void OnDisable() {
            playerInputs.CoconutControls.Disable();
            playerInputs.Disable();
        }
    }
}