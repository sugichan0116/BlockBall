using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class WallGenerator : MonoBehaviour
{
    public List<Wall> prefabs;
    public Vector2 offset;
    public float rate = .3f;

    public Subject<int> onFired = new Subject<int>();

    // Start is called before the first frame update
    void Start()
    {
        onFired
            .Subscribe(level => {
                //unirxで書き換えたい
                for (int i = 0; i < 8; i++)
                {
                    if(level % 10 == 0 || rate <= Random.Range(0f, 1f))
                    {
                        Wall wall = Instantiate(Prefab());
                        wall.transform.position = offset + Vector2.right * i;
                        wall.hp = level;
                        wall.transform.parent = transform;
                    }
                }
            });
    }

    private Wall Prefab()
    {
        int i = Random.Range(0, prefabs.Count);
        return prefabs[i];
    }
}
