using UnityEngine;
using UniRx;
using System.Linq;
using System;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour
{
    public float speed = 10f;
    public float error = 0f;

    public enum State
    {
        IDLE,
        SETUP,
        ACTIVE
    }

    public State state;

    // Start is called before the first frame update
    void Start()
    {

        //壁動かす準備
        Observable
            .EveryUpdate()
            .Where(_ => state == State.SETUP)
            .Subscribe(_ => {
                state = State.ACTIVE;

                foreach (var wall in FindObjectsOfType<Wall>())
                {
                    wall.transform
                        .DOMove(Vector2.down, .3f)
                        .SetRelative();
                }

                GetComponent<Player>()
                    .onWallMove.OnNext(Unit.Default);
            })
            .AddTo(gameObject);

        //ボール戻るまでまつ
        Observable
            .EveryUpdate()
            .Where(_ => state == State.IDLE)
            .Where(_ => {
                return FindObjectsOfType<Ball>()
                    .All(ball => ball.state == Ball.State.IDLE);
            })
            .Subscribe(_ => {

                state = State.SETUP;
            })
            .AddTo(gameObject);

        //発射
        Observable
            .EveryUpdate()
            .Where(_ => state == State.ACTIVE)
            .Where(_ => Input.GetMouseButtonDown(0))
            .Select(_ => FindObjectsOfType<Ball>())
            .Subscribe(balls => {
                GetComponent<Player>()
                    .onFire.OnNext(Unit.Default);

                state = State.IDLE;

                int i = 0;
                foreach (var ball in balls)
                {
                    ball.state = Ball.State.SETUP;
                    Observable.Timer(TimeSpan.FromMilliseconds(100 * ++i))
                        .Subscribe(_ => {
                            ball.state = Ball.State.FIRE;
                        })
                        .AddTo(gameObject);
                }
            })
            .AddTo(gameObject);

        //全消し
        Observable
            .EveryLateUpdate()
            .Where(_ => state == State.IDLE)
            .Where(_ => {
                return FindObjectsOfType<Wall>().Count() == 0;
            })
            .Subscribe(_ => {
                GetComponent<Player>()
                    .onWallClear.OnNext(Unit.Default);
            })
            .AddTo(gameObject);
    }
    
    public Vector2 LauncherForce()
    {
        Quaternion rotate = Quaternion
            .Euler(0, 0, Random.Range(-error, error));

        var pos = Camera.main.WorldToScreenPoint(transform.localPosition);
        rotate *= Quaternion.LookRotation(Vector3.forward, Input.mousePosition - pos);

        return rotate * Vector2.up * speed;
    }
}
