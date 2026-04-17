using System;
using System.Collections;
using UnityEngine;

namespace Gameplay.Abilities.Abilities {
    [CreateAssetMenu(menuName = "Abilities/Enlarge")]
    public class EnlargeAbility : AbilityData {
        [SerializeField] private float sizeIncreasePercent = 3;
        [SerializeField] private float massIncreasePercent = 6;
        [SerializeField] private float timeIncreaseSize = 7f;

        private Vector3 _ogSize;
        private float _ogMass;
        
        private Rigidbody2D _rb;
        private Transform _transform;
        
        public override void UseOn(Coconut player, Action endCallback) {
            _transform = player.transform;
            _ogSize = _transform.localScale;
            _rb = player.GetComponent<Rigidbody2D>();
            _ogMass = _rb.mass;
            
            player.transform.localScale *= sizeIncreasePercent;
            _rb.mass *= massIncreasePercent;
            player.StartCoroutine(IncreaseSize(endCallback));
        }

        private IEnumerator IncreaseSize(Action endCallback) {
            yield return new WaitForSeconds(timeIncreaseSize);
            _transform.localScale = _ogSize;
            _rb.mass = _ogMass;
            
            endCallback();
        }
    }
}