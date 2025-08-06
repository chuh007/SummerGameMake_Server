using Code.Networking;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.UI
{
    public class SceneNames
    {
        public const string GameScene = "GameScene";
        public const string WaitScene = "WaitScene";
        public const string MenuScene = "MenuScene";
        public const string BootstrapScene = "BootstrapScene";
    }
    
    public class RelayConnectUI : MonoBehaviour
    {
        [SerializeField] private Button hostBtn;
        [SerializeField] private Button joinBtn;
        [SerializeField] private TMP_InputField joinCodeInput;
        
        private void Awake()
        {
            if(NetworkManager.Singleton == null) SceneManager.LoadScene(SceneNames.BootstrapScene);
            hostBtn.onClick.AddListener(HandleRealyHostClick);
            joinBtn.onClick.AddListener(HandleJoinClick);
        }
        
        private async void HandleRealyHostClick()
        {
            bool result = await HostSingleton.Instance.GameManager.StartHostAsync();
            if (result)
            {
                HostSingleton.Instance.GameManager.ChangeNetworkScene(SceneNames.GameScene);
            }
            else
            {
                Debug.LogError("Relay 호스트 생성중 오류가 발생했습니다.");
            }
        }
        
        private async void HandleJoinClick()
        {
            string joinCode = joinCodeInput.text;
            if (string.IsNullOrEmpty(joinCode)) return;
            
            bool result = await ClientSingleton.Instance.GameManager.StartClientWithJoinCode(joinCode);
        
            if (result == false)
            {
                Debug.LogError("Join실패");
            }
        }
    }
}