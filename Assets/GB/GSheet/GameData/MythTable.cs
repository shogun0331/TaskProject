using System.Collections.Generic;
using System;
using Newtonsoft.Json;


[Serializable]
public class MythTable  : GameData
{	
	 [JsonProperty] public MythTableProb[] Datas{get; private set;}
	 IReadOnlyDictionary<string, MythTableProb> _DicDatas;

	public void SetJson(string json)
    {
        var data = JsonConvert.DeserializeObject <MythTable> (json);
        MythTableProb[] arr = data.Datas;
        Datas = arr;

		var dic = new Dictionary<string, MythTableProb>();

        for (int i = 0; i < Datas.Length; ++i)
            dic[Datas[i].ID.ToString()] = Datas[i];

        _DicDatas = dic;

    }

	public bool ContainsColumnKey(string name)
    {
        switch (name)
        {
				case "ID": return true;
				case "A_ID": return true;
				case "B_ID": return true;
				case "C_ID": return true;
				case "D_ID": return true;
				case "Name": return true;

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
            MythTableProb data = this[row];
            switch (col)
            {
				case "ID": return data.ID;
				case "A_ID": return data.A_ID;
				case "B_ID": return data.B_ID;
				case "C_ID": return data.C_ID;
				case "D_ID": return data.D_ID;
				case "Name": return data.Name;


                default: return null;
            }
        }
    }


    public object this[string row, string col]
    {
        get
        {
             MythTableProb data = this[row];
            switch (col)
            {
				case "ID": return data.ID;
				case "A_ID": return data.A_ID;
				case "B_ID": return data.B_ID;
				case "C_ID": return data.C_ID;
				case "D_ID": return data.D_ID;
				case "Name": return data.Name;


                default: return null;
            }
        }
    }


	 public object this[int row, int col]
    {
        get
        {
            MythTableProb data = Datas[row];

            switch (col)
            {
				case 0: return data.ID;
				case 1: return data.A_ID;
				case 2: return data.B_ID;
				case 3: return data.C_ID;
				case 4: return data.D_ID;
				case 5: return data.Name;

                default: return null;
            }
        }
    }

    public MythTableProb this[string name]
    {
        get
        {
            return _DicDatas[name];
        }
    }


    public MythTableProb this[int index]
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
public class MythTableProb : GameDataProb
{
		[JsonProperty] public readonly string ID;
	[JsonProperty] public readonly string A_ID;
	[JsonProperty] public readonly string B_ID;
	[JsonProperty] public readonly string C_ID;
	[JsonProperty] public readonly string D_ID;
	[JsonProperty] public readonly string Name;

}
