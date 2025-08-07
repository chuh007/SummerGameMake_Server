using System;
using Code.Networking;
using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class WaitPlayerUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI joinCodeText;

        private void Start()
        {
            joinCodeText.text = HostSingleton.Instance.GameManager.JoinCode;
        }
    }
}