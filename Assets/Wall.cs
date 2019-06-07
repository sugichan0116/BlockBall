using UnityEngine;
using UniRx;

public class Wall : MonoBehaviour
{
    public int hp = 10;
    
    public Subject<Collision2D> onCollide = new Subject<Collision2D>();
    public Subject<Unit> onDestroy = new Subject<Unit>();
    public Subject<int> onHit = new Subject<int>();

    // Start is called before the first frame update
    void Start()
    {
        onCollide
            .Select(c => c.collider.GetComponent<Ball>())
            .Subscribe(c => {
                hp -= c.strongth;
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
