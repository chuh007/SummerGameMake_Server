using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Player
{
    public class PlayerSpikeCompo : NetworkBehaviour
    {
        [Header("reference data")] 
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private Transform forceTrm;
        
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
            if (IsOwner)
            {
                SpikeServerRpc();
            }
        }

        [ServerRpc]
        private void SpikeServerRpc()
        {
            Collider2D targetCol = Physics2D.OverlapCircle(forceTrm.position, 1f, whatIsTarget);
            if(targetCol == null) return;
            targetCol.GetComponent<Rigidbody2D>().AddForce((targetCol.transform.position - forceTrm.position) * spikePower, ForceMode2D.Impulse);
        }
    }
}