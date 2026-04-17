using System;
using System.Collections.Generic;
using PlayerSelection;
using TerrainGeneration;
using Unity.Cinemachine;
using UnityEngine;

namespace Gameplay.RaceManagement {
    public class RaceManager : MonoBehaviour {
        [SerializeField] private GameObject gameEnd;
        
        public static event Action<int, float> OnRaceProgressUpdate; // PlayerID, progress 0-1
        // private float _averageProgress = 0f;
        // private List<float> _progress = new();

        private List<Coconut> _coconutTracking = new();
        
        private float _raceLength;

        private Coconut[] _places;

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
            
            _places = new Coconut[_coconutTracking.Count];
        }

        private void Update() {
            _coconutTracking.Sort((a, b) => 
                a.transform.position.x > b.transform.position.x ? -1 : 1);
            
            for (int i = 0; i < _coconutTracking.Count; i++) {
                Coconut coconut = _coconutTracking[i];
                coconut.SetCurrentPlace(i);
                OnRaceProgressUpdate?.Invoke(coconut.PlayerID, (coconut.transform.position.x / _raceLength));
            }
        }
    }
}