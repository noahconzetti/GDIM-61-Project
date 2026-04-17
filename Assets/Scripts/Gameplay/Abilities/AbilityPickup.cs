using UnityEngine;

namespace Gameplay.Abilities {
    public class AbilityPickup : MonoBehaviour {
        [SerializeField] private AbilityStorage abilityData;

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.TryGetComponent(out Coconut player)) return;
            if (player.TryPickupAbility(ChooseRandomAbility())) {
                Destroy(gameObject);
            }
        }

        private AbilityData ChooseRandomAbility() {
            return abilityData.abilityData[Random.Range(0, abilityData.AbilityCount)];
        }
    }
}