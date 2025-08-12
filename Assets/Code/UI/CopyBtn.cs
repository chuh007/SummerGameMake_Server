using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class CopyBtn : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI copyTargetText;
        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(HandleBtnClick);
        }

        private void HandleBtnClick()
        {
            GUIUtility.systemCopyBuffer = copyTargetText.text;
        }
    }
}