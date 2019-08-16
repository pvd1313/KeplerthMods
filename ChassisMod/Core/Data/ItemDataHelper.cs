﻿using System.Collections.Generic;
using ChassisMod.Core.Data.Util;
using System.Linq;
using DataBase;
using System;
using Common;

namespace ChassisMod.Core.Data
{
    internal sealed class ItemDataHelper : DataHelper<ConfigItem>
    {
        public static ItemDataHelper Instance { get; } = new ItemDataHelper();

        private ItemDataHelper() { }

        public override string NameFor(int entityID)
        {
            try
            {
                var item = Database[entityID];

                if (LanguageDataHelper.TryGetInEnglish(item.Name, out var name)) { return Language.Normalize(name); }
            }
            catch (Exception e) { Log.ExceptionOnce(e); }

            return "Item" + Language.Normalize(entityID);
        }

        public override IEnumerable<string> CommentFor(int entityID)
        {
            try
            {

                var result = new List<string>();
                var item = Database[entityID];

                if (LanguageDataHelper.TryGetInEnglish(item.Name, out var name)) { result.Add($"Name: \"{name}\""); }
                if (LanguageDataHelper.TryGetInEnglish(item.Description, out var description)) { result.Add($"Description: \"{description}\""); }
                if (LanguageDataHelper.TryGetInEnglish(item.FunctionDes, out var funcDes)) { result.Add($"Function: \"{funcDes}\""); }

                result.AddRange(FormatUtil.GetPropertiesWithValues(item, nameof(item.Name), nameof(item.Description), nameof(item.FunctionDes)));

                return result;
            }
            catch (Exception e) { Log.ExceptionOnce(e); }

            return new string[] { "???" };
        }
    }
}
