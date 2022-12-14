using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassUI : MonoBehaviour
{
    public RawImage compass;
    public Transform player;

    private void Start()
    {
        compass = GameObject.Find("NESW").GetComponent<RawImage>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        compass.uvRect = new Rect(player.localEulerAngles.y / 360f, 0, 1, 1);
    }
}
