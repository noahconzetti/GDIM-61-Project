using UnityEngine;

namespace Gameplay.Environment {
    public class SpeedModifierArea : MonoBehaviour
    {
        [SerializeField] private float speedModifier = 1f;

        private void Start() {
            Destroy(gameObject);
        }
    }
}
