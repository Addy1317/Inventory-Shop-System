using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShopAndInventory
{
    public class ShopService : MonoBehaviour
    {
        private GameObject mainShopPanel;
        private GameObject weaponsPanel;
        private GameObject consumablesPanel;
        private GameObject treasuresPanel;
        private GameObject materialsPanel;

        private StorageUI mainShopUI;
        private StorageUI weaponsUI;
        private StorageUI consumablesUI;
        private StorageUI treasuresUI;
        private StorageUI materialsUI;

        public List<ItemScriptableObject> currentShopItemList { get; private set; }
        public List<ItemScriptableObject> mainShopList { get; private set; }
        public List<ItemScriptableObject> weaponsList { get; private set; }
        public List<ItemScriptableObject> consumablesList { get; private set; }
        public List<ItemScriptableObject> treasuresList { get; private set; }
        public List<ItemScriptableObject> materialsList { get; private set; }


        public ShopService(GameObject _mainShopPanel, GameObject _weaponsPanel, GameObject _consumablesPanel, GameObject _treasuresPanel, GameObject _materialsPanel, string dataLoadPath)
        {
            mainShopPanel = _mainShopPanel;
            weaponsPanel = _weaponsPanel;
            consumablesPanel = _consumablesPanel;
            treasuresPanel = _treasuresPanel;
            materialsPanel = _materialsPanel;

            mainShopList = new List<ItemScriptableObject>();
            weaponsList = new List<ItemScriptableObject>();
            consumablesList = new List<ItemScriptableObject>();
            treasuresList = new List<ItemScriptableObject>();
            materialsList = new List<ItemScriptableObject>();
            currentShopItemList = new List<ItemScriptableObject>();

            currentShopItemList = mainShopList;
            LoadData(dataLoadPath);
        }

        private void LoadData(string dataLoadPath)
        {
            var shopItemsList = Resources.LoadAll<ItemScriptableObject>(dataLoadPath); //Loading all the shop items
            InitializeShopUI(shopItemsList);
        }

        private void InitializeShopUI(ItemScriptableObject[] shopItemsList)
        {
            foreach (var item in shopItemsList)
            {
                var newItem = GameObject.Instantiate(item);
                newItem.name = item.name;
                AddItemToLists(newItem, newItem.quantity);
            }

            mainShopUI = new StorageUI(mainShopPanel, mainShopList);
            weaponsUI = new StorageUI(weaponsPanel, weaponsList);
            consumablesUI = new StorageUI(consumablesPanel, consumablesList);
            treasuresUI = new StorageUI(treasuresPanel, treasuresList);
            materialsUI = new StorageUI(materialsPanel, materialsList);
        }

        public void SetCurrentList(ItemType listType)
        {
            switch (listType)
            {
                case ItemType.Weapon:
                    currentShopItemList = weaponsList; break;

                case ItemType.Consumable:
                    currentShopItemList = consumablesList; break;

                case ItemType.Treasure:
                    currentShopItemList = treasuresList; break;

                case ItemType.Material:
                    currentShopItemList = materialsList; break;

                default:
                    currentShopItemList = mainShopList; break;
            }
        }

        public void AddItemToShop(ItemScriptableObject item, int quantity)
        {
            ItemScriptableObject itemFound = mainShopList.Find((x) => x.name == item.name);
            int mainIndex = int.MinValue;
            int typeIndex = int.MinValue;

            if (!itemFound)
            {
                AddItemToLists(item, quantity);
            }

            else
            {
                mainIndex = mainShopList.IndexOf(itemFound);
                itemFound.quantity += quantity;
                FindItemInOtherLists(itemFound, out typeIndex);
            }

            mainShopUI.AddItemToStorageUI(item, quantity, mainIndex);
            AddItemToTypeUI(item, quantity, typeIndex);
        }

        private void AddItemToLists(ItemScriptableObject item, int quantity) 
        {
            mainShopList.Add(item);

            switch (item.type)
            {
                case ItemType.Weapon:
                    weaponsList.Add(item);
                    break;

                case ItemType.Consumable:
                    consumablesList.Add(item);
                    break;

                case ItemType.Treasure:
                    treasuresList.Add(item);
                    break;

                case ItemType.Material:
                    materialsList.Add(item);
                    break;

                default: break;
            }

            item.quantity = quantity;
        }
        private void AddItemToTypeUI(ItemScriptableObject item, int quantity, int index) 
        {
            switch (item.type)
            {
                case ItemType.Weapon:
                    weaponsUI.AddItemToStorageUI(item, quantity, index);
                    break;
                case ItemType.Consumable:
                    consumablesUI.AddItemToStorageUI(item, quantity, index);
                    break;
                case ItemType.Treasure:
                    treasuresUI.AddItemToStorageUI(item, quantity, index);
                    break;
                case ItemType.Material:
                    materialsUI.AddItemToStorageUI(item, quantity, index);
                    break;
                default: break;
            }
        }

        public void RemoveItemFromShop(ItemScriptableObject item, int quantity)
        {
            ItemScriptableObject itemFound = mainShopList.Find((x) => x.name == item.name);

            if (!itemFound || itemFound.quantity < quantity)
            {
                return;
            }

            int mainIndex = mainShopList.IndexOf(itemFound);
            int typeIndex = 0;
            var typeList = FindItemInOtherLists(itemFound, out typeIndex);

            if (itemFound.quantity - quantity == 0)
            {
                itemFound.quantity = 0;

                typeList.Remove(itemFound);
                mainShopList.Remove(itemFound);
            }
            else
            {
                itemFound.quantity -= quantity;
            }

            mainShopUI.RemoveItemFromStorageUI(itemFound, quantity, mainIndex);
            RemoveItemFromTypeUI(itemFound, quantity, typeIndex);
        }

        private List<ItemScriptableObject> FindItemInOtherLists(ItemScriptableObject item, out int index) 
        {
            List<ItemScriptableObject> typeList = new List<ItemScriptableObject>();
            switch (item.type)
            {
                case ItemType.Weapon:
                    index = weaponsList.IndexOf(item);
                    typeList = weaponsList;
                    break;

                case ItemType.Consumable:
                    index = consumablesList.IndexOf(item);
                    typeList = consumablesList;
                    break;

                case ItemType.Treasure:
                    index = treasuresList.IndexOf(item);
                    typeList = treasuresList;
                    break;

                case ItemType.Material:
                    index = materialsList.IndexOf(item);
                    typeList = materialsList;
                    break;

                default:
                    index = -1;
                    break;
            }
            return typeList;
        }

        private void RemoveItemFromTypeUI(ItemScriptableObject item, int quantity, int index) 
        {
            switch (item.type)
            {
                case ItemType.Weapon:
                    weaponsUI.RemoveItemFromStorageUI(item, quantity, index);
                    break;

                case ItemType.Consumable:
                    consumablesUI.RemoveItemFromStorageUI(item, quantity, index);
                    break;

                case ItemType.Treasure:
                    treasuresUI.RemoveItemFromStorageUI(item, quantity, index);
                    break;

                case ItemType.Material:
                    materialsUI.RemoveItemFromStorageUI(item, quantity, index);
                    break;

                default: break;
            }
        }
    }
}