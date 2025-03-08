using System.Collections.Generic;
using System;
using Newtonsoft.Json;


[Serializable]
public class GachaTable  : GameData
{	
	 [JsonProperty] public GachaTableProb[] Datas{get; private set;}
	 IReadOnlyDictionary<string, GachaTableProb> _DicDatas;

	public void SetJson(string json)
    {
        var data = JsonConvert.DeserializeObject <GachaTable> (json);
        GachaTableProb[] arr = data.Datas;
        Datas = arr;

		var dic = new Dictionary<string, GachaTableProb>();

        for (int i = 0; i < Datas.Length; ++i)
            dic[Datas[i].ID.ToString()] = Datas[i];

        _DicDatas = dic;

    }

	public bool ContainsColumnKey(string name)
    {
        switch (name)
        {
				case "ID": return true;
				case "Weight": return true;
				case "PriceID": return true;
				case "PriceValue": return true;

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
            GachaTableProb data = this[row];
            switch (col)
            {
				case "ID": return data.ID;
				case "Weight": return data.Weight;
				case "PriceID": return data.PriceID;
				case "PriceValue": return data.PriceValue;


                default: return null;
            }
        }
    }


    public object this[string row, string col]
    {
        get
        {
             GachaTableProb data = this[row];
            switch (col)
            {
				case "ID": return data.ID;
				case "Weight": return data.Weight;
				case "PriceID": return data.PriceID;
				case "PriceValue": return data.PriceValue;


                default: return null;
            }
        }
    }


	 public object this[int row, int col]
    {
        get
        {
            GachaTableProb data = Datas[row];

            switch (col)
            {
				case 0: return data.ID;
				case 1: return data.Weight;
				case 2: return data.PriceID;
				case 3: return data.PriceValue;

                default: return null;
            }
        }
    }

    public GachaTableProb this[string name]
    {
        get
        {
            return _DicDatas[name];
        }
    }


    public GachaTableProb this[int index]
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
public class GachaTableProb : GameDataProb
{
		[JsonProperty] public readonly string ID;
	[JsonProperty] public readonly int Weight;
	[JsonProperty] public readonly string PriceID;
	[JsonProperty] public readonly int PriceValue;

}
