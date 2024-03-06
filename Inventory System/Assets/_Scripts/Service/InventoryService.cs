using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShopAndInventory
{
    public class InventoryService : MonoBehaviour
    {
        private GameObject inventoryUIPanel;
        private int itemLimit;
        private float weightLimit;

        private float currentWeight;
        private int currentcoinsOwned;

        private bool isGathering;

        private StorageUI inventoryUI;

        public List<ItemScriptableObject> inventoryItems { get; private set; }

        public InventoryService(InventoryScriptableObject inventoryScriptableObject, GameObject inventoryUiPanel, string dataLoadPath)
        {
            inventoryUIPanel = inventoryUiPanel;
            itemLimit = inventoryScriptableObject.itemLimit;
            weightLimit = inventoryScriptableObject.weightLimit;

            currentWeight = 0f;
            currentcoinsOwned = 0;

            isGathering = false;

            LoadData(dataLoadPath);
        }

        private void LoadData(string dataPath)
        {
            var emptyItem = Resources.LoadAll<ItemScriptableObject>(dataPath);
            inventoryItems = new List<ItemScriptableObject>();
            inventoryItems.Capacity = itemLimit;

            InitalizeInventoryUI(emptyItem);
        }

        private void InitalizeInventoryUI(ItemScriptableObject[] itemList)
        {
            var itemUIList = new List<ItemScriptableObject>(itemList);
            //inventoryUI = new StorageUI(inventoryUIPanel, itemUIList);
        }
    
        private void AddItemToInventory(ItemScriptableObject item, int quantity)
        {
            ItemScriptableObject itemFound = inventoryItems.Find((x) => x.name == item.name);
            int index = int.MinValue;

            if(!itemFound)
            {
                inventoryItems.Add(item);
                item.quantity = quantity;
            }
            else
            {
                itemFound.quantity += quantity;
                index = inventoryItems.IndexOf(itemFound);
            }

            //inventoryUI.AddItemToStorageUI(item, quantity, index);
            currentWeight += item.weight * quantity;

            if (!isGathering)
                currentcoinsOwned -= item.buyingPrice * quantity;

            if (currentcoinsOwned < 0)
                currentcoinsOwned = 0;

            EventService.Instance.onInventoryUpdated.InvokeEvent(currentcoinsOwned, currentWeight);
        }
        public void RemoveItemFromInventory(ItemScriptableObject item, int quantity)
        {
            ItemScriptableObject itemFound = inventoryItems.Find((x) => x.name == item.name);

            if (!itemFound || !CanRemoveItems(itemFound, quantity))
                return;

            int index = inventoryItems.IndexOf(itemFound);

            currentWeight -= itemFound.weight * quantity;
            currentcoinsOwned += itemFound.sellingPrice * quantity;

            if (itemFound.quantity - quantity == 0)
            {
                itemFound.quantity = 0;
                inventoryItems.Remove(itemFound);
            }
            else
            {
                itemFound.quantity -= quantity;
            }

            //inventoryUI.RemoveItemFromStorageUI(itemFound, quantity, index);
            EventService.Instance.onInventoryUpdated?.InvokeEvent(currentcoinsOwned, currentWeight);
        }

        public void FillInventory()
        {
            var resourcesGathered = Resources.LoadAll<ItemScriptableObject>("ItemSOs/ResourceGathering");

            isGathering = true;

            foreach (var item in resourcesGathered)
            {
                var newItem = GameObject.Instantiate<ItemScriptableObject>(item);
                newItem.name = item.name;

                if (!HasEnoughWeight(newItem, newItem.quantity))
                {
                    EventService.Instance.onItemAddFail.InvokeEvent(ItemAddFailType.Weight);
                    break;
                }

                AddItemToInventory(newItem, newItem.quantity);
            }

            isGathering = false;
        }

        public bool HasEnoughWeight(ItemScriptableObject item, int quantity)
        {
            if (currentWeight + item.weight * quantity > weightLimit)
                return false;

            return true;
        }

        public bool HasEnoughCoins(ItemScriptableObject item, int quantity)
        {
            if (currentcoinsOwned < item.buyingPrice * quantity)
                return false;

            return true;
        }

        private bool CanRemoveItems(ItemScriptableObject item, int quantity)
        {
            if (item.quantity < quantity)
                return false;
            return true;
        }
    }
}