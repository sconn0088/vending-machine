using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Capstone
{
    public class Drinks : Item
    {
        public override string Name { get; set; }
        public override decimal Price { get; set; }
        public override string ItemType { get; set; }
        public Drinks(string name, decimal price, string itemType, int numberOfItem) : base(name, price, itemType, numberOfItem)
        {

        }
        public Drinks(string name, decimal price, string itemType) : base(name, price, itemType)
        {

        }
        public override string EatingSounds()
        {
            return "Glug Glug, Yum!\n------------------------------------------------------";
        }
        public override void DispenseItem()
        {
            base.DispenseItem();
        }
    }
}
