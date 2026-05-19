using System;
using Gameplay.Abilities.Abilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Abilities {
    public class AbilityPickup : MonoBehaviour {
        [SerializeField] private AbilityStorage abilityData;

        private Animator _animator;
        private Collider2D _collider;

        private void Awake() {
            TryGetComponent(out _animator);
            TryGetComponent(out _collider);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.TryGetComponent(out Coconut player)) return;
            if (player.TryPickupAbility(ChooseRandomAbility(player.place))) {
                _animator.SetTrigger("Collect");
                _collider.enabled = false;
            }
        }

        private AbilityData ChooseRandomAbility(int place) {
            while (true) {
                AbilityData ability = abilityData.abilityData[Random.Range(0, abilityData.AbilityCount)];
                if (place == 0 && ability.GetType() == typeof(BoostAbility)) continue;
                return ability;
            }
        }

        public void CollectAnimationCompleted() {
            Destroy(gameObject);
        }
    }
}