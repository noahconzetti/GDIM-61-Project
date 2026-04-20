using System;
using System.Collections.Generic;
using PlayerSelection;
using UnityEngine;

namespace Gameplay.RaceManagement {
    public class CoconutProgressIconsManager : MonoBehaviour {
        [SerializeField] private GameObject progressIconPrefab;
        [SerializeField] private float distanceBetweenIcons = 5f;
        
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
                float yPos = player.PlayerID * distanceBetweenIcons;
                progressIcon.transform.localPosition = new Vector3(0, yPos, 0);
                icon.Init(player);
            }
        }
    }
}