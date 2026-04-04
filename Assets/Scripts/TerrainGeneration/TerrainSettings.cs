using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration {
    [CreateAssetMenu(fileName = "TerrainSettings", menuName = "Terrain Settings")]
    public class TerrainSettings : ScriptableObject {
        [SerializeField] public List<TerrainBlock> blocks;
    }
}