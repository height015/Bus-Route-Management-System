using System;
using Newtonsoft.Json;

namespace JosephProfile.Core;

public static class SessionExtentions
{
	public static void Set<T>(this ISession session, string key, T value)
	{
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    public static T Get<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        if (value == null)
            return default(T);

        return JsonConvert.DeserializeObject<T>(value);
    }


}

