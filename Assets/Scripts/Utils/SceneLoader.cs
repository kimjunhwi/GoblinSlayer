using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// 遷移先のシーン名。
    /// </summary>
    [SerializeField]
    string sceneName;

    /// <summary>
    /// シーン読み込みモード。
    /// </summary>
    [SerializeField]
    LoadSceneMode loadSceneMode = LoadSceneMode.Single;

    /// <summary>
    /// シーンの遷移を実行する。
    /// </summary>
    public void Load()
    {
        SceneManager.LoadScene(sceneName, loadSceneMode);
    }
}
