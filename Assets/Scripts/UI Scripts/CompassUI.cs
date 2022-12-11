using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassUI : MonoBehaviour
{
    public RawImage compass;
    public Transform player;

    private void Awake()
    {
        compass = GameObject.Find("NESW").GetComponent<RawImage>();
        player = GameObject.Find("FPSPlayer").GetComponent<Transform>();
    }

    void Update()
    {
        compass.uvRect = new Rect(player.localEulerAngles.y / 360f, 0, 1, 1);
    }
}
