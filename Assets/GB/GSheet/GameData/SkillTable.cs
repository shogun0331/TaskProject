using System.Collections.Generic;
using System;
using Newtonsoft.Json;


[Serializable]
public class SkillTable  : GameData
{	
	 [JsonProperty] public SkillTableProb[] Datas{get; private set;}
	 IReadOnlyDictionary<string, SkillTableProb> _DicDatas;

	public void SetJson(string json)
    {
        var data = JsonConvert.DeserializeObject <SkillTable> (json);
        SkillTableProb[] arr = data.Datas;
        Datas = arr;

		var dic = new Dictionary<string, SkillTableProb>();

        for (int i = 0; i < Datas.Length; ++i)
            dic[Datas[i].ID.ToString()] = Datas[i];

        _DicDatas = dic;

    }

	public bool ContainsColumnKey(string name)
    {
        switch (name)
        {
				case "ID": return true;
				case "SkillType": return true;
				case "Temp_SkillInfo": return true;
				case "SkillName": return true;
				case "SkillInfo": return true;
				case "Chance": return true;
				case "DEFValue": return true;
				case "SlowValue": return true;
				case "Duration": return true;
				case "Delay": return true;
				case "floatValue": return true;
				case "AD_Value": return true;
				case "AP_Value": return true;
				case "FX": return true;

		  default: return false;

        }
    }

	public override double GetNumber(int row, string col)
    {
        return double.Parse(this[row, col].ToString(), System.Globalization.CultureInfo.InvariantCulture);
    }

    public override double GetNumber(string row, string col)
    {
        return double.Parse(this[row, col].ToString(), System.Globalization.CultureInfo.InvariantCulture);
    }


	public object this[int row, string col]
    {
        get
        {
            SkillTableProb data = this[row];
            switch (col)
            {
				case "ID": return data.ID;
				case "SkillType": return data.SkillType;
				case "Temp_SkillInfo": return data.Temp_SkillInfo;
				case "SkillName": return data.SkillName;
				case "SkillInfo": return data.SkillInfo;
				case "Chance": return data.Chance;
				case "DEFValue": return data.DEFValue;
				case "SlowValue": return data.SlowValue;
				case "Duration": return data.Duration;
				case "Delay": return data.Delay;
				case "floatValue": return data.floatValue;
				case "AD_Value": return data.AD_Value;
				case "AP_Value": return data.AP_Value;
				case "FX": return data.FX;


                default: return null;
            }
        }
    }


    public object this[string row, string col]
    {
        get
        {
             SkillTableProb data = this[row];
            switch (col)
            {
				case "ID": return data.ID;
				case "SkillType": return data.SkillType;
				case "Temp_SkillInfo": return data.Temp_SkillInfo;
				case "SkillName": return data.SkillName;
				case "SkillInfo": return data.SkillInfo;
				case "Chance": return data.Chance;
				case "DEFValue": return data.DEFValue;
				case "SlowValue": return data.SlowValue;
				case "Duration": return data.Duration;
				case "Delay": return data.Delay;
				case "floatValue": return data.floatValue;
				case "AD_Value": return data.AD_Value;
				case "AP_Value": return data.AP_Value;
				case "FX": return data.FX;


                default: return null;
            }
        }
    }


	 public object this[int row, int col]
    {
        get
        {
            SkillTableProb data = Datas[row];

            switch (col)
            {
				case 0: return data.ID;
				case 1: return data.SkillType;
				case 2: return data.Temp_SkillInfo;
				case 3: return data.SkillName;
				case 4: return data.SkillInfo;
				case 5: return data.Chance;
				case 6: return data.DEFValue;
				case 7: return data.SlowValue;
				case 8: return data.Duration;
				case 9: return data.Delay;
				case 10: return data.floatValue;
				case 11: return data.AD_Value;
				case 12: return data.AP_Value;
				case 13: return data.FX;

                default: return null;
            }
        }
    }

    public SkillTableProb this[string name]
    {
        get
        {
            return _DicDatas[name];
        }
    }


    public SkillTableProb this[int index]
    {
        get
        {
            return Datas[index];
        }
    }

    public bool ContainsKey(string name)
    {
        return _DicDatas.ContainsKey(name);
    }



    public int Count
    {
        get
        {
            return Datas.Length;
        }
    }
}

[Serializable]
public class SkillTableProb : GameDataProb
{
		[JsonProperty] public readonly string ID;
	[JsonProperty] public readonly string SkillType;
	[JsonProperty] public readonly string Temp_SkillInfo;
	[JsonProperty] public readonly string SkillName;
	[JsonProperty] public readonly string SkillInfo;
	[JsonProperty] public readonly string Chance;
	[JsonProperty] public readonly float DEFValue;
	[JsonProperty] public readonly float SlowValue;
	[JsonProperty] public readonly float Duration;
	[JsonProperty] public readonly float Delay;
	[JsonProperty] public readonly float floatValue;
	[JsonProperty] public readonly float AD_Value;
	[JsonProperty] public readonly float AP_Value;
	[JsonProperty] public readonly string FX;

}
