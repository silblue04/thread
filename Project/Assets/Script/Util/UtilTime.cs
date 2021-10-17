using System;
using System.Collections;

public class UtilTime
{
    public static bool IsItSameDay(DateTime res, DateTime des)
    {
        return (res.Year == des.Year && res.Month == des.Month && res.Day == des.Day);
    }

    public static bool IsItOver(DateTime target, DateTime standard)
    {
        if(target.Year > standard.Year)
        {
            return true;
        }

        if(target.Month > standard.Month)
        {
            return true;
        }

        if(target.Day > standard.Day)
        {
            return true;
        }

        if(target.Hour > standard.Hour)
        {
            return true;
        }

        if(target.Minute > standard.Minute)
        {
            return true;
        }

        if(target.Second > standard.Second)
        {
            return true;
        }

        return false;
    }
    public static bool IsItOver(long target, long standard)
    {
        if(target > standard)
        {
            return true;
        }

        return false;
    }

    public static int GetTimeInterval(DateTime des, DateTime res)
    {
        return System.Convert.ToInt32(Math.Round((des - res).TotalSeconds));
    }
    public static int GetTimeInterval(long des, long res)
    {
        long elapsedTicks = des - res;
        TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
        int elapsedTime = System.Convert.ToInt32(elapsedSpan.TotalSeconds);

        return elapsedTime;
    }

    public static string GetTimeString(int second)
    {
        const int secPerMin     = 60;
		const int secPerHour    = 60 * secPerMin;
		const int secPerDay     = 24 * secPerHour;

		int sec     = (int)second;

		int day     = (int)(sec / secPerDay);		sec -= day * secPerDay;
		int hour    = (int)(sec / secPerHour);		sec -= hour * secPerHour;
		int min     = (int)(sec / secPerMin);		sec -= min * secPerMin;
		

        int frontValue = DefsDefault.VALUE_ZERO;
        int backValue = DefsDefault.VALUE_ZERO;

        string frontTailText = string.Empty;
        string backTailText = string.Empty;

        if(day > DefsDefault.VALUE_ZERO)
        {
            frontValue = day;
            backValue = hour;

            frontTailText = Localization.Get(DefsUI.LocalizationKey.TIME_DAY);
            backTailText = Localization.Get(DefsUI.LocalizationKey.TIME_HOUR);
        }
        else if(hour > DefsDefault.VALUE_ZERO)
        {
            frontValue = hour;
            backValue = min;

            frontTailText = Localization.Get(DefsUI.LocalizationKey.TIME_HOUR);
            backTailText = Localization.Get(DefsUI.LocalizationKey.TIME_MIN);
        }
        else if(min > DefsDefault.VALUE_ZERO)
        {
            frontValue = min;
            backValue = sec;

            frontTailText = Localization.Get(DefsUI.LocalizationKey.TIME_MIN);
            backTailText = Localization.Get(DefsUI.LocalizationKey.TIME_SEC);
        }
        else
        {
            frontValue = sec;
            frontTailText = Localization.Get(DefsUI.LocalizationKey.TIME_SEC);
        }

        if(backValue == DefsDefault.VALUE_ZERO)
        {
            return string.Format(DefsString.TextFormat.TIME_STRING_SHORT_FORMAT, frontValue, frontTailText);
        }
        else
        {
            return string.Format(DefsString.TextFormat.TIME_STRING_FORMAT, frontValue, frontTailText, backValue, backTailText);
        }
    }

    static private IEnumerator _WaitTimeCallback(float time, Action Callback)
    {
        yield return YieldInstructionCache.WaitForSeconds(time);
        Callback?.Invoke();
    }

    static public void WaitTimeAfterCallback(float time, Action Callback)
    {
        Main.Instance.StartCoroutine(_WaitTimeCallback(time, Callback));
    }
}
