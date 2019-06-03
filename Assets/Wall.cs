using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Wall : MonoBehaviour
{
    public int hp = 10;
    
    public Subject<Unit> onDestroy = new Subject<Unit>();
    public Subject<int> onHit = new Subject<int>();

    // Start is called before the first frame update
    void Start()
    {
        //物理
        this.OnCollisionEnter2DAsObservable()
            .Select(hit => hit.gameObject.tag)
            .Where(tag => tag == "Ball")
            .Subscribe(_ => {
                hp--;
                onHit.OnNext(hp);
                if (hp <= 0)
                {
                    onDestroy.OnNext(Unit.Default);
                    Destroy(gameObject);
                }
            });
        
        onHit
            .Subscribe(_ => {
                FindObjectOfType<Player>()
                    .onCollided
                    .OnNext(1);
            });
    }
    
}
