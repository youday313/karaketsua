//EnumRapper
//作成日
//<summary>
//
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
public class EnumRapper
{

    public static TEnum GetEnum<TEnum>(string target)
    {
        if (string.IsNullOrEmpty(target)) throw new ArgumentException("Target is null.");

        var type = typeof(TEnum);

        if (!type.IsEnum) throw new NotSupportedException(string.Format("{0} is not enum.", type.Name));

        var members = Enum.GetNames(type);

        if (!members.Contains(target)) throw new ArgumentException("Target Name Not Found.");

        return (TEnum)Enum.Parse(type, members.Where(x => x == target).First(), true);
    }
}