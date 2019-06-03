using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play : SSController
{
    public void OnBackButtonTap()
    {
        SSSceneManager.Instance.Screen("Menu");
    }
}
