using System;
using System.Collections;
using UnityEngine;

namespace Gameplay.Abilities.Abilities {
    [CreateAssetMenu(menuName = "Abilities/Enlarge")]
    public class EnlargeAbility : AbilityData {
        [SerializeField] private float sizeIncreasePercent = 3;
        [SerializeField] private float massIncreasePercent = 6;
        [SerializeField] private float timeIncreaseSize = 7f;
        [SerializeField] private float maxSpeedIncrease = 2f;
        [SerializeField] private float boostForce = 1f;
        [SerializeField] private AnimationCurve enlargeCurve = AnimationCurve.EaseInOut(0, 0, .5f, 1);

        private Vector3 _ogScale;
        private float _ogMass;
        
        private Rigidbody2D _rb;
        private Transform _transform;
        
        public override void UseOn(Coconut player, Action endCallback) {
            _transform = player.transform;
            _ogScale = _transform.localScale;
            _rb = player.GetComponent<Rigidbody2D>();
            _ogMass = _rb.mass;
            
            _rb.mass *= massIncreasePercent;
            player.MaxSpeedIncreaseOverride = maxSpeedIncrease;
            
            _rb.linearVelocity += (Vector2.right * boostForce);
            
            player.StartCoroutine(IncreaseSize(endCallback, player));
        }

        private IEnumerator IncreaseSize(Action endCallback, Coconut player) {
            yield return ChangePlayerScale(player, _ogScale * sizeIncreasePercent, 1);
            yield return new WaitForSeconds(timeIncreaseSize);
            yield return ChangePlayerScale(player, _ogScale, -1);
    
            player.MaxSpeedIncreaseOverride = null;
            _rb.mass = _ogMass;
    
            endCallback();
        }

        private IEnumerator ChangePlayerScale(Coconut player, Vector3 targetScale, int direction) {
            float t = 0;
            float totalTime = enlargeCurve.keys[^1].time;
            Vector3 startScale = direction > 0 
                ? player.transform.localScale 
                : targetScale * sizeIncreasePercent;

            while (t < totalTime) {
                t += Time.deltaTime;
                float curveValue = enlargeCurve.Evaluate(t);
                player.transform.localScale = Vector3.Lerp(startScale, targetScale, curveValue);
                yield return null;
            }

            player.transform.localScale = targetScale;
        }
    }
}