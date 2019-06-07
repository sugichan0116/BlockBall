using UnityEngine;
using UniRx;

public class Result : MonoBehaviour
{
    public GameJudge game;
    public GameObject container;

    // Start is called before the first frame update
    void Start()
    {
        Observable
            .EveryUpdate()
            .Where(_ => game.state == GameJudge.State.OVER)
            .Subscribe(_ => {
                container.SetActive(true);
            })
            .AddTo(gameObject);

        Observable
            .EveryUpdate()
            .Where(_ => game.state == GameJudge.State.OVER)
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ => {
                SSSceneManager.Instance.Screen("Menu");
            })
            .AddTo(gameObject);
    }
}
