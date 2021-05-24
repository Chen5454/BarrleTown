using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelManager : MonoBehaviour
{
    //
    public BarrelSpawnRegion[] spawnRegions;
	public int minBarrelsSpawn;
	public int maxBarrelsSpawn;

    public void GenerateBarrels()
	{
		//spawn barrels in each region, each region has randomized amount of barrels that will spawn and randomly location in the region
		//this checks each region's each spawn location if there is a barrel near it
		for (int i = 0; i < spawnRegions.Length; i++)
		{
			spawnRegions[i].CheckNearByBarrels(minBarrelsSpawn, maxBarrelsSpawn);
		}
	}
	public void SpawnBarrelsAtAvailableLocations()
	{

	}

 
}
