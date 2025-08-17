using UnityEngine;
using UnityEngine.SceneManagement;

public class ToEnd : MonoBehaviour
{
    public float delay = 1f; // �T���v���V�[���ł̑ҋ@�b��
    public string nextSceneName = "End";

    void Start()
    {
        Invoke(nameof(GoToNextScene), delay);
    }

    void GoToNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
