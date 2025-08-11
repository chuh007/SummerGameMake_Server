using System;
using Unity.Netcode;
using UnityEngine;

namespace Code.Ball
{
    public class Ball : NetworkBehaviour
    {
        public event Action<float> EnterGroundEvent;
        
        [SerializeField] private LayerMask whatIsWall;
        [SerializeField] private LayerMask whatIsGround;

        private Rigidbody2D _rbCompo;
        private Vector2 _velocity;
        private bool _isHitGround = false;

        private void Awake()
        {
            _rbCompo = GetComponent<Rigidbody2D>();
        }
        
        private void FixedUpdate()
        {
            _velocity = _rbCompo.linearVelocity;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if ((whatIsGround & (1 << other.gameObject.layer)) != 0 && !_isHitGround)
            {
                EnterGroundEvent?.Invoke(transform.position.x);
                _isHitGround = true;
            }
            if ((whatIsWall & (1 << other.gameObject.layer)) == 0) return;
            Vector2 normal = other.contacts[0].normal;
            Vector2 reflectedVelocity = Vector2.Reflect(_velocity, normal);
            _rbCompo.linearVelocity = reflectedVelocity;
        }

        public void ResetBall(Vector2 pos)
        {
            _rbCompo.position = pos;
            _rbCompo.linearVelocity = Vector2.zero;
            _velocity = Vector2.zero;
            _isHitGround = false;
        }
    }
}