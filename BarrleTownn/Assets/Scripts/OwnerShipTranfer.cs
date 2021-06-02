using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class OwnerShipTranfer : MonoBehaviourPun, IPunOwnershipCallbacks
{


	private void Awake()
	{
		PhotonNetwork.AddCallbackTarget(this);
	}
	private void OnDestroy()
	{
		PhotonNetwork.RemoveCallbackTarget(this);
	}


	public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
	{
		Debug.Log("Changing ownership: " + requestingPlayer.NickName);
		if (targetView != base.photonView)
			return;

		base.photonView.TransferOwnership(requestingPlayer);

	}

	public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
	{
		if (targetView != base.photonView)
			return;
	}

	public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
	{
		
	}

	public void PickingUp()
	{
		base.photonView.RequestOwnership();
	}
}
