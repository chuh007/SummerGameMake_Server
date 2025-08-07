using System;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Manager
{
    public class GameManager : NetworkBehaviour
    {
        [SerializeField] private Image fadeImage;
        [SerializeField] private GameObject playerPrefab;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                SpawnAllPlayers();
            }
        }

        private void SpawnAllPlayers()
        {
            foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            {
                Vector3 spawnPos = client.ClientId == 0 ? Vector3.left * 5 : Vector3.right * 5;
                GameObject player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
                player.GetComponent<NetworkObject>().SpawnAsPlayerObject(client.ClientId);
            }
        }

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
            TeleportPlayerClientRpc(0, Vector3.left * 5);
            TeleportPlayerClientRpc(1, Vector3.right * 5);
        }

        [ClientRpc]
        private void TeleportPlayerClientRpc(ulong clientId, Vector3 newPosition)
        {
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var client))
            {
                NetworkObject playerObj = client.PlayerObject;
                if (playerObj != null)
                {
                    playerObj.transform.position = newPosition;
                }
            }
        }
    }
}