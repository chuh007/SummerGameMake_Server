using DG.Tweening;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Manager
{
    public class GameManager : NetworkBehaviour
    {
        [SerializeField] private Image fadeImage;
        
        public void NextGameStart()
        {
            FadeClientRpc();
        }
        
        [ClientRpc]
        private void FadeClientRpc()
        {
            fadeImage.DOFade(1f, 0.5f).OnComplete(() =>
            {
                ResetPlayer();
                fadeImage.DOFade(0f, 0.5f);
            });
        }
        
        private void ResetPlayer()
        {
            TeleportPlayerClientRpc(0,Vector3.right * -5);
            TeleportPlayerClientRpc(1,Vector3.right * 5);
        }
        
        [ClientRpc]
        private void TeleportPlayerClientRpc(ulong clientId, Vector3 newPosition)
        {
            NetworkObject playerObj = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
            if (playerObj == null) return;
            playerObj.transform.position = newPosition;
        }
    }
}