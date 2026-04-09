using System;
using PlayerSelection;
using UnityEngine;

namespace Gameplay {
    public class GameManager : MonoBehaviour {
        [SerializeField] private CustomizationManager customizationManager;

        [SerializeField] private GameObject coconutPrefab;
        [SerializeField] private Transform coconutStartPoint;
        [SerializeField] private Transform coconutParent;

        public static event Action OnGameStart;

        public void StartGame() {
            SpawnCoconuts();
            OnGameStart?.Invoke();
        }

        private void SpawnCoconuts() {
            foreach (var customizationData in customizationManager.Players) {
                GameObject newPlayer = Instantiate(coconutPrefab, coconutStartPoint.position, coconutStartPoint.rotation, coconutParent);
                newPlayer.GetComponent<Coconut>().Init(customizationData);
            }
        }
    }
}