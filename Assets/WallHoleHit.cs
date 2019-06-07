using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class WallHoleHit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //物理
        this.OnCollisionEnter2DAsObservable()
            .Where(c => c.gameObject.tag == "Ball")
            .Subscribe(c => {
                Destroy(c.gameObject);
                GetComponent<Wall>()
                    .onCollide
                    .OnNext(c);
            });

        GetComponent<Wall>().onDestroy
            .Subscribe(_ => {
                FindObjectOfType<Player>()
                    .onPowerUp.OnNext(1);
            });
    }
}
