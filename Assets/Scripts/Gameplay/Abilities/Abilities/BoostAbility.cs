using System;
using System.Collections;
using AppCore;
using UnityEngine;

namespace Gameplay.Abilities.Abilities {
    [CreateAssetMenu(menuName = "Abilities/Boost")]
    public class BoostAbility : AbilityData {
        [SerializeField] public Vector2 boostForce;
        [SerializeField] public float betweenBoostsTime = .5f;
        [SerializeField] public int numBoosts = 4;
        [SerializeField] public GameObject boostParticlesPrefab;
        [SerializeField] public AudioData boostSFX;
        
        public override void UseOn(Coconut player, Action endCallback) {
            player.StartCoroutine(Boost(player, endCallback));
            
        }

        private IEnumerator Boost(Coconut player, Action endCallback) {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

            for (int i = 0; i < numBoosts; i++) {
                rb.linearVelocity += boostForce;
                Instantiate(boostParticlesPrefab, player.transform.position, Quaternion.identity);
                boostSFX?.Play();
                yield return new WaitForSeconds(betweenBoostsTime);
            }
            
            endCallback();
        }
    }
}