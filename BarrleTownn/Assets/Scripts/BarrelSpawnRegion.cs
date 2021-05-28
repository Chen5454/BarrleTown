using Photon.Pun;
using UnityEngine;
public class BarrelSpawnRegion : MonoBehaviourPunCallbacks
{
	public Transform[] spawnLocations;
	public bool[] hasBarrelNearBy;
	public float barrelDetectorRange;
	public LayerMask barrelMask;
	public Transform barrelParent;
	public GameObject barrelPF;
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			//RandomizeBarrelsSpawn(2);
			////CheckNearByBarrels();
			//Debug.Log("Checking barrels");
		}
	}
	public void CheckNearByBarrels()
	{
		hasBarrelNearBy = new bool[spawnLocations.Length];

		for (int i = 0; i < spawnLocations.Length; i++)
		{
			Collider2D[] barrels = Physics2D.OverlapBoxAll(spawnLocations[i].position, new Vector2(barrelDetectorRange, barrelDetectorRange), 0, barrelMask);
			//Debug.Log("Amount of barrels at: " + i+ " amount: " +barrels.Length);
			if (barrels.Length > 0)
			{
				hasBarrelNearBy[i] = true;
			}
			else
			{
				hasBarrelNearBy[i] = false;
			}


		}
	}

	public void RandomizeBarrelsSpawn(int amount)
	{
		int loopIndex = amount;

		CheckNearByBarrels();




		Debug.Log("Loop: " + loopIndex);
		for (int i = 0; i < spawnLocations.Length; i++)
		{
			if (loopIndex > 0)
			{

				if (!CheckIfRegionIsFull())
				{
					int spawnLocationIndex = Random.Range(0, spawnLocations.Length);
					if (hasBarrelNearBy[spawnLocationIndex])
					{

						Debug.Log("Can't generate barrel at: " + spawnLocationIndex + " Remaining attempts: " + loopIndex);
						RandomizeBarrelsSpawn(loopIndex);
						break;
					}
					else
					{

						//spawn barrel at location
						InstantiateBarrel(spawnLocations[spawnLocationIndex]);
						loopIndex -= 1;
						Debug.Log("Generating Barrels: " + spawnLocationIndex + " Remaining attempts: " + loopIndex);
						if (loopIndex > 0)
							RandomizeBarrelsSpawn(loopIndex);

						break;
					}
				}
				CheckNearByBarrels();
			}

		}

	}
	public bool CheckIfRegionIsFull()
	{
		int barrelAmount = 0;

		for (int i = 0; i < hasBarrelNearBy.Length; i++)
		{
			if (hasBarrelNearBy[i])
				barrelAmount += 1;
		}


		if (barrelAmount == hasBarrelNearBy.Length)
		{
			Debug.Log("Region is full");
			return true;
		}
		else
		{
			Debug.Log("Region is not full");
			return false;
		}

	}

	public void InstantiateBarrel(Transform trans)
	{

		//GameObject barrel = Instantiate(barrelPF, trans.position, new Quaternion() ,barrelParent);

		//multiplayer instantiate
		if (PhotonNetwork.IsMasterClient)
		{
			GameObject barrel = PhotonNetwork.Instantiate("Barrel", trans.position, new Quaternion());
			_barrel = barrel;
			_barrel.GetComponent<InteractItem>().contain = RandomizeBarrelType();
			//photonView.RPC("RPC_RandomizeBarrel", RpcTarget.AllBufferedViaServer,RandomizeBarrelType());
			//barrel.GetComponent<InteractItem>().contain = RandomizeBarrelType();
			barrel.transform.SetParent(barrelParent);
		}
	}





	GameObject _barrel;
	[PunRPC]
	void RPC_RandomizeBarrel(RecipeItems _item)
	{
		
		_barrel.GetComponent<InteractItem>().contain = _item;
	}
	RecipeItems RandomizeBarrelType()
	{
		int randomizer = Random.Range(1, 3);
		RecipeItems test = (RecipeItems)randomizer;
		return test;
	}


	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		for (int i = 0; i < spawnLocations.Length; i++)
		{
			Gizmos.DrawWireCube(spawnLocations[i].position, new Vector2(barrelDetectorRange, barrelDetectorRange));
		}
	}


}

