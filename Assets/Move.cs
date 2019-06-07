using UnityEngine;
using UniRx;

public class Move : MonoBehaviour
{
    public float sensitivity = 0.1f;
    public float range = 3f;

    // Start is called before the first frame update
    void Start()
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
        
    }
}
