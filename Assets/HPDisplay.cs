using TMPro;
using UniRx;
using UnityEngine;
using DG.Tweening;

public class HPDisplay : MonoBehaviour
{
    public TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {
        Wall wall = GetComponent<Wall>();

        //表示
        Observable
            .EveryUpdate()
            .Subscribe(_ => {
                text.text = "" + wall.hp;
            });

        Sequence seq = DOTween.Sequence();

        //動き
        wall.onHit
            .Where(hp => hp > 0)
            .Subscribe(_ => {
                seq.Append(text.transform.DOShakeScale(.1f));
                seq.Append(text.transform.DOScale(Vector3.one, .1f));
            });

        //wall.onDestroy
        //    .Subscribe(_ => {
        //        Debug.Log(seq + "/" + gameObject);
        //        seq.Complete();
        //        seq.Kill(true);
        //    });
    }
}
