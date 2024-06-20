using UnityEngine;

public class GameUtils
{
    public static float Interpolate(float input, float minInput, float maxInput, float minOutput, float maxOutput)
    {
        float t = Mathf.InverseLerp(minInput, maxInput, input);
        return Mathf.RoundToInt(Mathf.Lerp(minOutput, maxOutput, t));
    }

    public static float Map(float input, float minInput, float maxInput, float minOutput, float maxOutput)
    {
        return (input - minInput) * (maxOutput - minOutput) / (maxInput - minInput) + minInput;
    }

    public static float ReverseLinear(float curInput, float maxInput, float minOutput, float maxOutput)
    {
        return minOutput + (maxOutput - minOutput) * (1 - (float)curInput / maxInput);
    }

    public static Color HexToRGB(string hex)
    {
        hex = hex.Replace("#", "");
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }

    public static string CurrencyFormat(int currency)
    {
        return string.Format("{0:" + "#,##0" + "}", currency);
    }

    public static int ParseCurrency(string currencyString)
    {
        string currencyWithoutCommas = currencyString.Replace(",", "");
        return int.Parse(currencyWithoutCommas);
    }

    public static string AmountFormat(int currency)
    {
        if (currency >= 1000000)
        {
            return string.Format("{0:#,##0.##}M", currency / 1000000m);
        }
        else if (currency >= 1000)
        {
            return string.Format("{0:#,##0.##}K", currency / 1000m);
        }
        else
        {
            return string.Format("{0:#,##0}", currency);
        }
    }

    public static int ParseAmount(string value)
    {
        if (value.EndsWith("K") || value.EndsWith("k"))
        {
            float numericValue = float.Parse(value.Substring(0, value.Length - 1));
            return (int)(numericValue * 1000);
        }
        else if (value.EndsWith("M") || value.EndsWith("m"))
        {
            float numericValue = float.Parse(value.Substring(0, value.Length - 1));
            return (int)(numericValue * 1000000);
        }
        else
        {
            return int.Parse(value);
        }
    }

    public static int RandomWeighted(int min, int max, float pow)
    {
        float randomValue = Mathf.Pow(Random.value, pow);
        int weightedExpDrop = Mathf.RoundToInt(min + (max - min) * randomValue);
        return weightedExpDrop;
    }

    public static Vector3 RandomAroundPosition(Vector3 basePosition, float radius)
    {
        return new Vector3(
            basePosition.x + Random.Range(-radius, radius),
            0,
            basePosition.z + Random.Range(-radius, radius)
        );
    }
}