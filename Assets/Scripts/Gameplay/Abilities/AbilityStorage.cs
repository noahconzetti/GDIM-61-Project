using UnityEngine;

namespace Gameplay.Abilities {
    [CreateAssetMenu(menuName = "Ability Storage")]
    public class AbilityStorage : ScriptableObject {
        [SerializeField] public AbilityData[] abilityData;
        
        public int AbilityCount => abilityData.Length;
    }
}