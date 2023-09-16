using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Configuration;

namespace Extention;

public static class StringExtension
{
    public static string ConvertFirstLetterToUpper(this string input)
    {

        if (string.IsNullOrEmpty(input))
            return input;

        CultureInfo cultureInfo = new CultureInfo("en-US", false);
        TextInfo textInfo = cultureInfo.TextInfo;

        string[] words = input.Split(' ');

        for (int i = 0; i < words.Length; i++)
        {
            if (!string.IsNullOrEmpty(words[i]))
            {
                words[i] = textInfo.ToTitleCase(words[i].ToLower(cultureInfo));
            }
        }

        return string.Join(" ", words);
    }


    public static bool ContainsOnlySpecialCharacters(this string input)
    {
        // Regular expression pattern to match only special characters
        string pattern = "^[^a-zA-Z0-9]+$";

        // Check if the input matches the pattern
        return Regex.IsMatch(input, pattern);
    }

    public static bool ContainsOnlyNumbers(this string input)
    {

        string pattern = "^[0-9]+$";
        return Regex.IsMatch(input, pattern);
    }

    public static bool IsAccountNumberValid(this string accountNo)
    {
        return Regex.IsMatch(accountNo, "^[0-9]{10,10}$", RegexOptions.Compiled);
    }

    public static bool IsNameValid(this string mName)
    {
        return Regex.IsMatch(mName, "^[a-zA-Z.' -]{2,50}$", RegexOptions.Compiled);
    }

    public static bool IsEmailValid(this string mName)
    {
        return Regex.IsMatch(mName, "\\w+([-+.']\\w+)@\\w+([-.]\\w+)\\.\\w+([-.]\\w+)*", RegexOptions.Compiled);
    }

    public static List<int> ToSplit_Int(this string stringToSplit)
    {
        if (string.IsNullOrEmpty(stringToSplit))
        {
            return null;
        }

        List<int> mList = new List<int>();
        try
        {
            string[] array = new string[3] { ",", ";", "|" };
            if (!array.Any((string m) => stringToSplit.Contains(m)))
            {
                return stringToSplit.IsNumeric() ? new List<int> { int.Parse(stringToSplit) } : mList;
            }

            string[] array2 = stringToSplit.Split(array, StringSplitOptions.RemoveEmptyEntries);
            if (array2 == null || !array2.Any())
            {
                return mList;
            }

            array2.ForEachx(delegate (string m)
            {
                if (m.IsNumeric())
                {
                    mList.Add(int.Parse(m));
                }
            });
            return mList;
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
            return mList;
        }
    }

    public static List<long> ToSplit_Long(this string stringToSplit)
    {
        if (string.IsNullOrEmpty(stringToSplit))
        {
            return null;
        }

        List<long> mList = new List<long>();
        try
        {
            string[] array = new string[3] { ",", ";", "|" };
            if (!array.Any((string m) => stringToSplit.Contains(m)))
            {
                return stringToSplit.IsNumeric() ? new List<long> { long.Parse(stringToSplit) } : mList;
            }

            string[] array2 = stringToSplit.Split(array, StringSplitOptions.RemoveEmptyEntries);
            if (array2 == null || !array2.Any())
            {
                return mList;
            }

            array2.ForEachx(delegate (string m)
            {
                if (m.IsNumeric())
                {
                    mList.Add(long.Parse(m));
                }
            });
            return mList;
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
            return mList;
        }
    }

    public static List<string> ToSplit_Str(this string stringToSplit)
    {
        if (string.IsNullOrEmpty(stringToSplit))
        {
            return null;
        }

        List<string> mList = new List<string>();
        try
        {
            string[] array = new string[3] { ",", ";", "|" };
            if (!array.Any((string m) => stringToSplit.Contains(m)))
            {
                return new List<string> { stringToSplit.Trim() };
            }

            string[] array2 = stringToSplit.Split(array, StringSplitOptions.RemoveEmptyEntries);
            if (array2 == null || !array2.Any())
            {
                return mList;
            }

            array2.ForEachx(delegate (string m)
            {
                mList.Add(m.Trim());
            });
            return mList;
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
            return mList;
        }
    }

    public static string ToCamelCaseSplit(this string camelCasedString)
    {
        if (string.IsNullOrEmpty(camelCasedString))
        {
            return "";
        }

        try
        {
            IEnumerable<string> values = from Match m in Regex.Matches(camelCasedString, "([A-Z][a-z]+)")
                                         select m.Value;
            return string.Join(" ", values);
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
            return camelCasedString;
        }
    }

    public static string ToDigits(this string stringToGroup)
    {
        if (string.IsNullOrEmpty(stringToGroup))
        {
            return "";
        }

        try
        {
            if (!stringToGroup.IsNumeric())
            {
                return stringToGroup;
            }

            return $"{double.Parse(stringToGroup):0,0.00}".TrimStart(new char[1] { '0' });
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
            return stringToGroup;
        }
    }

    public static bool IsNumeric(this string myVal)
    {
        if (string.IsNullOrEmpty(myVal))
        {
            return false;
        }

        try
        {
            double result;
            return double.TryParse(myVal, NumberStyles.Any, CultureInfo.CurrentCulture, out result);
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
            return false;
        }
    }

    public static bool IsNaijaMobileNumberValid(this string gsmno)
    {
        try
        {
            return Regex.IsMatch(gsmno, "^0[7-9][0-9]\\d{8}$", RegexOptions.Compiled);
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
            return false;
        }
    }
}