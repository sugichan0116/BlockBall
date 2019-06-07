using UnityEngine;
using UniRx;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    public enum State {
        IDLE,
        SETUP,
        FIRE,
        ACTIVE
    }

    public State state = State.IDLE;
    public int strongth = 1;

    private Gun gun;
    private Vector3 offset = new Vector3(0, .2f, 0);
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        gun = FindObjectOfType<Gun>();
        rb = GetComponent<Rigidbody2D>();

        //発射
        Observable
            .EveryUpdate()
            .Where(_ => state == State.FIRE)
            .Subscribe(_ => {
                state = State.ACTIVE;
                Launch();
            })
            .AddTo(gameObject);

        //初期位置へ
        Observable
            .EveryUpdate()
            .Where(_ => state != State.ACTIVE)
            .Subscribe(_ => {
                if (gun == null) return;
                Vector3 target = gun.transform.position + offset;
                transform.DOMove(target, .3f);
            })
            .AddTo(gameObject);
    }

    private void Launch()
    {
        rb.position = gun.transform.position;
        rb.velocity = Vector3.zero;
        rb.AddForce(gun.LauncherForce(), ForceMode2D.Impulse);
    }
}
