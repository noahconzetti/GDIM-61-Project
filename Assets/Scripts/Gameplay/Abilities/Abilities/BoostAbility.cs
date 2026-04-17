using System;
using UnityEngine;

namespace Gameplay.Abilities.Abilities {
    [CreateAssetMenu(menuName = "Abilities/Boost")]
    public class BoostAbility : AbilityData {
        [SerializeField] public float boostForce = 4f;
        public override void UseOn(Coconut player, Action endCallback) {
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.right * boostForce);
            endCallback();
        }
    }
}