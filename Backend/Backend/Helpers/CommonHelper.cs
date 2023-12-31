﻿using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using Configuration;

namespace Core;

public class CommonHelper
{
    #region Fields

    //we use EmailValidator from FluentValidation. So let's keep them sync - https://github.com/JeremySkinner/FluentValidation/blob/master/src/FluentValidation/Validators/EmailValidator.cs
    private const string EMAIL_EXPRESSION = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-||_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+([a-z]+|\d|-|\.{0,1}|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])?([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$";

    private static readonly Regex _emailRegex;

    #endregion

    #region Ctor

    static CommonHelper()
    {
        _emailRegex = new Regex(EMAIL_EXPRESSION, RegexOptions.IgnoreCase);
    }

    #endregion

    #region Methods


    public static string EnsureSubscriberEmailOrThrow(string email)
    {
        var output = EnsureNotNull(email);
        output = output.Trim();
        output = EnsureMaximumLength(output, 255);

        if (!IsValidEmail(output))
        {
            throw new Exception("Email is not valid.");
        }

        return output;
    }

   
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        email = email.Trim();

        return _emailRegex.IsMatch(email);
    }

    /// <summary>
    /// Verifies that string is an valid IP-Address
    /// </summary>
    /// <param name="ipAddress">IPAddress to verify</param>
    /// <returns>true if the string is a valid IpAddress and false if it's not</returns>
    public static bool IsValidIpAddress(string ipAddress)
    {
        return IPAddress.TryParse(ipAddress, out var _);
    }


    public static string EnsureMaximumLength(string str, int maxLength, string postfix = null)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        if (str.Length <= maxLength)
            return str;

        var pLen = postfix?.Length ?? 0;

        var result = str[0..(maxLength - pLen)];
        if (!string.IsNullOrEmpty(postfix))
        {
            result += postfix;
        }

        return result;
    }

    /// <summary>
    /// Ensures that a string only contains numeric values
    /// </summary>
    /// <param name="str">Input string</param>
    /// <returns>Input string with only numeric values, empty string if input is null/empty</returns>
    public static string EnsureNumericOnly(string str)
    {
        return string.IsNullOrEmpty(str) ? string.Empty : new string(str.Where(char.IsDigit).ToArray());
    }

    /// <summary>
    /// Ensure that a string is not null
    /// </summary>
    /// <param name="str">Input string</param>
    /// <returns>Result</returns>
    public static string EnsureNotNull(string str)
    {
        return str ?? string.Empty;
    }

    /// <summary>
    /// Indicates whether the specified strings are null or empty strings
    /// </summary>
    /// <param name="stringsToValidate">Array of strings to validate</param>
    /// <returns>Boolean</returns>
    public static bool AreNullOrEmpty(params string[] stringsToValidate)
    {
        return stringsToValidate.Any(string.IsNullOrEmpty);
    }

    /// <summary>
    /// Compare two arrays
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="a1">Array 1</param>
    /// <param name="a2">Array 2</param>
    /// <returns>Result</returns>
    public static bool ArraysEqual<T>(T[] a1, T[] a2)
    {
        //also see Enumerable.SequenceEqual(a1, a2);
        if (ReferenceEquals(a1, a2))
            return true;

        if (a1 == null || a2 == null)
            return false;

        if (a1.Length != a2.Length)
            return false;

        var comparer = EqualityComparer<T>.Default;
        return !a1.Where((t, i) => !comparer.Equals(t, a2[i])).Any();
    }

   
    /// <summary>
    /// Converts a value to a destination type.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="destinationType">The type to convert the value to.</param>
    /// <returns>The converted value.</returns>
    public static object To(object value, Type destinationType)
    {
        return To(value, destinationType, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Converts a value to a destination type.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="destinationType">The type to convert the value to.</param>
    /// <param name="culture">Culture</param>
    /// <returns>The converted value.</returns>
    public static object To(object value, Type destinationType, CultureInfo culture)
    {
        if (value == null)
            return null;

        var sourceType = value.GetType();

        var destinationConverter = TypeDescriptor.GetConverter(destinationType);
        if (destinationConverter.CanConvertFrom(value.GetType()))
            return destinationConverter.ConvertFrom(null, culture, value);

        var sourceConverter = TypeDescriptor.GetConverter(sourceType);
        if (sourceConverter.CanConvertTo(destinationType))
            return sourceConverter.ConvertTo(null, culture, value, destinationType);

        if (destinationType.IsEnum && value is int)
            return Enum.ToObject(destinationType, (int)value);

        if (!destinationType.IsInstanceOfType(value))
            return Convert.ChangeType(value, destinationType, culture);

        return value;
    }

    /// <summary>
    /// Converts a value to a destination type.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <typeparam name="T">The type to convert the value to.</typeparam>
    /// <returns>The converted value.</returns>
    /// 
    public static T To<T>(object value)
    {
        //return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        return (T)To(value, typeof(T));
    }

    /// <summary>
    /// Convert enum for front-end
    /// </summary>
    /// <param name="str">Input string</param>
    /// <returns>Converted string</returns>
    public static string ConvertEnum(string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;
        var result = string.Empty;
        foreach (var c in str)
            if (c.ToString() != c.ToString().ToLowerInvariant())
                result += " " + c.ToString();
            else
                result += c.ToString();

        //ensure no spaces (e.g. when the first letter is upper case)
        result = result.TrimStart();
        return result;
    }

    /// <summary>
    /// Get difference in years
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    public static int GetDifferenceInYears(DateTime startDate, DateTime endDate)
    {
        //source: http://stackoverflow.com/questions/9/how-do-i-calculate-someones-age-in-c
        //this assumes you are looking for the western idea of age and not using East Asian reckoning.
        var age = endDate.Year - startDate.Year;
        if (startDate > endDate.AddYears(-age))
            age--;
        return age;
    }

    /// <summary>
    /// Get DateTime to the specified year, month, and day using the conventions of the current thread culture
    /// </summary>
    /// <param name="year">The year</param>
    /// <param name="month">The month</param>
    /// <param name="day">The day</param>
    /// <returns>An instance of the Nullable<System.DateTime></returns>
    public static DateTime? ParseDate(int? year, int? month, int? day)
    {
        if (!year.HasValue || !month.HasValue || !day.HasValue)
            return null;

        DateTime? date = null;
        try
        {
            date = new DateTime(year.Value, month.Value, day.Value, CultureInfo.CurrentCulture.Calendar);
        }
        catch { }
        return date;
    }


    public static DateTime StringToDate(string dateString)
    {
        if (string.IsNullOrEmpty(dateString))
            return new DateTime();
        DateTime date = new DateTime();

        try
        {
        

            CultureInfo provider = CultureInfo.InvariantCulture;
            // It throws Argument null exception
            DateTime dateTime10 = DateTime.ParseExact(dateString, "MM/dd/yyyy", provider);
        }
        catch(Exception ex) {

            var message = ex.InnerException;
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return new DateTime();
        }
        return date;
    }

    #endregion

    #region Properties



    #endregion
}


