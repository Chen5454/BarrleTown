using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerItems
{
	[SerializeField] GunSO playerGun;
	public GunSO getGun => playerGun;
	[SerializeField] ArmorSO playerArmor;
	public int armor;
	public ArmorSO getArmor => playerArmor;
	[SerializeField] ShoeSO playerShoes;
	public ShoeSO getShoes => playerShoes;
	// Start is called before the first frame update

	public bool CanShoot()
	{
		if (playerGun != null)
		{
			if (playerGun.ammoAmount > 0)
			{
				playerGun.ammoAmount -= 1;
				
				if (playerGun.ammoAmount <= 0)
					playerGun = null;
				return true;
			}
		}

		return false;
	}

	


	public bool CanDamageArmor(int damage)
	{
		if (playerArmor != null)
		{
			armor -= damage;
			if (armor <= 0)
				playerArmor = null;
			return true;
		}
		return false;
	}
	public float GetShoeSpeed()
	{
		if (playerShoes != null)
			return playerShoes.shoeSpeed;
		else
			return 0;
	}

	public bool CanEquipItem(ItemSO item)
	{
		if(item as ShoeSO)
		{
			if(playerShoes == null)
			{
				playerShoes = (ShoeSO)item;
				return true;
			}
		}
		else if(item as ArmorSO)
		{
			if (playerArmor == null)
			{
				playerArmor = (ArmorSO)item;
				return true;
			}
		}
		else if(item as GunSO)
		{
			if (playerGun == null)
			{
				playerGun = (GunSO)item;
				return true;
			}
		}
		return false;
	}

	public void EquipItem(ItemSO item)
	{
		if (item as ShoeSO)
		{
			if (playerShoes == null)
			{
				playerShoes = (ShoeSO)item;
			}
		}
		else if (item as ArmorSO)
		{
			if (playerArmor == null)
			{
				playerArmor = (ArmorSO)item;
				this.armor = playerArmor.armourAmount;
			}
		}
		else if (item as GunSO)
		{
			if (playerGun == null)
			{
				playerGun = (GunSO)item;
			}
		}
	
	}


	public void UnEquipItem(int index)
	{
		if (index == 0)
		{
			playerShoes = null;
		}
		else if (index == 1)
		{
			playerArmor = null;
		}
		else if (index == 2)
		{
			playerGun = null;
		}
	}

}
