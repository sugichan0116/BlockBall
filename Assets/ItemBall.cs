using UnityEngine;
using UniRx;

public class ItemBall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Wall wall = GetComponent<Wall>();

        wall.hp = 1;

        //ボール増やす
        wall.onDestroy
            .Subscribe(_ => {
                FindObjectOfType<BallGenerator>()
                    .onGain.OnNext(1);
            });
    }
}
