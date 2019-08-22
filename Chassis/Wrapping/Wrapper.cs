﻿using System.Collections.Generic;
using System.Reflection;
using Chassis.Entities;
using Chassis.Patching;
using DataBase;
using Common;
using System;

namespace Chassis.Wrapping
{
    internal abstract partial class Wrapper<TConfig> : IWrapper<TConfig> where TConfig : DBBase
    {
        private static FieldInfo TableField { get; }

        internal static Dictionary<int, TConfig> Table
        {
            get
            {
                var table = TableField.GetValue(null);
                if (table == null) throw new Exception($"{typeof(TConfig).Name}.Table was null");

                return table as Dictionary<int, TConfig>;
            }
        }

        private readonly IEntity _owner;

        static Wrapper()
        {
            TableField = typeof(TConfig).GetField("Table", BindingFlags.Public | BindingFlags.Static);

            if (TableField == null)
            {
                Log.Error($"{typeof(TConfig).Name}.Table was not found!");
                return;
            }

            if (TableField.FieldType != typeof(Dictionary<int, TConfig>))
            {
                Log.Error($"{typeof(TConfig).Name}.Table has invalid type!");
            }
        }

        protected Wrapper(IEntity owner)
        {
            _owner = owner;
        }

        void IWrapper<TConfig>.AddPatch(Action patch, Assembly patcher) => ConfigPatcher.Add(patch, patcher);

        TConfig IWrapper<TConfig>.GetConfig()
        {
            if (Table.TryGetValue(_owner.ID, out var config))
            {
                if (config == null) throw new InvalidOperationException($"{_owner.Name}({_owner.ID}) was null");

                return config;
            }

            throw new InvalidOperationException($"{_owner.Name}({_owner.ID}) was not found in {typeof(TConfig).Name}.{nameof(Table)}");
        }

        public override string ToString() => _owner.Name;
    }
}
