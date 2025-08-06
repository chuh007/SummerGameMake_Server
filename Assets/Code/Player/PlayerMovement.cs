using Unity.Netcode;
using UnityEngine;

namespace Code.Player
{
    public class PlayerMovement : NetworkBehaviour
    {
        [Header("reference data")] 
        [SerializeField] private PlayerInputSO playerInput;
        
        [Header("Setting values")] 
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float jumpPower = 15f;
        [SerializeField] private LayerMask whatIsGround;
        
        private Rigidbody2D _rbCompo;
        private bool _isGround = false;
        
        private void Awake()
        {
            _rbCompo = GetComponent<Rigidbody2D>();
        }
        
        public override void OnNetworkSpawn()
        {
            if (IsOwner == false) return;
            transform.position = OwnerClientId == 0 ? new Vector3(-5, 0, 0) : new Vector3(5, 0, 0);
            playerInput.OnJumpPressed += HandleJumpKey;
        }
        
        public override void OnNetworkDespawn()
        {
            if (IsOwner == false) return;
            playerInput.OnJumpPressed -= HandleJumpKey;
        }
        
        private void FixedUpdate()
        {
            if (IsOwner == false) return;

            CheckGround();
            HandleMovement();
        }
        
        private void HandleMovement()
        {
            _rbCompo.linearVelocityX = playerInput.MovementKey.x * moveSpeed;
        }
        
        private void CheckGround()
        {
            _isGround = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, whatIsGround);
        }
        
        private void HandleJumpKey()
        {
            if(!_isGround) return;
            _rbCompo.linearVelocityY = 0;
            _rbCompo.AddForceY(jumpPower, ForceMode2D.Impulse);
        }
        
    }
}
