using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Capstone
{
    public class Chips : Item
    {
        public override string Name { get; set; }
        public override decimal Price { get; set; }
        public override string ItemType { get; set; }
        public Chips(string name, decimal price, string itemType, int numberOfItem) :base(name, price, itemType, numberOfItem)
        {

        }
        public Chips(string name, decimal price, string itemType) : base(name, price, itemType)
        {

        }
        public override string EatingSounds()
        {
            return "Crunch Crunch, Yum!\n------------------------------------------------------";
        }
        public override void DispenseItem()
        {
            base.DispenseItem();
        }
    }
}
