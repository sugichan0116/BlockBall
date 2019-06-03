using UnityEngine;
using UniRx;
using System.Linq;
using TMPro;

public class BallCount : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Observable
            .EveryUpdate()
            .Select(_ => FindObjectsOfType<Ball>().Count())
            .Subscribe(count => {
                GetComponent<TextMeshProUGUI>().text = "x" + count;
            })
            .AddTo(gameObject);
    }
}
