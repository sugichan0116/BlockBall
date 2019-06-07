using DG.Tweening;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float time = 1, interval = 3, rotate = 360;
    private Sequence seq;

    // Start is called before the first frame update
    void Start()
    {
        seq = DOTween.Sequence();

        seq.Append(
            transform.DORotate(
                new Vector3(0f, 0f, rotate),   // 終了時点のRotation
                time,                    // アニメーション時間
                RotateMode.FastBeyond360
            ).SetDelay(interval).SetEase(Ease.InOutCirc)
        ).SetLoops(-1);

    }

    private void OnDestroy() => seq.Kill();
}
