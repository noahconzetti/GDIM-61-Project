using System;
using System.Collections.Generic;
using PlayerSelection;
using UnityEngine;

namespace Gameplay.RaceManagement {
    public class CoconutProgressIconsManager : MonoBehaviour {
        [SerializeField] private GameObject progressIconPrefab;
        
        private void OnEnable() {
            CustomizationManager.OnPlayersFinalized += CreateProgressIcons;
        }

        private void OnDisable() {
            CustomizationManager.OnPlayersFinalized -= CreateProgressIcons;
        }

        private void CreateProgressIcons(List<PlayerStartData> players) {
            foreach (var player in players) {
                GameObject progressIcon = Instantiate(progressIconPrefab, transform);
                CoconutProgressIcon icon = progressIcon.GetComponent<CoconutProgressIcon>();
                icon.Init(player);
            }
        }
    }
}