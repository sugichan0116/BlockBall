using UniRx;
using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    public Ball prefab;

    public Subject<int> onGain = new Subject<int>();

    // Start is called before the first frame update
    void Start()
    {
        onGain.Subscribe(num => {
            Instantiate(prefab).transform.parent = transform;
        });
    }
}
