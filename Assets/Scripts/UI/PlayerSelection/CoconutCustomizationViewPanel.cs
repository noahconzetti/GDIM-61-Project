using System;
using UnityEngine;

namespace PlayerSelection {
    public class CoconutCustomizationViewPanel : MonoBehaviour {
        [SerializeField] public int playerIndex;
        [SerializeField] public Transform colorButtonParent;
        [SerializeField] public Transform hatButtonParent;
        [SerializeField] private CustomizationSelectionPreview previewer;

        private void Start() {
            previewer.playerIndex = playerIndex;
        }
    }
}