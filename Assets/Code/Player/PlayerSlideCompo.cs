using System;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;

namespace Code.Player
{
    public class PlayerSlideCompo : NetworkBehaviour
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private float slideForce = 5f;
        [SerializeField] private float slideDuration = 0.5f;
        [SerializeField] private float slideDelay = 0.75f;

        private Rigidbody2D _rbCompo;
        private float _lastSlideTime;

        private void Awake()
        {
            _rbCompo = GetComponent<Rigidbody2D>();
        }

        public override void OnNetworkSpawn()
        {
            if (IsOwner == false) return;
            playerInput.OnSlidePressed += HandleSlideKey;
        }
        
        public override void OnNetworkDespawn()
        {
            if (IsOwner == false) return;
            playerInput.OnSlidePressed -= HandleSlideKey;
        }

        private void HandleSlideKey()
        {
            if (IsOwner)
            {
                if (_lastSlideTime + slideDelay <= Time.time)
                {
                    Slide();
                }
            }
        }

        private void Slide()
        {
            float dir = playerInput.MovementKey.x;
            if (Mathf.Approximately(dir, 0)) return;
            _lastSlideTime = Time.time;
            playerInput.BlockInputChange(false);
            _rbCompo.AddForceX(dir * slideForce, ForceMode2D.Impulse);
            transform.rotation = Quaternion.Euler(0, 0, dir * 90);
            DOVirtual.DelayedCall(slideDuration, () =>
            {
                playerInput.BlockInputChange(true);
                transform.rotation = Quaternion.Euler(Vector3.zero);
                _rbCompo.linearVelocity = Vector2.zero;
            });
        }
    }
}