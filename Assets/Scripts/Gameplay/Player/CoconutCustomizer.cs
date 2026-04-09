using PlayerSelection;
using UnityEngine;

namespace Gameplay {
    public class CoconutCustomizer : MonoBehaviour{
        [SerializeField] private SpriteRenderer coconutBaseSprite;
        [SerializeField] private SpriteRenderer hatSprite;

        public void SetData(PlayerStartData playerStartData) {
            coconutBaseSprite.color = playerStartData.PlayerColor;
            hatSprite.color = playerStartData.PlayerColor;
        }
    }
}