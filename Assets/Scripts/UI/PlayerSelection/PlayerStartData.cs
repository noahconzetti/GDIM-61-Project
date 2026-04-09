using UnityEngine;

namespace PlayerSelection {
    public class PlayerStartData {
        public readonly int PlayerIndex;
        public Color PlayerColor;
        public Sprite PlayerHat;

        public PlayerStartData(int playerIndex, Color playerColor, Sprite playerHat) {
            PlayerIndex = playerIndex;
            PlayerColor = playerColor;
            PlayerHat = playerHat;
        }
    }
}