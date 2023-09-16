using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
namespace Extention;

public static class GenericValidator
{
    public static bool ObjValid(this object model, out string msg)
    {
        List<ValidationResult> results;
        if (!EntityValidatorHelper.Validate(model, out results))
        {
            StringBuilder errorDetail = new StringBuilder();
            if (!results.IsNullOrEmpty())
            {
                errorDetail.AppendLine("Validation Error(s): ");
                int total = results.Count;
                int kounter = 0;
                results.ForEachx(m =>
                {
                    ++kounter;
                    if (kounter == total)
                        errorDetail.AppendLine(m.ErrorMessage);
                    else
                        errorDetail.AppendLine(m.ErrorMessage + "<br />");
                });
            }
            else
                errorDetail.AppendLine("Validation error occurred! Please check all supplied parameters and try again");
            msg = errorDetail.ToString();
            return false;
        }
        msg = "";
        return true;
    }

    public static void ForEachx<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (T item in source)
        {
            action(item);
        }
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
    {
        try
        {
            return collection == null || !collection.Any<T>();
        }
        catch
        {
            return false;
        }
    }



}




