using System;
using UnityEngine;

namespace Gameplay.Abilities {
    [CreateAssetMenu(menuName = "Ability Data")]
    public class AbilityData : ScriptableObject {
        [SerializeField] public Sprite uiIcon;
        [SerializeField] public Ability ability;

        [SerializeField] public float boostForce = 4f;
        
        public void UseOn(Coconut player) {
            switch (ability) {
                case Ability.Boost:
                    player.GetComponent<Rigidbody2D>().AddForce(Vector2.right * boostForce);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}