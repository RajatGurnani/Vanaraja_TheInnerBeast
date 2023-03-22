using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public AsyncOperation sceneLoadOperation;

    public float fadeDuration = 1f;
    public CanvasGroup canvasGroup;
    public CanvasGroup nerdyPanel;
    public CanvasGroup vanarajaPanel;

    private void Start()
    {
        sceneLoadOperation = SceneManager.LoadSceneAsync(Scenes.Game);
        sceneLoadOperation.allowSceneActivation = false;
        //StartSplash();
    }

    public void StartSplash()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(nerdyPanel.DOFade(1f, fadeDuration));
        sequence.Append(nerdyPanel.DOFade(0f, fadeDuration));
        sequence.Append(vanarajaPanel.DOFade(1f, fadeDuration));
        sequence.Append(vanarajaPanel.DOFade(0f, fadeDuration));
        sequence.Play().OnComplete(() => sceneLoadOperation.allowSceneActivation = true);
    }
}
