using System;
using System.Collections.Generic;
using PlayerSelection;
using TerrainGeneration;
using UnityEngine;

namespace Gameplay.RaceManagement {
    public class RaceManager : MonoBehaviour {
        [SerializeField] private GameObject gameEnd;
        
        public static event Action<int, float> OnRaceProgressUpdate; // PlayerID, progress 0-1
        // private float _averageProgress = 0f;
        // private List<float> _progress = new();

        private List<Coconut> _coconutTracking = new();
        
        private float _raceLength;

        private void OnEnable() {
            GameManager.OnGameStart += AttachPlayers;
            TerrainManager.OnTerrainGenerationComplete += MoveEnd;
        }

        private void OnDisable() {
            GameManager.OnGameStart -= AttachPlayers;
            TerrainManager.OnTerrainGenerationComplete -= MoveEnd;
        }

        private void MoveEnd(Vector2 end) {
            gameEnd.transform.position = end;
        }

        private void AttachPlayers(RaceInfo raceInfo) {
            _coconutTracking = raceInfo.Players;
            // _progress = new List<float>(raceInfo.Players.Count);
            _raceLength = raceInfo.RaceDistance;
        }

        private void Update() {
            foreach (Coconut coconut in _coconutTracking) {
                OnRaceProgressUpdate?.Invoke(coconut.PlayerID, (coconut.transform.position.x / _raceLength));
            }
        }
    }
}