using System;
using UnityEngine;

namespace Gameplay.Abilities.Abilities {
    [CreateAssetMenu(menuName = "Abilities/Bombs Ability")]
    public class BombsAbility : AbilityData {
        [SerializeField] private GameObject bombDropperPrefab;
        public override void UseOn(Coconut player, Action endCallback) {
            GameObject dropperObject = Instantiate(bombDropperPrefab, player.transform);
            BombDropper dropper = dropperObject.GetComponent<BombDropper>();
            dropper.Init(player);
            dropper.AllBombsDropped = endCallback;
        }
    }
}