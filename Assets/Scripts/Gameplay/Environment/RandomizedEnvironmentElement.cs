using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Environment {
    public class RandomizedEnvironmentElement : MonoBehaviour {
        [SerializeField] private float appearChance = 1f;

        [SerializeField] private bool randomizeObject = false;
        [SerializeField] private List<GameObject> optionPrefabs;
        [SerializeField] private List<float> weights;

        private void Awake() {
            if (Random.Range(0f, 1f) > appearChance) {
                Destroy(gameObject);
            } else if (randomizeObject) {
                RandomizeObject();
            }
        }

        private void RandomizeObject() {
            GameObject chosen = ExtraRandom.WeightedChoice(optionPrefabs, weights);
            Instantiate(chosen, transform.position, transform.rotation);
        }
    }
}