using Photon.Pun;
using System.Collections;
using UnityEngine;
public class InteractItem : MonoBehaviourPunCallbacks
{
	public RecipeItems contain;

	public VillagerCharacter player;
	//public bool canHide;
	public SpriteRenderer spriteRenderer;
	public OwnerShipTranfer transfer;
	public int barrleCurrentHp;
	public int barrleMaxHp;
	// 0 = wood, 1 = leather, 2 = iron
	public Sprite[] itemSprites;

	public bool IsBarrelALive()
	{
		if (this.barrleCurrentHp > 0)
		{
			return true;
		}
		else
		{
			return false;
		}

	}

	public void SetGameObjectActive(bool _active, Vector3 pos, RecipeItems _item)
	{
		if (PhotonNetwork.IsMasterClient)
			photonView.RPC("RPC_SetActive", RpcTarget.AllBufferedViaServer, _active, pos, (int)_item);
	}

	[PunRPC]
	void RPC_SetActive(bool _active, Vector3 pos, int _item)
	{
		RecipeItems item = (RecipeItems)_item;
		this.contain = item;
		gameObject.SetActive(_active);
		gameObject.transform.position = pos;
	}

	public void PlayerHiding(VillagerCharacter _player)
	{
		int viewID = 0;
		if (_player != null)
		{
			viewID = _player.photonView.ViewID;
			player = _player;
		}
		else
		{
			viewID = -1;
			player = null;
		}


			photonView.RPC("RPC_PlayerHiding", RpcTarget.AllBuffered, viewID);
	}
	[PunRPC]
	public void RPC_PlayerHiding(int id)
	{
		if (id != -1)
			player = PhotonView.Find(id).GetComponent<VillagerCharacter>();
		else
			player = null;
	}
	public void HpBarrel()
	{
		if (barrleCurrentHp > barrleMaxHp)
		{
			barrleCurrentHp = barrleMaxHp;
		}
	}

	public void BerrelGetDamage(int amount)
	{
		photonView.RPC("RPC_BerrelGetDamage", RpcTarget.AllBufferedViaServer, amount);
	}


	[PunRPC]
	public void RPC_BerrelGetDamage(int amount)
	{
		this.barrleCurrentHp -= amount;
		if (!IsBarrelALive())
		{
			if (player != null)
				//if (player == GameManager.getInstance.player)
				GameManager.getInstance.player.PlayerAppear(this.photonView.ViewID);


			DestoryBerrel();
		}

	}


	public void DestoryBerrel()
	{
		//player.photonView.RPC("RPC_PlaySound", RpcTarget.All, "BarrelDestroy");
		this.gameObject.SetActive(false);
		player = null;
	}

	IEnumerator WaitBeforeDestory()
	{

		yield return new WaitForSeconds(0.5f);

	}


	public Sprite ReturnSpriteByBarrelType()
	{
		switch (contain)
		{
			case RecipeItems.Empty:
				return null;
			case RecipeItems.Wood:
				return itemSprites[0];

			case RecipeItems.Leather:
				return itemSprites[1];
			case RecipeItems.Metal:
				return itemSprites[2];
			default:
				break;
		}
		return null;
	}

	//private void OnCollisionStay2D(Collision2D other)
	//{
	//    if (other.collider.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
	//    {
	//        canHide = true;
	//        spriteRenderer.sortingOrder = 2;
	//        player.playeRenderer.enabled = false;
	//        //player.transform.position = gameObject.transform.position;
	//        player.canMove = false;
	//        transfer.PickingUp();
	//    }
	//    else if (Input.GetKeyUp(KeyCode.E))
	//    {
	//        canHide = false;
	//        spriteRenderer.sortingOrder = 0;
	//        player.canMove = true;
	//        player.playeRenderer.enabled = true;
	//    }
	//}

	//private void OnTriggerEnter2D(Collider2D other)
	//{
	//    if (other.gameObject.CompareTag("Player"))
	//    {
	//        canHide = true;
	//    }
	//}

	//private void OnTriggerExit2D(Collider2D other)
	//{
	//    if (other.gameObject.CompareTag("Player"))
	//    {
	//        canHide = false;
	//    }
	//}
}
