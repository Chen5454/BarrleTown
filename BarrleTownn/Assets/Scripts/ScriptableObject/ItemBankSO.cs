using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New BankSO", menuName = "Bank")]
public class ItemBankSO : ScriptableObject
{
   public List<ItemSO> itemList;
}
