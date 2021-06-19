using Photon.Pun;
using UnityEngine;
using System.Collections;
public class InteractItem : MonoBehaviourPunCallbacks
{
	public RecipeItems contain;

	public VillagerCharacter player;
	//public bool canHide;
	public SpriteRenderer spriteRenderer;
	public OwnerShipTranfer transfer;
	public int barrleCurrentHp;
	public int barrleMaxHp;

	public bool IsBarrelALive()
    {
        if (barrleCurrentHp>0)
        {
			return true;
        }
        else
        {
			return false;
        }
		
    }

    public void SetGameObjectActive(bool _active,Vector3 pos)
	{
		if (PhotonNetwork.IsMasterClient)
			photonView.RPC("RPC_SetActive", RpcTarget.AllBufferedViaServer, _active, pos);
	}

	[PunRPC]
	void RPC_SetActive(bool _active, Vector3 pos)
	{
		gameObject.SetActive(_active);
		gameObject.transform.position = pos;
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
		barrleCurrentHp -= amount;
        if (!IsBarrelALive() && PhotonNetwork.IsMasterClient)
        {
			GameManager.getInstance.player.PlayerAppear(this.photonView.ViewID);
        }
	}


	public void DestoryBerrel()
    {
		this.gameObject.SetActive(false);
	}

	IEnumerator WaitBeforeDestory()
    {

		yield return new WaitForSeconds(0.5f);

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
