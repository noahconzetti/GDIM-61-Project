using System.Collections.Generic;
using UnityEngine;

namespace Gameplay {
    public struct RaceInfo {
        public List<Coconut> Players;
        public float RaceDistance;
        public float? Seed;

        public RaceInfo(List<Coconut> players, float raceDistance, float? seed) {
            Players = players;
            RaceDistance = raceDistance;
            Seed = seed;
        }
    }
}