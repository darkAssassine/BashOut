using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BashOut.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private TextMeshProUGUI textComponent;

        public void OnGameOver(string winner)
        {
            animator.Play("GameOverUI");
            textComponent.text = $"{winner} won!";
            StartCoroutine(loadMainMenu());
        }

        private IEnumerator loadMainMenu()
        {
            yield return new WaitForSeconds(2.5f);
            SceneManager.LoadScene(0);
        }
    }
}
