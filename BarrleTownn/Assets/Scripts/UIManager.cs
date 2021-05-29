using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager getInstance => _instance;
    public ShopUI shop;


	private void Awake()
	{
		if(_instance == null)
		{
            _instance = this;
		}
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.V))
		{
            
        }
    }
}
