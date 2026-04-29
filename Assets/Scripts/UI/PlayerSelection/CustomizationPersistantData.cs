using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSelection {
    public class CustomizationPersistantData : MonoBehaviour
    {
        public List<PlayerStartData> Players = null;
        
        public static CustomizationPersistantData Instance = null;

        public static event Action<List<PlayerStartData>> OnPlayersFinalized;
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }

        public static void FinalizePlayers() {
            OnPlayersFinalized?.Invoke(Instance.Players);
        }
    }
}
