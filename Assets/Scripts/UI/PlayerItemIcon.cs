using System;
using Gameplay;
using Gameplay.Abilities;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemIcon : MonoBehaviour {
    [SerializeField] private int player = 0;
    [SerializeField] private Image abilityIcon;
    
    private void OnEnable() {
        Coconut.OnPickupAbility += HandlePickup;
        Coconut.OnUseAbility += HandleUse;
    }
    
    private void OnDisable() {
        Coconut.OnPickupAbility += HandlePickup;
        Coconut.OnUseAbility += HandleUse;
    }
    
    private void HandlePickup(Coconut coconut, AbilityData abilityData) {
        if (coconut.PlayerID != player) return;

        abilityIcon.sprite = abilityData.uiIcon;
    }
    
    private void HandleUse(Coconut coconut, AbilityData abilityData) {
        if (coconut.PlayerID != player) return;
        
        abilityIcon.sprite = null;
    }
}
