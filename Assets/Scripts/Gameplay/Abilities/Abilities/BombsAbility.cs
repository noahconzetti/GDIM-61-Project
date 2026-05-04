using System;
using UnityEngine;

namespace Gameplay.Abilities.Abilities {
    [CreateAssetMenu(menuName = "Abilities/Bombs Ability")]
    public class BombsAbility : AbilityData {
        [SerializeField] private GameObject bombDropperPrefab;
        public override void UseOn(Coconut player, Action endCallback) {
            GameObject dropper = Instantiate(bombDropperPrefab, player.transform);
            dropper.GetComponent<BombDropper>().Init(player);
        }
    }
}