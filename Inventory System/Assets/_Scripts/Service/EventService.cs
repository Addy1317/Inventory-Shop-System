using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShopAndInventory
{
    public class EventService : GenericMonoSingleton<EventService>
    {
        public EventController<int, int> onItemUIClickedEvent;
        public EventController<int, float> onInventoryUpdatedEvent;

        public EventController<ItemAddFailType> onItemAddFail;
        public EventController<ItemScriptableObject, int> onBuyTransactionInitiated;
        public EventController<ItemScriptableObject, int> onSellTransactionInitiated;

        protected override void Awake()
        {
            base.Awake();
        }

        public EventService()
        {
            onItemUIClickedEvent = new EventController<int, int>();
            onInventoryUpdatedEvent = new EventController<int, float>();
            onItemAddFail = new EventController<ItemAddFailType>();
            onBuyTransactionInitiated = new EventController<ItemScriptableObject, int>();
            onSellTransactionInitiated = new EventController<ItemScriptableObject, int>();
        }
    }
}