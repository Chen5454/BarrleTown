using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NameHolder : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    [SerializeField] Vector3 offset;
    public Transform playerPos;




    Renderer targetRenderer;
    CanvasGroup _canvasGroup;
    Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void LateUpdate()
    {
        // Do not show the UI if we are not visible to the camera, thus avoid potential bugs with seeing the UI, but not the player itself.
        if (targetRenderer != null)
        {
            this._canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
        }


        // #Critical
        // Follow the Target GameObject on screen.
        if (playerPos != null)
        {
            this.transform.position = Camera.main.WorldToScreenPoint(playerPos.position) + offset;
        }
    }

}
