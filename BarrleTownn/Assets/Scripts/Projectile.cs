using Photon.Pun;
using UnityEngine;
public class Projectile : MonoBehaviourPunCallbacks
{
	public int projectileSpeed;
	public Rigidbody2D rb;
	public float timeToStayActive;
	public float timer;
	public bool isActive;
	public GameObject projGFX;
	Vector2 directionToGo;
	private void Update()
	{
		if (isActive)
		{
			rb.velocity = (projectileSpeed * directionToGo) * Time.deltaTime;
			if (timer > 0)
			{
				timer -= Time.deltaTime;
			}
			if (timer <= 0)
			{
				isActive = false;
				KillProj();
			}

		}
	}
	public void KillProj()
	{
		if (photonView.IsMine)
			photonView.RPC("RPC_KillProj", RpcTarget.All);
	}
	[PunRPC]
	public void RPC_KillProj()
	{
		timer = 0;
		isActive = false;
		this.gameObject.SetActive(false);
	}
	//direction, 0 = down, 1 = right, 2 = left, 3 = up
	public void InitProjectile(Transform pos, Vector2 direction)
	{
		if(photonView.IsMine)
		photonView.RPC("RPC_InitProjectile", RpcTarget.All, pos.position, direction);
	}

	[PunRPC]
	public void RPC_InitProjectile(Vector3 pos, Vector2 direction)
	{
		transform.position = pos;
		timer = timeToStayActive;


		SetDirection(ReturnIntDirectionByInt(direction));
		RotateGFX(ReturnIntDirectionByInt(direction));
		isActive = true;
		this.gameObject.SetActive(true);
	}

	int ReturnIntDirectionByInt(Vector2 direction)
	{
		if(direction.y == 0)
		{
			if (direction.x == 1)
				return 1;
			else if (direction.x == -1)
				return 2;
		}
		else if(direction.x == 0)
		{
			if (direction.y == 1)
				return 0;
			else if (direction.y == -1)
				return 3;
		}

		return -1;
	}


	void RotateGFX(int direction)
	{
		if (direction == 0)
		{
			projGFX.transform.localRotation = Quaternion.Euler(0, 0, -135f);
		}
		if (direction == 1)
		{
			projGFX.transform.localRotation = Quaternion.Euler(0, 0, -45f);
		}
		if (direction == 2)
		{
			projGFX.transform.localRotation = Quaternion.Euler(0, 0, 135f);
		}
		if (direction == 3)
		{
			projGFX.transform.localRotation = Quaternion.Euler(0, 0, 45f);
		}

	}

	void SetDirection(int direction)
	{
		if (direction == 0)
			directionToGo = new Vector2(0, -1);
		if (direction == 1)
			directionToGo = new Vector2(1, 0);
		if (direction == 2)
			directionToGo = new Vector2(-1, 0);
		if (direction == 3)
			directionToGo = new Vector2(0, 1);
	}



	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (photonView.IsMine)
		{
			if (collision.gameObject.tag == "Player")
			{
				if (collision.gameObject != GameManager.getInstance.player.gameObject)
				{
					if (collision.gameObject.GetComponent<VillagerCharacter>().Maxhp > 1 && GameManager.getInstance.getGamePhase == GamePhases.Night)
						collision.gameObject.GetComponent<VillagerCharacter>().GetDamage(1);

					KillProj();
				}
			}
			
		}
	}


}