using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShopAndInventory
{
    [CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "ScriptableObject/NewItem")]
    public class ItemScriptableObject : ScriptableObject
    {
        public GameObject itemUIPrefab;
        public string itemDescription;
        public ItemType type;
        public ItemRarity rarity;
        public Sprite itemIcon;
        public int quantity;
        public int buyingPrice;
        public int sellingPrice;
        public float weight;
    }
}