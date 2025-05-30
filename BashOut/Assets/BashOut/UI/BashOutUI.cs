using UnityEngine;
using UnityEngine.SceneManagement;

namespace BashOut.UI
{
    public class BashOutUI : MonoBehaviour
    {
        public void LoadScene(int _i)
        {
            SceneManager.LoadScene(_i);
        }

        public void LoadScene(string _name)
        {
            SceneManager.LoadScene(_name);
        }

        public void DisableGameObject(GameObject _object)
        {
            _object.SetActive(false);
        }

        public void EnableGameObject(GameObject _object)
        {
            _object.SetActive(true);
        }

        public void EndApplication()
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}
