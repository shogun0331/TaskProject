
public static class GameUtil
{
    public static string FormatKmt(int number)
    {
        if (number >= 1000000000) // 10억 이상 (t)
        {
            return (number / 1000000000.0).ToString("0.##") + "t";
        }
        else if (number >= 1000000) // 100만 이상 (m)
        {
            return (number / 1000000.0).ToString("0.##") + "m";
        }
        else if (number >= 1000) // 1000 이상 (k)
        {
            return (number / 1000.0).ToString("0.##") + "k";
        }
        else
        {
            return number.ToString();
        }
    }

}
