using System.Collections.Generic;
using System;
using Newtonsoft.Json;


[Serializable]
public class UnitTable  : GameData
{	
	 [JsonProperty] public UnitTableProb[] Datas{get; private set;}
	 IReadOnlyDictionary<string, UnitTableProb> _DicDatas;

	public void SetJson(string json)
    {
        var data = JsonConvert.DeserializeObject <UnitTable> (json);
        UnitTableProb[] arr = data.Datas;
        Datas = arr;

		var dic = new Dictionary<string, UnitTableProb>();

        for (int i = 0; i < Datas.Length; ++i)
            dic[Datas[i].ID.ToString()] = Datas[i];

        _DicDatas = dic;

    }

	public bool ContainsColumnKey(string name)
    {
        switch (name)
        {
				case "ID": return true;
				case "UnitID": return true;
				case "Rank": return true;
				case "Level": return true;
				case "ATK": return true;
				case "A_SPD": return true;
				case "A_DIST": return true;
				case "UP_CNT": return true;
				case "UP_GOLD": return true;
				case "Abilitie1": return true;
				case "Abilitie2": return true;
				case "Abilitie3": return true;
				case "Abilitie4": return true;
				case "Weight": return true;
				case "Price": return true;

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
            UnitTableProb data = this[row];
            switch (col)
            {
				case "ID": return data.ID;
				case "UnitID": return data.UnitID;
				case "Rank": return data.Rank;
				case "Level": return data.Level;
				case "ATK": return data.ATK;
				case "A_SPD": return data.A_SPD;
				case "A_DIST": return data.A_DIST;
				case "UP_CNT": return data.UP_CNT;
				case "UP_GOLD": return data.UP_GOLD;
				case "Abilitie1": return data.Abilitie1;
				case "Abilitie2": return data.Abilitie2;
				case "Abilitie3": return data.Abilitie3;
				case "Abilitie4": return data.Abilitie4;
				case "Weight": return data.Weight;
				case "Price": return data.Price;


                default: return null;
            }
        }
    }


    public object this[string row, string col]
    {
        get
        {
             UnitTableProb data = this[row];
            switch (col)
            {
				case "ID": return data.ID;
				case "UnitID": return data.UnitID;
				case "Rank": return data.Rank;
				case "Level": return data.Level;
				case "ATK": return data.ATK;
				case "A_SPD": return data.A_SPD;
				case "A_DIST": return data.A_DIST;
				case "UP_CNT": return data.UP_CNT;
				case "UP_GOLD": return data.UP_GOLD;
				case "Abilitie1": return data.Abilitie1;
				case "Abilitie2": return data.Abilitie2;
				case "Abilitie3": return data.Abilitie3;
				case "Abilitie4": return data.Abilitie4;
				case "Weight": return data.Weight;
				case "Price": return data.Price;


                default: return null;
            }
        }
    }


	 public object this[int row, int col]
    {
        get
        {
            UnitTableProb data = Datas[row];

            switch (col)
            {
				case 0: return data.ID;
				case 1: return data.UnitID;
				case 2: return data.Rank;
				case 3: return data.Level;
				case 4: return data.ATK;
				case 5: return data.A_SPD;
				case 6: return data.A_DIST;
				case 7: return data.UP_CNT;
				case 8: return data.UP_GOLD;
				case 9: return data.Abilitie1;
				case 10: return data.Abilitie2;
				case 11: return data.Abilitie3;
				case 12: return data.Abilitie4;
				case 13: return data.Weight;
				case 14: return data.Price;

                default: return null;
            }
        }
    }

    public UnitTableProb this[string name]
    {
        get
        {
            return _DicDatas[name];
        }
    }


    public UnitTableProb this[int index]
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
public class UnitTableProb : GameDataProb
{
		[JsonProperty] public readonly string ID;
	[JsonProperty] public readonly string UnitID;
	[JsonProperty] public readonly string Rank;
	[JsonProperty] public readonly int Level;
	[JsonProperty] public readonly int ATK;
	[JsonProperty] public readonly float A_SPD;
	[JsonProperty] public readonly float A_DIST;
	[JsonProperty] public readonly int UP_CNT;
	[JsonProperty] public readonly int UP_GOLD;
	[JsonProperty] public readonly string Abilitie1;
	[JsonProperty] public readonly string Abilitie2;
	[JsonProperty] public readonly string Abilitie3;
	[JsonProperty] public readonly string Abilitie4;
	[JsonProperty] public readonly int Weight;
	[JsonProperty] public readonly int Price;

}
