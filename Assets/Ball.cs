using UnityEngine;
using UniRx;
using DG.Tweening;
using System;

public class Ball : MonoBehaviour
{
    public enum State {
        IDLE,
        SETUP,
        FIRE,
        ACTIVE
    }

    public State state = State.IDLE;
    Player player;
    Vector3 offset = new Vector3(0, .2f, 0);
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();

        //発射
        Observable
            .EveryUpdate()
            .Where(_ => state == State.FIRE)
            .Subscribe(_ => {
                state = State.ACTIVE;
                Launch();
            });
        
        //初期位置へ
        Observable
            .EveryUpdate()
            .Where(_ => state != State.ACTIVE)
            .Subscribe(_ => {
                if (player == null) return;
                Vector3 target = player.transform.position + offset;
                transform.DOMove(target, .3f);
            });
    }

    private void Launch()
    {
        rb.position = player.transform.position;
        rb.velocity = Vector3.zero;
        rb.AddForce(player.LauncherForce(), ForceMode2D.Impulse);
    }
}
