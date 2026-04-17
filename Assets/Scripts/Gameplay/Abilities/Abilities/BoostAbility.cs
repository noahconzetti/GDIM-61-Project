using System;
using UnityEngine;

namespace Gameplay.Abilities.Abilities {
    [CreateAssetMenu(menuName = "Abilities/Boost")]
    public class BoostAbility : AbilityData {
        [SerializeField] public float boostForce = 4f;
        public override void UseOn(Coconut player, Action endCallback) {
            Debug.Log("Hello");
            player.GetComponent<Rigidbody2D>().linearVelocity += (Vector2.right * boostForce);
            endCallback();
        }
    }
}