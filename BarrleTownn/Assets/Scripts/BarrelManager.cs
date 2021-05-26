using System.Collections;
using UnityEngine;

public class BarrelManager : MonoBehaviour
{
	//
	public BarrelSpawnRegion[] spawnRegions;
	public int minBarrelsSpawn;
	public int maxBarrelsSpawn;
	public bool canStartGeneration;
	public void GenerateBarrels()
	{
		if (canStartGeneration)
		{
			canStartGeneration = false;
			SpawnBarrelsAtAvailableLocations();
		}
		
	}

	public void SpawnBarrelsAtAvailableLocations()
	{	
			for (int i = 0; i < spawnRegions.Length; i++)
			{
			int randomizer = Random.Range(minBarrelsSpawn, maxBarrelsSpawn + 1);
			spawnRegions[i].RandomizeBarrelsSpawn(randomizer);
			}
			//RandomizeBarrelsSpawn(randomizer, spawnRegions[i].spawnLocations, i);
	}
}


