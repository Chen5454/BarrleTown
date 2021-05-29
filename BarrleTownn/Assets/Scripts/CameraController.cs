using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
       
    // Update is called once per frame
    void Update()
    {
        if (GameManager.getInstance != null)
            if (GameManager.getInstance.player != null)
            {
                Vector3 cameraPos = new Vector3(GameManager.getInstance.player.transform.position.x, GameManager.getInstance.player.transform.position.y, -10f);

                transform.position = cameraPos;
            }
    }
}
