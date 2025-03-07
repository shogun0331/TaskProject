using System.Collections.Generic;
using System;
using Newtonsoft.Json;


[Serializable]
public class UpgradeTable  : GameData
{	
	 [JsonProperty] public UpgradeTableProb[] Datas{get; private set;}
	 IReadOnlyDictionary<string, UpgradeTableProb> _DicDatas;

	public void SetJson(string json)
    {
        var data = JsonConvert.DeserializeObject <UpgradeTable> (json);
        UpgradeTableProb[] arr = data.Datas;
        Datas = arr;

		var dic = new Dictionary<string, UpgradeTableProb>();

        for (int i = 0; i < Datas.Length; ++i)
            dic[Datas[i].ID.ToString()] = Datas[i];

        _DicDatas = dic;

    }

	public bool ContainsColumnKey(string name)
    {
        switch (name)
        {
				case "ID": return true;
				case "UpgradeType": return true;
				case "Level": return true;
				case "PrieceID": return true;
				case "PriceValue": return true;
				case "AD_PER": return true;
				case "AD_SPD_PER": return true;

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
            UpgradeTableProb data = this[row];
            switch (col)
            {
				case "ID": return data.ID;
				case "UpgradeType": return data.UpgradeType;
				case "Level": return data.Level;
				case "PrieceID": return data.PrieceID;
				case "PriceValue": return data.PriceValue;
				case "AD_PER": return data.AD_PER;
				case "AD_SPD_PER": return data.AD_SPD_PER;


                default: return null;
            }
        }
    }


    public object this[string row, string col]
    {
        get
        {
             UpgradeTableProb data = this[row];
            switch (col)
            {
				case "ID": return data.ID;
				case "UpgradeType": return data.UpgradeType;
				case "Level": return data.Level;
				case "PrieceID": return data.PrieceID;
				case "PriceValue": return data.PriceValue;
				case "AD_PER": return data.AD_PER;
				case "AD_SPD_PER": return data.AD_SPD_PER;


                default: return null;
            }
        }
    }


	 public object this[int row, int col]
    {
        get
        {
            UpgradeTableProb data = Datas[row];

            switch (col)
            {
				case 0: return data.ID;
				case 1: return data.UpgradeType;
				case 2: return data.Level;
				case 3: return data.PrieceID;
				case 4: return data.PriceValue;
				case 5: return data.AD_PER;
				case 6: return data.AD_SPD_PER;

                default: return null;
            }
        }
    }

    public UpgradeTableProb this[string name]
    {
        get
        {
            return _DicDatas[name];
        }
    }


    public UpgradeTableProb this[int index]
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
public class UpgradeTableProb : GameDataProb
{
		[JsonProperty] public readonly string ID;
	[JsonProperty] public readonly string UpgradeType;
	[JsonProperty] public readonly string Level;
	[JsonProperty] public readonly string PrieceID;
	[JsonProperty] public readonly int PriceValue;
	[JsonProperty] public readonly float AD_PER;
	[JsonProperty] public readonly float AD_SPD_PER;

}
