using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class GameJudge : MonoBehaviour
{
    public enum State
    {
        START,
        PAUSE,
        OVER
    }

    public State state;

    // Start is called before the first frame update
    void Start()
    {
        //物理
        this.OnTriggerEnter2DAsObservable()
            .Select(collider => collider.GetComponent<Wall>())
            .Where(wall => wall != null)
            .Subscribe(_ => {
                state = State.OVER;
            })
            .AddTo(gameObject);
        
    }
}
