using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Capstone
{
    public abstract class Item : IEatingSounds
    {
        public abstract string Name 
        {get; set;}
        public abstract decimal Price { get; set; }
        public abstract string ItemType { get; set; }
        public int NumberOfItem { get; private set; } = 5;

        public Item(string name, decimal price, string itemType, int numberOfItem)
        {
            this.Name = name;
            this.Price = price;
            this.ItemType = itemType;
            this.NumberOfItem = numberOfItem;
        }
        public Item(string name, decimal price, string itemType)
        {
            this.Name = name;
            this.Price = price;
            this.ItemType = itemType;
        }

        public abstract string EatingSounds();

        public virtual void DispenseItem()
        {
            this.NumberOfItem -= 1;
        }
    }
}
       

