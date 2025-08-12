using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.Networking
{
    public class LoadSceneBtn : NetworkBehaviour
    {
        [SerializeField] private string sceneName;
        
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(HandleBtnClick);
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if(!IsHost) gameObject.SetActive(false);
        }

        private void HandleBtnClick()
        {
            NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}
