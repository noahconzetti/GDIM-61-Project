using System;
using System.Collections;
using UnityEngine;

namespace Gameplay.Abilities.Abilities {
    [CreateAssetMenu(menuName = "Shockwave Ability")]
    public class ShockwaveAbility : AbilityData {
        [SerializeField] private float radius = 20f;
        [SerializeField] private float expandTime = 1f;
        [SerializeField] private float recoverTime = 2f;
        [SerializeField] private GameObject shockwavePrefab;
        
        public override void UseOn(Coconut player, Action endCallback) {
            player.StartCoroutine(Shockwave(player, endCallback));
        }

        private IEnumerator Shockwave(Coconut player, Action endCallback) {
            GameObject shockwave = Instantiate(shockwavePrefab, player.transform.position, Quaternion.identity);
            shockwave.GetComponent<Shockwave>().Init(recoverTime, player);
            yield return new WaitForSeconds(expandTime);
            yield return new WaitForSeconds(recoverTime);
            endCallback();
        }
    }
}