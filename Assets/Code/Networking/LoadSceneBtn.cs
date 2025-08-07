using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.Networking
{
    public class LoadSceneBtn : MonoBehaviour
    {
        [SerializeField] private string sceneName;
        
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(HandleBtnClick);
        }

        private void HandleBtnClick()
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
