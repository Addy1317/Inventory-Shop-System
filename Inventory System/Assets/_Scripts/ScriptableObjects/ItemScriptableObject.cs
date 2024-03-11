using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShopAndInventory
{
    [CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "ScriptableObject/NewItem")]
    public class ItemScriptableObject : ScriptableObject
    {
        [Header("Inventory Items")]
        public GameObject itemUIPrefab;
        public Sprite itemIcon;
        [Space]
        public string itemDescription;
        [Space]
        public ItemType type;
        public ItemRarity rarity;
        [Space] 
        public int quantity;
        public float weight;
        [Space]
        public int buyingPrice;
        public int sellingPrice;
       
    }
}