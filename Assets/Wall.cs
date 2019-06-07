using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;

public class Wall : MonoBehaviour
{
    public int hp = 10;
    
    public Queue<int> queue = new Queue<int>();
    public Subject<Unit> onMove = new Subject<Unit>();
    public Subject<Collision2D> onCollide = new Subject<Collision2D>();
    public Subject<Unit> onDestroy = new Subject<Unit>();
    public Subject<int> onHit = new Subject<int>();

    // Start is called before the first frame update
    void Awake()
    {
        onMove
            .Subscribe(_ => {
                queue.Enqueue(1);
            });

        Observable
            .EveryUpdate()
            .Sample(System.TimeSpan.FromSeconds(.5f))
            .Where(_ => queue.Count > 0)
            .Subscribe(_ => {
                queue.Dequeue();
                transform.DOMove(Vector3.down, .3f).SetRelative();
            })
            .AddTo(gameObject);

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
