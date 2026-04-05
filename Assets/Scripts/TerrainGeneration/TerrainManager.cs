using System;
using UnityEngine;

namespace TerrainGeneration {
    public class TerrainManager : MonoBehaviour {
        [SerializeField] private TerrainSettings settings;
        [SerializeField] private float startGenerationAmt = 100f;

        private Vector2 _lastPosition = Vector2.zero;

        private void Awake() {
            GenerateChunks(startGenerationAmt);
        }

        private void GenerateChunks(float width) {
            float generationProgress = 0f;
            while (generationProgress < width) {
                TerrainBlock currentBlock = ChooseRandomBlock();
                GameObject newBlock = Instantiate(currentBlock.gameObject, Vector3.zero, Quaternion.identity);
                TerrainBlock block = newBlock.GetComponent<TerrainBlock>();
                Vector2 leftAttachOffset = block.leftAttach.localPosition;
                newBlock.transform.position = _lastPosition - leftAttachOffset;

                _lastPosition += block.Size;
                generationProgress += block.Width;
            }
        }

        private TerrainBlock ChooseRandomBlock() {
            return ExtraRandom.WeightedChoice(settings.blocks, x => x.weight);
        }
    }
}