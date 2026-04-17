using UnityEngine;

namespace PlayerSelection {
    public class PlayerStartData {
        public readonly int PlayerID;
        public Color PlayerColor;
        public int PlayerColorID;
        public Sprite PlayerHat;
        public int PlayerHatID;

        public PlayerStartData(int playerID, Color playerColor, int colorID, Sprite playerHat, int hatID) {
            PlayerID = playerID;
            PlayerColor = playerColor;
            PlayerColorID = colorID;
            PlayerHat = playerHat;
            PlayerHatID = hatID;
        }
    }
}