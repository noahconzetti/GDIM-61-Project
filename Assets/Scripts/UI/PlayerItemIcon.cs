using System;
using System.Collections.Generic;
using Gameplay;
using Gameplay.Abilities;
using PlayerSelection;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemIcon : MonoBehaviour {
    [SerializeField] private int player = 0;
    [SerializeField] private Image abilityIcon;
    [SerializeField] private Image backgroundColor;
    
    private void OnEnable() {
        Coconut.OnPickupAbility += HandlePickup;
        Coconut.OnUseAbilityEnd += HandleUse;
        CustomizationManager.OnPlayersFinalized += SetBackgroundColor;
    }
    
    private void OnDisable() {
        Coconut.OnPickupAbility -= HandlePickup;
        Coconut.OnUseAbilityEnd -= HandleUse;
        CustomizationManager.OnPlayersFinalized -= SetBackgroundColor;
    }

    private void SetBackgroundColor(List<PlayerStartData> playerStartData) {
        foreach (var playerData in playerStartData) {
            if (playerData.PlayerID == player) {
                backgroundColor.color = playerData.PlayerColor;
            }
        }
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
