using Gameplay.Abilities.Abilities;
using UnityEngine;

namespace Gameplay.Abilities {
    public class AbilityPickup : MonoBehaviour {
        [SerializeField] private AbilityStorage abilityData;

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.TryGetComponent(out Coconut player)) return;
            if (player.TryPickupAbility(ChooseRandomAbility(player.place))) {
                Destroy(gameObject);
            }
        }

        private AbilityData ChooseRandomAbility(int place) {
            while (true) {
                AbilityData ability = abilityData.abilityData[Random.Range(0, abilityData.AbilityCount)];
                if (place == 0 && ability.GetType() == typeof(BoostAbility)) continue;
                return ability;
            }
        }
    }
}