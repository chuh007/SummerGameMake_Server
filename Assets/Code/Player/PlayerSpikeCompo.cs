using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Player
{
    public class PlayerSpikeCompo : NetworkBehaviour
    {
        [Header("reference data")] 
        [SerializeField] private PlayerInputSO playerInput;
        
        [Header("Setting values")] 
        [SerializeField] private float spikePower = 5f;
        [SerializeField] private LayerMask whatIsTarget;
        
        public override void OnNetworkSpawn()
        {
            if (IsOwner == false) return;
            playerInput.OnAttackPressed += HandleSpikeKey;
        }
        
        public override void OnNetworkDespawn()
        {
            if (IsOwner == false) return;
            playerInput.OnAttackPressed -= HandleSpikeKey;
        }

        private void HandleSpikeKey()
        {
            Debug.Log("스파이크");
            Collider2D targetCol = Physics2D.OverlapCircle(transform.position, 1f, whatIsTarget);
            if(targetCol == null) return;
            targetCol.GetComponent<Rigidbody2D>().AddForce(targetCol.transform.position - transform.position * spikePower, ForceMode2D.Impulse);
        }
    }
}