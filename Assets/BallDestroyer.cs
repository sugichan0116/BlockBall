using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class BallDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        this.OnTriggerEnter2DAsObservable()
            .Select(collider => collider.GetComponent<Ball>())
            .Where(ball => ball != null)
            .Subscribe(ball => {
                ball.state = Ball.State.IDLE;
            });
    }
}
