using UniRx;
using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    public Ball prefab;

    public Subject<int> onGain = new Subject<int>();

    // Start is called before the first frame update
    void Start()
    {
        Player player = FindObjectOfType<Player>();

        onGain.Subscribe(num => {
            Ball ball = Instantiate(prefab);
            ball.transform.parent = transform;
            ball.strongth = player.strongth;
        });
    }
}
