using UnityEngine;

namespace PlayerSelection {
    public class PlayerStartData {
        public readonly int PlayerID;
        public Color PlayerColor;
        public Sprite PlayerHat;

        public PlayerStartData(int playerID, Color playerColor, Sprite playerHat) {
            PlayerID = playerID;
            PlayerColor = playerColor;
            PlayerHat = playerHat;
        }
    }
}