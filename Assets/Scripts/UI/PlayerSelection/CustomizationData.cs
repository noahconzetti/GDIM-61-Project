using UnityEngine;

namespace PlayerSelection {
    public class CustomizationData {
        public readonly int PlayerIndex;
        public Color PlayerColor;
        public Sprite PlayerHat;

        public CustomizationData(int playerIndex, Color playerColor, Sprite playerHat) {
            PlayerIndex = playerIndex;
            PlayerColor = playerColor;
            PlayerHat = playerHat;
        }
    }
}