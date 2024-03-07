using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShopAndInventory
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject itemInfoPanel;
        [SerializeField] private GameObject buySellPanel;

        [Header("Text Info")]
        [SerializeField] private TextMeshProUGUI currencyText;
        [SerializeField] private TextMeshProUGUI weightText;
        [SerializeField] private TextMeshProUGUI buySellInfoHeading;
        [SerializeField] private TextMeshProUGUI buySellInfoText;
        [SerializeField] private TextMeshProUGUI buySellButtonText;
        [SerializeField] private TextMeshProUGUI transactionStatusText;

        private GameObject inventoryPanel;
        private GameObject shopPanel;

        private ItemScriptableObject currentItemSelected;
        private int itemAmountSelected;

        private TextMeshProUGUI[] itemInfoPanelTexts;

        private enum CurrentAction
        {
            Buy,
            Sell
        };

        private CurrentAction currentAction;

        private void Awake()
        {
            itemInfoPanel.SetActive(false);
            buySellPanel.SetActive(false);

            EventService.Instance.onItemUIClickedEvent?.AddEventListener(PopUpInfoPanel);
            EventService.Instance.onInventoryUpdatedEvent?.AddEventListener(UpdateCurrencyAndWeight);
            EventService.Instance.onItemAddFail?.AddEventListener(HandleItemAdditionFailure);

            inventoryPanel = StorageController.Instance.GetInventoryPanel();
            shopPanel = StorageController.Instance.GetActivePanel();

            itemInfoPanelTexts = itemInfoPanel.GetComponentsInChildren<TextMeshProUGUI>();
            itemAmountSelected = 0;

            ClearTransactionStatusText();
        }

        private void OnDestroy()
        {
            EventService.Instance.onItemUIClickedEvent?.RemoveEventListener(PopUpInfoPanel);
            EventService.Instance.onInventoryUpdatedEvent?.RemoveEventListener(UpdateCurrencyAndWeight);
            EventService.Instance.onItemAddFail?.RemoveEventListener(HandleItemAdditionFailure);
        }
        private void HandleItemAdditionFailure(ItemAddFailType type)
        {
            DisplayTransactionStatusMessage($"NOT ENOUGH {type}!");
        }

        private void UpdateCurrencyAndWeight(int amountOwned, float weight)
        {
            currencyText.text = amountOwned.ToString();
            weightText.text = weight.ToString();
        }

        private void PopUpInfoPanel(int layer, int itemIndex)
        {
            if (buySellPanel.activeInHierarchy)
                CloseBuySellPanel();

            ItemScriptableObject item;
            if (layer == inventoryPanel.layer)
            {
                currentAction = CurrentAction.Sell;
                item = StorageController.Instance.GetInventoryService().inventoryItems[itemIndex];
                ShowInfo(item, item.sellingPrice);
            }
            else if (layer == shopPanel.layer)
            {
                currentAction = CurrentAction.Buy;
                item = StorageController.Instance.GetShopService().currentShopItemList[itemIndex];
                ShowInfo(item, item.buyingPrice);
            }
            else
            {
                currentItemSelected = null;
                itemInfoPanel.SetActive(false);
            }
        }

        private void ShowInfo(ItemScriptableObject itemInfo, int price)
        {
            buySellButtonText.text = currentAction.ToString();
            currentItemSelected = itemInfo;

            itemInfoPanel.SetActive(true);
            itemInfoPanelTexts[0].text = itemInfo.name.ToString();
            itemInfoPanelTexts[1].text = itemInfo.type.ToString();
            itemInfoPanelTexts[2].text = itemInfo.itemDescription.ToString();
            itemInfoPanelTexts[3].text = itemInfo.rarity.ToString();
            itemInfoPanelTexts[4].text = "Weight: " + itemInfo.weight.ToString();
            itemInfoPanelTexts[5].text = "Price: " + price.ToString() + " Qty: " + itemInfo.quantity.ToString();
        }

        public void ShowBuySellPanel()
        {
            OpenBuySellPanel();
        }

        public void ShowWeaponsPanel()
        {
            StorageController.Instance.SetActivePanel(ItemType.Weapon);
        }

        public void ShowConsumablesPanel()
        {
            StorageController.Instance.SetActivePanel(ItemType.Consumable);
        }

        public void ShowTreasuresPanel()
        {
            StorageController.Instance.SetActivePanel(ItemType.Treasure);
        }

        public void ShowMaterialsPanel()
        {
            StorageController.Instance.SetActivePanel(ItemType.Material);
        }

        public void ShowAllPanel()
        {
            StorageController.Instance.SetActivePanel(ItemType.None);
        }

        public void UpdateByIncrease()
        {
            if (itemAmountSelected >= currentItemSelected.quantity)
            {
                return;
            }

            int price = currentAction == CurrentAction.Buy ? currentItemSelected.buyingPrice : currentItemSelected.sellingPrice;
            buySellInfoText.text = $"{++itemAmountSelected} {currentItemSelected.name} FOR {itemAmountSelected * price} coins";
        }

        public void UpdateByDecrease()
        {
            if (itemAmountSelected <= 1)
            {
                return;
            }

            int price = currentAction == CurrentAction.Buy ? currentItemSelected.buyingPrice : currentItemSelected.sellingPrice;
            buySellInfoText.text = $"{--itemAmountSelected} {currentItemSelected.name} FOR {itemAmountSelected * price} coins";
        }

        public void ConfirmPayment()
        {

            if (currentAction == CurrentAction.Sell)
            {
                DisplayTransactionStatusMessage($"SOLD {itemAmountSelected} {currentItemSelected.name} for {currentItemSelected.sellingPrice * itemAmountSelected}");
                EventService.Instance.onSellTransactionInitiated.InvokeEvent(currentItemSelected, itemAmountSelected);
            }

            else if (currentAction == CurrentAction.Buy)
            {
                DisplayTransactionStatusMessage($"BOUGHT {itemAmountSelected} {currentItemSelected.name} for {currentItemSelected.buyingPrice * itemAmountSelected}");
                EventService.Instance.onBuyTransactionInitiated.InvokeEvent(currentItemSelected, itemAmountSelected);
            }

            CloseBuySellPanel();
            itemInfoPanel.SetActive(false);
        }

        private void DisplayTransactionStatusMessage(string message)
        {
            transactionStatusText.text = message;
            if (IsInvoking(nameof(ClearTransactionStatusText)))
            { 
                return;
            }
            Invoke(nameof(ClearTransactionStatusText), 3f);
        }

        private void ClearTransactionStatusText() => transactionStatusText.text = string.Empty;

        private void CloseBuySellPanel()
        {
            buySellPanel.SetActive(false);
            itemAmountSelected = 0;
        }

        private void OpenBuySellPanel()
        {
            buySellPanel.SetActive(true);
            UpdateByIncrease();
            buySellInfoHeading.text = $"HOW MANY DO YOU WANT TO {currentAction}?";
        }
    }
}