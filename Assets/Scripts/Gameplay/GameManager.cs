using System;
using System.Collections.Generic;
using Gameplay.Player;
using PlayerSelection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay {
    public class GameManager : MonoBehaviour {
        [SerializeField] private float raceLength;
        [SerializeField] private bool useSeed = false;
        [SerializeField] private int seed = 0;
        
        // [SerializeField] private CustomizationManager customizationManager;
        [SerializeField] private CoconutSpawner coconutSpawner;
        
        public static event Action<RaceInfo> OnGameStart;
        
        private RaceInfo _raceInfo;
        
        public void Start() {
            if (useSeed) ExtraRandom.SetSeed(seed);

            if (CustomizationManager.Instance == null) {
                SceneManager.LoadScene("Player Selection");
                return;
            }
            Debug.Log(CustomizationManager.Instance);
            CustomizationManager.Instance.FinalizePlayers();
            List<Coconut> coconuts = coconutSpawner.SpawnCoconuts(CustomizationManager.Instance.Players);
            _raceInfo = new RaceInfo(coconuts, raceLength, useSeed?seed:null);
            OnGameStart?.Invoke(_raceInfo);
            
            
        }
    }
}