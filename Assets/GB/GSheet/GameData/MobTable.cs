using System.Collections.Generic;
using System;
using Newtonsoft.Json;


[Serializable]
public class MobTable  : GameData
{	
	 [JsonProperty] public MobTableProb[] Datas{get; private set;}
	 IReadOnlyDictionary<string, MobTableProb> _DicDatas;

	public void SetJson(string json)
    {
        var data = JsonConvert.DeserializeObject <MobTable> (json);
        MobTableProb[] arr = data.Datas;
        Datas = arr;

		var dic = new Dictionary<string, MobTableProb>();

        for (int i = 0; i < Datas.Length; ++i)
            dic[Datas[i].ID.ToString()] = Datas[i];

        _DicDatas = dic;

    }

	public bool ContainsColumnKey(string name)
    {
        switch (name)
        {
				case "ID": return true;
				case "Name": return true;
				case "MobType": return true;
				case "MAX_HP": return true;
				case "M_SPD": return true;
				case "DEF": return true;
				case "Gold": return true;
				case "Luck": return true;

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
            MobTableProb data = this[row];
            switch (col)
            {
				case "ID": return data.ID;
				case "Name": return data.Name;
				case "MobType": return data.MobType;
				case "MAX_HP": return data.MAX_HP;
				case "M_SPD": return data.M_SPD;
				case "DEF": return data.DEF;
				case "Gold": return data.Gold;
				case "Luck": return data.Luck;


                default: return null;
            }
        }
    }


    public object this[string row, string col]
    {
        get
        {
             MobTableProb data = this[row];
            switch (col)
            {
				case "ID": return data.ID;
				case "Name": return data.Name;
				case "MobType": return data.MobType;
				case "MAX_HP": return data.MAX_HP;
				case "M_SPD": return data.M_SPD;
				case "DEF": return data.DEF;
				case "Gold": return data.Gold;
				case "Luck": return data.Luck;


                default: return null;
            }
        }
    }


	 public object this[int row, int col]
    {
        get
        {
            MobTableProb data = Datas[row];

            switch (col)
            {
				case 0: return data.ID;
				case 1: return data.Name;
				case 2: return data.MobType;
				case 3: return data.MAX_HP;
				case 4: return data.M_SPD;
				case 5: return data.DEF;
				case 6: return data.Gold;
				case 7: return data.Luck;

                default: return null;
            }
        }
    }

    public MobTableProb this[string name]
    {
        get
        {
            return _DicDatas[name];
        }
    }


    public MobTableProb this[int index]
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
public class MobTableProb : GameDataProb
{
		[JsonProperty] public readonly string ID;
	[JsonProperty] public readonly string Name;
	[JsonProperty] public readonly string MobType;
	[JsonProperty] public readonly int MAX_HP;
	[JsonProperty] public readonly float M_SPD;
	[JsonProperty] public readonly int DEF;
	[JsonProperty] public readonly int Gold;
	[JsonProperty] public readonly int Luck;

}
