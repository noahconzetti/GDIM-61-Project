using System;
using Gameplay.RaceManagement;
using UnityEngine;

namespace Gameplay.Abilities.Abilities {
    [CreateAssetMenu(menuName = "Abilities/Projectile")]
    public class ProjectileAbility : AbilityData {
        [SerializeField] private GameObject projectilePrefab;
        public override void UseOn(Coconut player, Action endCallback) {
            GameObject projectile = Instantiate(projectilePrefab, player.transform.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().SetTarget(PickRandomPlayer(player));
            endCallback();
        }

        private Coconut PickRandomPlayer(Coconut ignore) {
            Coconut[] coconuts = RaceManager.GetPlayerPlaces();
            
            for (int i = 0; i < coconuts.Length - 1; i++) {
                if (coconuts[i].PlayerID == ignore.PlayerID) {
                    return coconuts[i+1];
                }
            }
            return coconuts[^2];
        }
    }
}