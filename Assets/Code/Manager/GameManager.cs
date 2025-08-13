using System;
using System.Collections.Generic;
using Code.Networking;
using Code.UI;
using DG.Tweening;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Code.Manager
{
    public class GameManager : NetworkBehaviour
    {
        public UnityEvent OnGameReset;
        
        [SerializeField] private Image fadeImage;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private TextMeshProUGUI winLoseTxt;
        [SerializeField] private Button goConnectBtn;
        
        private Dictionary<ulong, bool> _restartDict = new Dictionary<ulong, bool>();
        
        public override void OnNetworkSpawn()
        {
            goConnectBtn.gameObject.SetActive(false);
            Debug.Log(IsServer);
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
                if (client.ClientId != 0)
                {
                    player.transform.rotation = Quaternion.Euler(0, 180f, 0);
                }
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

        public void GameEndAndWinnerSet(bool isP1Win)
        {
            GameEndClientRpc(isP1Win);
        }

        [ClientRpc]
        private void GameEndClientRpc(bool isP1Win)
        {
            bool isWin;
            if (NetworkManager.Singleton.IsHost) 
                isWin = isP1Win;
            else
                isWin = !isP1Win;

            winLoseTxt.gameObject.SetActive(true);
            winLoseTxt.text = isWin ? "승리!" : "패배...";
            goConnectBtn.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }

        public void Reset()
        {
            winLoseTxt.gameObject.SetActive(false);
            Time.timeScale = 1f;
            _restartDict.Clear();
            OnGameReset?.Invoke();
        }
        
        public void GoConnectScene()
        {
            NetworkManager.Singleton.Shutdown();
            Time.timeScale = 1f;
            ClientSingleton.Instance.GameManager.ChangeScene(SceneNames.MenuScene);
        }
    }
}