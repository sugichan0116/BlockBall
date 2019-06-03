using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Voltage : MonoBehaviour
{
    public GameObject text;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();

        Observable
            .EveryUpdate()
            .Subscribe(_ => {
                slider.value = FindObjectOfType<Player>().voltage;
            })
            .AddTo(gameObject);

        Observable
            .EveryUpdate()
            .Subscribe(_ => {
                text.SetActive(slider.value > .8f);
            })
            .AddTo(gameObject);
    }
}
