using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : SSController
{
    public void OnPlayButtonTap()
    {
        SSSceneManager.Instance.Screen("Play");
    }
}
