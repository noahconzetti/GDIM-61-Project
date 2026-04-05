using System;
using UnityEngine;

namespace Gameplay.Abilities {
    public class AbilityPickup : MonoBehaviour {
        [SerializeField] private AbilityData abilityData;

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;
            
            Coconut player = other.gameObject.GetComponent<Coconut>();
            player.TryPickupAbility(abilityData);
            Destroy(gameObject);
        }
    }
}