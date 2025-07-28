using System;
using Unity.Netcode;
using UnityEngine;

namespace Code.Ball
{
    public class Ball : NetworkBehaviour
    {
        [SerializeField] private LayerMask whatIsWall;

        private Rigidbody2D _rbCompo;

        private void Awake()
        {
            _rbCompo = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
             Vector2 normal = other.contacts[0].normal;
             Debug.Log(normal);
        }
    }
}