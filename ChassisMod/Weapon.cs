﻿using Common.Reflection;
using ChassisMod.Core;
using UnityEngine;
using DataBase;
using System;

namespace ChassisMod
{
    public sealed class Weapon : DataWrapper<ConfigWeapon>
    {
        public PropertyWrapper<string, Sprite, PropertyIdentity.ID0> Icon
        {
            get => this;
            set => value.Patch(this, "Icon", x => x.Texture, (x, v) => x.Texture = v, x => SpritePatcher.FindOrAdd(x));
        }

        public PropertyWrapper<float, PropertyIdentity.ID1> AttackSpeed
        {
            get => this;
            set => value.Patch(this, "AttackSpeed", x => x.AttackSpeed, (x, v) => x.AttackSpeed = Math.Max(v, .01f));
        }

        internal Weapon(string name, int id) : base(name, id) { }

        internal Weapon(string assemblyID, string name, int id) : base(assemblyID, name, id) { }

        internal void CopyFrom(Weapon source)
        {
            if (source == null) throw new ArgumentNullException("source was null");

            // var patchInfo = $"{this} = {source}";
            AddModification("", item => 
            {
                var data = ConfigWeapon.Table[source.ID];
                item.CopyInstanceFieldValues(data);
            });
        }
    }
}
