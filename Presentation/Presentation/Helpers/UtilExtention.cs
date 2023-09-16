using System;
using System.Globalization;

namespace Presentation.Helpers;

public static class UtilExtention
{
    public static void ForEachx<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (T obj in source)
            action(obj);
    }

    public static List<NameValueObject> ToNameValueList(this Type enumType)
    {
        if (enumType == null)
            return null;
        int[] values = (int[])Enum.GetValues(enumType);
        string[] names = Enum.GetNames(enumType);
        List<NameValueObject> nameValueList = new List<NameValueObject>();
        try
        {
            for (int index = 0; index < values.GetLength(0); ++index)
            {
                NameValueObject nameValueObject = new NameValueObject()
                {
                    Id = values[index]
                };
                if (names[index].IndexOf("_", StringComparison.Ordinal) > -1)
                    names[index] = names[index].Replace("_", " ");
                nameValueObject.Name = names[index];
                nameValueList.Add(nameValueObject);
            }
        }
        catch (Exception ex)
        {
            //TODO Implement Error Logging 
            throw new CustomException(ex.StackTrace!, ex.Source!, ex.GetBaseException().Message);
           
        }
        return nameValueList;
    }


    public static bool IsNumeric(this string myVal)
    {
        if (string.IsNullOrEmpty(myVal))
            return false;
        try
        {
            return double.TryParse(myVal, NumberStyles.Any, (IFormatProvider)CultureInfo.CurrentCulture, out double _);
        }
        catch (Exception ex)
        {
            throw new CustomException(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
           
        }
    }
}

