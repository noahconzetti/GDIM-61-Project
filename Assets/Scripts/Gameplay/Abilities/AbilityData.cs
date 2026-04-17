using System;
using UnityEngine;

namespace Gameplay.Abilities {
    public abstract class AbilityData : ScriptableObject {
        [SerializeField] public Sprite uiIcon;

        public abstract void UseOn(Coconut player, Action endCallback);
    }
}