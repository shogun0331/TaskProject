using System.Collections.Generic;
using System;
using Newtonsoft.Json;


[Serializable]
public class NomalWaveTable  : GameData
{	
	 [JsonProperty] public NomalWaveTableProb[] Datas{get; private set;}
	 IReadOnlyDictionary<string, NomalWaveTableProb> _DicDatas;

	public void SetJson(string json)
    {
        var data = JsonConvert.DeserializeObject <NomalWaveTable> (json);
        NomalWaveTableProb[] arr = data.Datas;
        Datas = arr;

		var dic = new Dictionary<string, NomalWaveTableProb>();

        for (int i = 0; i < Datas.Length; ++i)
            dic[Datas[i].ID.ToString()] = Datas[i];

        _DicDatas = dic;

    }

	public bool ContainsColumnKey(string name)
    {
        switch (name)
        {
				case "ID": return true;
				case "MobID": return true;
				case "CreateCount": return true;
				case "Dealay": return true;
				case "BossID": return true;

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
            NomalWaveTableProb data = this[row];
            switch (col)
            {
				case "ID": return data.ID;
				case "MobID": return data.MobID;
				case "CreateCount": return data.CreateCount;
				case "Dealay": return data.Dealay;
				case "BossID": return data.BossID;


                default: return null;
            }
        }
    }


    public object this[string row, string col]
    {
        get
        {
             NomalWaveTableProb data = this[row];
            switch (col)
            {
				case "ID": return data.ID;
				case "MobID": return data.MobID;
				case "CreateCount": return data.CreateCount;
				case "Dealay": return data.Dealay;
				case "BossID": return data.BossID;


                default: return null;
            }
        }
    }


	 public object this[int row, int col]
    {
        get
        {
            NomalWaveTableProb data = Datas[row];

            switch (col)
            {
				case 0: return data.ID;
				case 1: return data.MobID;
				case 2: return data.CreateCount;
				case 3: return data.Dealay;
				case 4: return data.BossID;

                default: return null;
            }
        }
    }

    public NomalWaveTableProb this[string name]
    {
        get
        {
            return _DicDatas[name];
        }
    }


    public NomalWaveTableProb this[int index]
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
public class NomalWaveTableProb : GameDataProb
{
		[JsonProperty] public readonly string ID;
	[JsonProperty] public readonly string MobID;
	[JsonProperty] public readonly int CreateCount;
	[JsonProperty] public readonly float Dealay;
	[JsonProperty] public readonly string BossID;

}
