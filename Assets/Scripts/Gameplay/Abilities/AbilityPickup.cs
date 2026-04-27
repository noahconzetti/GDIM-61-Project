using UnityEngine;

namespace Gameplay.Abilities {
    public class AbilityPickup : MonoBehaviour {
        [SerializeField] private AbilityStorage abilityData;

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.TryGetComponent(out Coconut player)) return;
            player.TryPickupAbility(ChooseRandomAbility());
        }

        private AbilityData ChooseRandomAbility() {
            return abilityData.abilityData[Random.Range(0, abilityData.AbilityCount)];
        }
    }
}