using System;
using Gameplay;
using UnityEngine;

namespace TerrainGeneration {
    public class TerrainManager : MonoBehaviour {
        [SerializeField] private float extraGenerationDistance = 20f;
        [SerializeField] private TerrainSettings settings;

        private Vector2 _lastPosition = Vector2.zero;
        public static event Action<Vector2> OnTerrainGenerationComplete;

        private void OnEnable() {
            GameManager.OnGameStart += HandleGameStart;
        }

        private void OnDisable() {
            GameManager.OnGameStart -= HandleGameStart;
        }

        private void HandleGameStart(RaceInfo raceInfo) {
            GenerateChunks(raceInfo.RaceDistance + extraGenerationDistance);
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
            OnTerrainGenerationComplete?.Invoke(_lastPosition);
        }

        private TerrainBlock ChooseRandomBlock() {
            return ExtraRandom.WeightedChoice(settings.blocks, x => x.weight);
        }
    }
}