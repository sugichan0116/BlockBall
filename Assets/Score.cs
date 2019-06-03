using UniRx;
using UnityEngine;
using System;
using TMPro;
using System.Linq;

public class Score : MonoBehaviour
{
    public Subject<int> onChanged = new Subject<int>();
    public float score = 0;

    // Start is called before the first frame update
    void Start()
    {
        onChanged.Subscribe(delta => {
            score += delta;
        });

        Observable
            .EveryUpdate()
            .First(_ => FindObjectsOfType<Wall>().Count() == 0)
            .Subscribe(_ => {
                score += (int)(score * FindObjectOfType<Player>().voltage);
            })
            .AddTo(gameObject);

        Observable
            .EveryUpdate()
            .Subscribe(_ => {
                GetComponent<TextMeshProUGUI>().text = "" + score;
            })
            .AddTo(gameObject);
    }
}
