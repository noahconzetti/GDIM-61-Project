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
        
            playerInputs.CoconutControls.Enable();
            playerInputs.CoconutControls.P1Jump.performed += ctx => OnPlayerJump?.Invoke(1);
            playerInputs.CoconutControls.P1Ability.performed += ctx => OnPlayerAbility?.Invoke(1);
            playerInputs.CoconutControls.P2Jump.performed += ctx => OnPlayerJump?.Invoke(2);
            playerInputs.CoconutControls.P2Ability.performed += ctx => OnPlayerAbility?.Invoke(2);
        }
    }
}