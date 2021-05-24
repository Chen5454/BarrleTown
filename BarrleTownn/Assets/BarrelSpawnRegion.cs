using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSpawnRegion : MonoBehaviour
{
    public Transform[] spawnLocations;
	public bool[] hasBarrelNearBy;
	public float barrelDetector;
	public LayerMask barrelMask;


    public void CheckNearByBarrels(int minBarrels,int maxBarrels)
	{
		for (int i = 0; i < spawnLocations.Length; i++)
		{
			if(Physics2D.OverlapBoxAll(spawnLocations[i].position, new Vector2(barrelDetector, barrelDetector),0,barrelMask) == null)
			{
				hasBarrelNearBy[i] = false;
			}
			else
			{
				hasBarrelNearBy[i] = true;
			}
			
		}
	}


	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		for (int i = 0; i < spawnLocations.Length; i++)
		{
			Gizmos.DrawWireCube(spawnLocations[i].position, new Vector2( barrelDetector, barrelDetector));
		}
	}


}
