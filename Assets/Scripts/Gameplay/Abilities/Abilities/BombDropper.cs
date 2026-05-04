using System.Collections;
using UnityEngine;

namespace Gameplay.Abilities.Abilities {
    public class BombDropper : MonoBehaviour {
        [SerializeField] private float dropFrequency = .5f;
        [SerializeField] private float dropNumber = 15f;
        [SerializeField] private float dropForce = 2f;
        [SerializeField] private GameObject bombPrefab;
        
        private Coconut ignore;
        
        public void Init(Coconut player) {
            ignore = player;
            StartCoroutine(SpawnBombs());
        }

        private IEnumerator SpawnBombs() {
            for (int i = 0; i < dropNumber; i++) {
                GameObject bomb = Instantiate(bombPrefab, transform.position, transform.rotation);
                bomb.GetComponent<Bomb>().Init(ignore);
                bomb.GetComponent<Rigidbody2D>().linearVelocityY = -dropForce;
                yield return new WaitForSeconds(dropFrequency);
            }
        }
    }
}