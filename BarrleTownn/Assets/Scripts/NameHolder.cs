using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class NameHolder : MonoBehaviourPunCallbacks
{
	public GameObject nameGO;
	public GameObject healthGO;
	public Slider healthSlider;


	public TextMeshProUGUI nameText;
	[SerializeField] Vector3 offset;
	public Transform playerPos;

	bool isActive;


	Renderer targetRenderer;
	CanvasGroup _canvasGroup;
	Vector3 targetPosition;

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
			int hp = playerPos.gameObject.GetComponent<VillagerCharacter>().currentHp;
			int maxhp = playerPos.gameObject.GetComponent<VillagerCharacter>().Maxhp;
			if (healthSlider.value != Map(hp, 0, maxhp, 0, 1))
			{
				healthSlider.value = Map(hp, 0, maxhp, 0, 1);
			}

		}
		if (playerPos == null)
			Destroy(this.gameObject);
		
	}


	public void DayPhase()
	{
		nameGO.SetActive(true);
		//healthGO.SetActive(false);
	}

	public void NightPhase()
	{
		nameGO.SetActive(false);
		//healthGO.SetActive(true);
	}

	public void ShowHPBar(bool _active)
	{
		healthGO.SetActive(_active);
	}

	




	public float Map(float value, float inMin, float inMax, float OutMin, float outMax)
	{
		return (value - inMin) * (outMax - OutMin) / (inMax - inMin) + OutMin;
	}







	

	
}
