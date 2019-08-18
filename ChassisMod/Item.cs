﻿using ChassisMod.Wrapping;
using System.Reflection;

namespace ChassisMod
{
    public sealed partial class Item : Entity
    {
        public override int ID
        {
            get => base.ID;

            internal set
            {
                base.ID = value;
                _item.ID = value;
            }
        }

        public override string Name
        {
            get => base.Name;

            internal set
            {
                base.Name = value;
                _item.Name = value;
            }
        }

        public Container<int> TreeDamageBonus
        {
            get => _item.TreeDamageBonus;
            set => _item.TreeDamageBonus.Set(value, Assembly.GetCallingAssembly());
        }

        private readonly ItemWrapper _item = new ItemWrapper();

        internal Item()
        {
            
        }
    }
}
