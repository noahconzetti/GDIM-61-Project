using System;
using UnityEngine;

namespace Gameplay.Abilities {
    public class AbilityPickup : MonoBehaviour {
        [SerializeField] private AbilityData abilityData;

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.TryGetComponent(out Coconut player)) return;
            if (player.TryPickupAbility(abilityData)) {
                Destroy(gameObject);
            }

        }
    }
}