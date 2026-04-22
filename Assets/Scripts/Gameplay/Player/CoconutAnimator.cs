using System;
using Gameplay.Abilities.Abilities;
using UnityEngine;

namespace Gameplay.Player {
    public class CoconutAnimator : MonoBehaviour {
        private Animator _animator;
        private Rigidbody2D _rb;
        private Coconut _coconut;
        
        private void Awake() {
            TryGetComponent(out _animator);
            TryGetComponent(out _rb);
            TryGetComponent(out _coconut);
        }

        private void Update() {
            _animator.SetFloat("ySpeed", _rb.linearVelocityY);
            _animator.SetFloat("xSpeed", _rb.linearVelocityX);
            _animator.SetBool("Enlarged", 
                _coconut.currentHeldAbility != null &&
                _coconut.currentHeldAbility.GetType() == typeof(EnlargeAbility) &&
                _coconut.UsingAbility);
            _animator.SetBool("Grounded", _coconut.Grounded);
        }
    }
}