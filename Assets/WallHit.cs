using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class WallHit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //物理
        this.OnCollisionEnter2DAsObservable()
            .Where(c => c.gameObject.tag == "Ball")
            .Subscribe(c => {
                GetComponent<Wall>()
                    .onCollide
                    .OnNext(c);
            });
    }
}
