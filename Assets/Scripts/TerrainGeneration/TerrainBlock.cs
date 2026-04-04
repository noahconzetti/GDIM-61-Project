using UnityEngine;

namespace TerrainGeneration {
    public class TerrainBlock : MonoBehaviour {
        [SerializeField] public Transform leftAttach;
        [SerializeField] public Transform rightAttach;
        [SerializeField] public float weight = 1f;
        
        public float Width => rightAttach.position.x - leftAttach.position.x;
        public Vector2 Size => rightAttach.position - leftAttach.position;
    }
}