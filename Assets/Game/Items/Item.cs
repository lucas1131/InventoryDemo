using System;
using UnityEngine;

namespace InventoryDemo.Items
{
    [Serializable]
    public struct ItemDefinition : IEquatable<ItemDefinition>
    {
        public int Id;
        public int MaxStack;
        public Sprite Icon;
        public GameObject ItemPrefab;

        public override int GetHashCode() => Id;
        public bool Equals(ItemDefinition other) => this.Id == other.Id;
        public override bool Equals(object obj) => obj is ItemDefinition other && Equals(other);
        public static bool operator ==(ItemDefinition item1, ItemDefinition item2) => item1.Equals(item2);
        public static bool operator !=(ItemDefinition item1, ItemDefinition item2) => !item1.Equals(item2);

        public static ItemDefinition GetCDO() => new();

        public bool IsValid() => Id > 0;
    }

    [Serializable]
    public struct ItemData : IEquatable<ItemData>, IEquatable<ItemDefinition>
    {
        public int Amount;
        public ItemDefinition Data;

        public override int GetHashCode() => Data.Id;
        public override bool Equals(object obj) => obj is ItemData other && Equals(other);

        #region IEquatable<ItemData>

        public bool Equals(ItemData other) => this.Data.Id == other.Data.Id;
        public static bool operator ==(ItemData item1, ItemData item2) => item1.Equals(item2);
        public static bool operator !=(ItemData item1, ItemData item2) => !item1.Equals(item2);

        #endregion

        #region IEquatable<ItemDefinition>

        public bool Equals(ItemDefinition other) => this.Data.Id == other.Id;
        public static bool operator ==(ItemData item1, ItemDefinition item2) => item1.Equals(item2);
        public static bool operator !=(ItemData item1, ItemDefinition item2) => !item1.Equals(item2);

        #endregion
    }
}