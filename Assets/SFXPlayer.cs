using UnityEngine;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(AudioSource))]
public class SFXPlayer : MonoBehaviour
{
    public string targetTag;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource sfx = GetComponent<AudioSource>();

        this.OnCollisionEnter2DAsObservable()
            .Select(hit => hit.collider.transform.tag)
            .Where(tag => tag == targetTag)
            .Subscribe(_ => {
                sfx.PlayOneShot(sfx.clip);
            });
    }
}
