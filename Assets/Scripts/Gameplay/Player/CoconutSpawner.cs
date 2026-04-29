using System;
using System.Collections.Generic;
using PlayerSelection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Player {
    public class CoconutSpawner : MonoBehaviour {
        [SerializeField] private GameObject coconutPrefab;
        [SerializeField] private Transform coconutStartPoint;
        [SerializeField] private Transform coconutParent;
        
        public List<Coconut> SpawnCoconuts(List<PlayerStartData> players) {
            List<Coconut> coconuts = new List<Coconut>();
            foreach (var customizationData in players) {
                GameObject newPlayer = Instantiate(coconutPrefab, coconutStartPoint.position, coconutStartPoint.rotation, coconutParent);
                Coconut coconut = newPlayer.GetComponent<Coconut>();
                coconut.Init(customizationData);
                coconut.ActiveRB(false);
                newPlayer.transform.position += new Vector3(Random.Range(0, 0.5f), 0, Random.Range(0, 0.5f));
                coconuts.Add(coconut);
            }

            return coconuts;
        }
    }
}