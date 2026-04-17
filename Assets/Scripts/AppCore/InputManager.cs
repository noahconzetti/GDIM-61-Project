using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AppCore {
    public class InputManager : MonoBehaviour {
        public static event Action<int> OnPlayerJump;
        public static event Action<int> OnPlayerAbility;
    
        private PlayerInputs playerInputs;

        private void OnEnable() {
            playerInputs = new PlayerInputs();
            playerInputs.Enable();
        
            playerInputs.CoconutControls.P1Jump.performed += P1Jump;
            playerInputs.CoconutControls.P1Ability.performed += P1Ability;
            playerInputs.CoconutControls.P2Jump.performed += P2Jump;
            playerInputs.CoconutControls.P2Ability.performed += P2Ability;
        }
        
        private void OnDisable() {
            playerInputs.CoconutControls.Disable();
            playerInputs.Disable();
            
            playerInputs.CoconutControls.P1Jump.performed -= P1Jump;
            playerInputs.CoconutControls.P1Ability.performed -= P1Ability;
            playerInputs.CoconutControls.P2Jump.performed -= P2Jump;
            playerInputs.CoconutControls.P2Ability.performed -= P2Ability;
        }


        private void P1Jump(InputAction.CallbackContext _) {
            OnPlayerJump?.Invoke(0);
        }
        private void P2Jump(InputAction.CallbackContext _) {
            OnPlayerJump?.Invoke(1);
        }

        private void P1Ability(InputAction.CallbackContext _) {
            OnPlayerAbility?.Invoke(0);
        }
        private void P2Ability(InputAction.CallbackContext _) {
            OnPlayerAbility?.Invoke(1);
        }

    }
}