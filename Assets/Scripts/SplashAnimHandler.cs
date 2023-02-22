using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashAnimHandler : MonoBehaviour
{
    public void PlayerDone()
    {
        //GameDb.LoadingSceneAsync("S0");
        GameDb.LoadingScene("S0");
    }
}
