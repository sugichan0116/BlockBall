using UniRx;
using UnityEngine;
using System;
using TMPro;
using System.Linq;

public class Level : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Observable
            .EveryUpdate()
            .Select(_ => FindObjectOfType<Player>().level)
            .Subscribe(level => {
                GetComponent<TextMeshProUGUI>().text = "" + level;
            })
            .AddTo(gameObject);
    }
}
