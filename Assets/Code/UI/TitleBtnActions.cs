using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.UI
{
    public class TitleBtnActions : MonoBehaviour
    {
        public void SceneLoad(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}