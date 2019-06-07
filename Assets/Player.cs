using UnityEngine;
using UniRx;
using System.Linq;

public class Player : MonoBehaviour
{
    public int level = 0;
    public float voltage = .2f;
    public int strongth = 1;

    public Subject<int> onCollided = new Subject<int>();
    public Subject<Unit> onWallMove = new Subject<Unit>();
    public Subject<Unit> onFire = new Subject<Unit>();
    public Subject<Unit> onWallClear = new Subject<Unit>();
    public Subject<int> onPowerUp = new Subject<int>();

    // Start is called before the first frame update
    void Awake()
    {
        onPowerUp
            .Subscribe(s => {
                Debug.Log(s + "/?" + strongth);
                strongth += s;
                foreach(var ball in FindObjectsOfType<Ball>())
                {
                    ball.strongth += s;
                }
            });

        onWallMove
            .Subscribe(_ => {
                FindObjectOfType<WallGenerator>()
                    .onFired
                    .OnNext(++level);
            });

        onWallMove
            .Subscribe(_ => {
                voltage -= .2f;
            });

        //スコア
        onCollided
            .Subscribe(weight => {
                int rate = (voltage > .8f) ? 5 : 1;
                voltage += 0.1f / level;

                FindObjectOfType<Score>()
                    .onChanged
                    .OnNext(weight * rate);
            });

        //全消し
        onWallClear
            .Where(_ => level > 0)
            .Subscribe(_ =>
            {
                Reset();
                FindObjectOfType<Score>()
                    .onChanged
                    .OnNext(level * 1000);
            });
        
        //clamp
        Observable
            .EveryLateUpdate()
            .Subscribe(_ => {
                voltage = Mathf.Clamp(voltage, 0f, 1f);
            })
            .AddTo(gameObject);
    }

    //comeback button
    public void Reset()
    {
        foreach(var ball in FindObjectsOfType<Ball>())
        {
            ball.state = Ball.State.IDLE;
        }
    }
}
