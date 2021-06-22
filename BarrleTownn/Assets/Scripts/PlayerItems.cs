using System;
using UnityEngine;

[Serializable]
public class PlayerItems
{
	[SerializeField] GunSO playerGun;
	public GunSO getGun => playerGun;
	public int ammo;
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
			if (ammo > 0)
			{
				ammo -= 1;

				if (ammo <= 0)
					playerGun = null;
				if (playerGun == null)
					GameManager.getInstance.getPlayerItemsUI.UnEquipUI(2);

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
			if (playerArmor == null)
				GameManager.getInstance.getPlayerItemsUI.UnEquipUI(1);

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
		if (item as ShoeSO)
		{
			if (playerShoes == null)
			{
				playerShoes = (ShoeSO)item;
				return true;
			}
		}
		else if (item as ArmorSO)
		{
			if (playerArmor == null)
			{
				playerArmor = (ArmorSO)item;
				return true;
			}
		}
		else if (item as GunSO)
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
				ammo = playerGun.ammoAmount;
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
