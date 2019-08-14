﻿using System.Collections.Generic;
using Common.Reflection;
using System;

namespace ChassisMod.Core
{
    partial class DataWrapper<TConfig>
    {
        public abstract class PropertyWrapper<TProperty, TPropertyID> where TPropertyID : PropertyIdentity
        {
            private sealed class WithSource : PropertyWrapper<TProperty, TPropertyID>
            {
                public DataWrapper<TConfig> Source { get; set; }

                internal override void Patch(DataWrapper<TConfig> target, string propertyName, Func<TConfig, TProperty> get, Action<TConfig, TProperty> set)
                {
                    var patchInfo = $"{target}.{propertyName} = {Source}.{propertyName}";
                    target.AddModification(patchInfo, config =>
                    {
                        var table = ReflectionHelper.GetStaticFieldValue<TConfig>("Table") as Dictionary<int, TConfig>;
                        var value = get(table[Source.ID]);
                        set(config, value);
                    });
                }
            }

            private sealed class WithData : PropertyWrapper<TProperty, TPropertyID>
            {
                public TProperty Data { get; set; }

                internal override void Patch(DataWrapper<TConfig> target, string propertyName, Func<TConfig, TProperty> get, Action<TConfig, TProperty> set)
                {
                    var patchInfo = $"{target}.{propertyName} = {Data}";
                    target.AddModification(patchInfo, config =>
                    {
                        set(config, Data);
                    });
                }
            }

            public static implicit operator PropertyWrapper<TProperty, TPropertyID> (TProperty data)
            {
                if (typeof(TProperty).IsClass && data == null)
                    throw new ArgumentNullException("data was null");

                return new WithData() { Data = data };
            }

            public static implicit operator PropertyWrapper<TProperty, TPropertyID>(DataWrapper<TConfig> source)
            {
                return new WithSource() { Source = source ?? throw new ArgumentNullException("source was null") };
            }

            internal abstract void Patch(DataWrapper<TConfig> target, string propertyName, Func<TConfig, TProperty> get, Action<TConfig, TProperty> set);
        }  
    }    
}
