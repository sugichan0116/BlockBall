using UnityEngine;
using UniRx;
using System.Linq;
using System;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public float sensitivity = 0.1f;
    public float speed = 10f;
    public float range = 3f;
    public float error = 0f;

    public int level = 0;
    public float voltage = .2f;
    public Subject<int> onCollided = new Subject<int>();

    public enum State
    {
        IDLE,
        SETUP,
        ACTIVE
    }

    public State state;

    // Start is called before the first frame update
    void Awake()
    {
        //移動
        Observable
            .EveryUpdate()
            .Subscribe(_ => {
                transform.position += new Vector3(
                    Input.GetAxis("Horizontal"),
                    0
                ) * sensitivity;

                Vector2 position = transform.position;
                position.x = Mathf
                .Clamp(transform.position.x, -range, range);
                transform.position = position;
            })
            .AddTo(gameObject);

        //壁動かす準備
        Observable
            .EveryUpdate()
            .Where(_ => state == State.SETUP)
            .Subscribe(_ => {
                state = State.ACTIVE;
                
                foreach(var wall in FindObjectsOfType<Wall>())
                {
                    wall.transform
                        .DOMove(Vector2.down, .3f)
                        .SetRelative();
                }

                FindObjectOfType<WallGenerator>()
                    .onFired
                    .OnNext(++level);
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
                voltage -= .2f;
                state = State.IDLE;
                
                int i = 0;
                foreach(var ball in balls)
                {
                    ball.state = Ball.State.SETUP;
                    Observable.Timer(TimeSpan.FromMilliseconds(100 * ++i))
                        .Subscribe(_ => {
                            ball.state = Ball.State.FIRE;
                        });
                }
            })
            .AddTo(gameObject);

        //全消し
        Observable
            .EveryLateUpdate()
            .Where(_ => state == State.IDLE)
            .Where(_ => level > 0)
            .Where(_ => {
                return FindObjectsOfType<Wall>().Count() == 0;
            })
            .Subscribe(_ => {
                Reset();
                FindObjectOfType<Score>()
                    .onChanged
                    .OnNext(level * 1000);
            })
            .AddTo(gameObject);

        //スコア
        onCollided
            .Subscribe(weight => {
                int rate = (voltage > .8f) ? 5 : 1;
                voltage += 0.1f / level;

                FindObjectOfType<Score>()
                    .onChanged
                    .OnNext(weight * rate);
            });

        //clamp
        Observable
            .EveryLateUpdate()
            .Subscribe(_ => {
                voltage = Mathf.Clamp(voltage, 0f, 1f);
            })
            .AddTo(gameObject);
    }

    public void Reset()
    {
        foreach(var ball in FindObjectsOfType<Ball>())
        {
            ball.state = Ball.State.IDLE;
        }
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
