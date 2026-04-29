using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using AppCore;
using PlayerSelection.CustomizationOptionButtons;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace PlayerSelection {
    public class CustomizationManager : MonoBehaviour {
        [SerializeField] private CustomizationOptionData data;
        [SerializeField] private GameObject colorButtonPrefab;
        [SerializeField] private GameObject hatButtonPrefab;
        [SerializeField] private CoconutCustomizationViewPanel[] coconutTabs;

        public const int OPTION_COLOR = 1;
        public const int OPTION_HAT = 2;

        public readonly List<PlayerStartData> Players = new(2);

        public static CustomizationManager Instance = null;

        private HashSet<int> _activeColorIndexes = new();

        public static event Action<PlayerStartData, HashSet<int>> OnOptionsUpdated;
        public static event Action<List<PlayerStartData>> OnPlayersFinalized;
        
        private void OnEnable() {
            CustomizationOptionButton.OnOptionSelected += ChangeOption;
        }
        
        private void OnDisable() {
            CustomizationOptionButton.OnOptionSelected -= ChangeOption;
        }

        private void Awake() {
            if (Instance != null) {
                Destroy(Instance.gameObject);
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start() {
            CreateButtons();
            CreatePlayerDefaults();
        }

        private void CreateButtons() {
            for (int playerIndex = 0; playerIndex < coconutTabs.Length; playerIndex++) {
                for (int choiceIndex = 0; choiceIndex < data.colors.Length; choiceIndex++) {
                    GameObject colorButton = Instantiate(colorButtonPrefab, coconutTabs[playerIndex].colorButtonParent);
                    CustomizationOptionButton button = colorButton.GetComponent<CustomizationOptionButton>();
                    button.Init(playerIndex, OPTION_COLOR, choiceIndex);
                    button.content.color = data.colors[choiceIndex];
                }
                for (int choiceIndex = 0; choiceIndex < data.hats.Length; choiceIndex++) {
                    GameObject hatButton = Instantiate(colorButtonPrefab, coconutTabs[playerIndex].hatButtonParent);
                    CustomizationOptionButton button = hatButton.GetComponent<CustomizationOptionButton>();
                    button.Init(playerIndex, OPTION_HAT, choiceIndex);
                    button.content.sprite = data.hats[choiceIndex];
                }
            }
        }
        
        private void CreatePlayerDefaults() {
            for (int i = 0; i < coconutTabs.Length; i++) {
                var currPlayer = new PlayerStartData(i, data.colors[i], i, data.hats[i], i);
                Players.Add(currPlayer);
                _activeColorIndexes.Add(i);
                OnOptionsUpdated?.Invoke(currPlayer, _activeColorIndexes);
            }
        }
        
        private void ChangeOption(int coconutIndex, int optionTypeIndex, int optionIndex) {
            switch (optionTypeIndex) {
                case OPTION_COLOR:
                    _activeColorIndexes.Remove(Players[coconutIndex].PlayerColorID);
                    Players[coconutIndex].PlayerColor = data.colors[optionIndex];
                    Players[coconutIndex].PlayerColorID = optionIndex;
                    _activeColorIndexes.Add(optionIndex);
                    break;
                case OPTION_HAT:
                    Players[coconutIndex].PlayerHat = data.hats[optionIndex];
                    break;
            }
            
            OnOptionsUpdated?.Invoke(Players[coconutIndex], _activeColorIndexes);
        }

        public void FinalizePlayers() {
            OnPlayersFinalized?.Invoke(Players);
        }

        public void LoadGameScene() {
            SceneManager.LoadScene("Game");
        }
    }
}