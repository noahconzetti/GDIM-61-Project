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
    [SerializeField] private Image hatImage;

    private Animator _animator;

    private void Awake() {
        TryGetComponent(out _animator);
    }

    private void OnEnable() {
        Coconut.OnPickupAbility += HandlePickup;
        Coconut.OnUseAbilityStart += HandleAbilityStart;
        Coconut.OnUseAbilityEnd += HandleAbilityEnd;
        CustomizationPersistantData.OnPlayersFinalized += SetBackgroundColor;
    }
    
    private void OnDisable() {
        Coconut.OnPickupAbility -= HandlePickup;
        Coconut.OnUseAbilityStart -= HandleAbilityStart;
        Coconut.OnUseAbilityEnd -= HandleAbilityEnd;
        CustomizationPersistantData.OnPlayersFinalized -= SetBackgroundColor;
    }
    
    private void SetBackgroundColor(List<PlayerStartData> playerStartData) {
        foreach (var playerData in playerStartData) {
            if (playerData.PlayerID == player) {
                backgroundColor.color = playerData.PlayerColor;
                hatImage.sprite = playerData.PlayerHat;
            }
        }
    }

    private void HandlePickup(Coconut coconut, AbilityData abilityData) {
        if (coconut.PlayerID != player) return;

        abilityIcon.sprite = abilityData.uiIcon;
        _animator.SetTrigger("Pickup");
    }
    
    private void HandleAbilityStart(Coconut coconut, AbilityData abilityData) {
        if (coconut.PlayerID != player) return;
        
    }
    
    private void HandleAbilityEnd(Coconut coconut, AbilityData abilityData) {
        if (coconut.PlayerID != player) return;
        
        abilityIcon.sprite = null;
    }
}
