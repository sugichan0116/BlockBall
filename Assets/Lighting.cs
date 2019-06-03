using UnityEngine;
using UniRx;
using DG.Tweening;

public class Lighting : MonoBehaviour
{
    public Vector3 offset;
    public Vector3 delta = new Vector2(10, -10);
    public bool isPlay = false;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position;

        Observable
            .EveryUpdate()
            .Select(_ => FindObjectOfType<Player>())
            .Where(player => player != null && player.voltage > .8f)
            .Where(_ => isPlay == false)
            .Subscribe(_ => {
                isPlay = true;
                transform.position = offset;
                transform
                    .DOMove(offset + delta, 1f)
                    .SetDelay(1f)
                    .OnStart(() => {
                        
                    })
                    .OnComplete(() => isPlay = false);
            });
    }
}
