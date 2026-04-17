using System;
using UnityEngine;

namespace Gameplay.Abilities.Abilities {
    [CreateAssetMenu(menuName = "Abilities/Boost")]
    public class BoostAbility : AbilityData {
        [SerializeField] public float boostForce = 4f;
        [SerializeField] public GameObject boostParticlesPrefab;
        
        public override void UseOn(Coconut player, Action endCallback) {
            player.GetComponent<Rigidbody2D>().linearVelocity += (Vector2.right * boostForce);
            Instantiate(boostParticlesPrefab, player.transform.position, player.transform.rotation);
            
            endCallback();
        }
    }
}