using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

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
                foreach (var wall in FindObjectsOfType<Wall>())
                {
                    wall.onMove.OnNext(Unit.Default);
                }

                foreach (var i in Enumerable.Range(0, 8))
                {
                    if(level % 10 == 0 || rate <= Random.Range(0f, 1f))
                    {
                        Wall wall = Instantiate(Prefab(), transform);
                        wall.transform.position = offset + Vector2.right * i;
                        wall.hp = level;
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
