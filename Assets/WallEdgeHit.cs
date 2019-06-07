using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class WallEdgeHit : MonoBehaviour
{
    public bool Top = true, Side = true, Bottom = true;

    // Start is called before the first frame update
    void Start()
    {
        //物理
        this.OnCollisionEnter2DAsObservable()
            .Where(c => c.gameObject.tag == "Ball")
            .Where(c => c.contacts[0].normal.y < .5)
            .Subscribe(c => {
                
                GetComponent<Wall>()
                    .onCollide
                    .OnNext(c);
            });
    }
}
