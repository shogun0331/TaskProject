using System.Collections.Generic;
using System;
using Newtonsoft.Json;


[Serializable]
public class SummonTable  : GameData
{	
	 [JsonProperty] public SummonTableProb[] Datas{get; private set;}
	 IReadOnlyDictionary<string, SummonTableProb> _DicDatas;

	public void SetJson(string json)
    {
        var data = JsonConvert.DeserializeObject <SummonTable> (json);
        SummonTableProb[] arr = data.Datas;
        Datas = arr;

		var dic = new Dictionary<string, SummonTableProb>();

        for (int i = 0; i < Datas.Length; ++i)
            dic[Datas[i].ID.ToString()] = Datas[i];

        _DicDatas = dic;

    }

	public bool ContainsColumnKey(string name)
    {
        switch (name)
        {
				case "ID": return true;
				case "C_Weight": return true;
				case "B_Weight": return true;
				case "A_Weight": return true;
				case "S_Weight": return true;

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
            SummonTableProb data = this[row];
            switch (col)
            {
				case "ID": return data.ID;
				case "C_Weight": return data.C_Weight;
				case "B_Weight": return data.B_Weight;
				case "A_Weight": return data.A_Weight;
				case "S_Weight": return data.S_Weight;


                default: return null;
            }
        }
    }


    public object this[string row, string col]
    {
        get
        {
             SummonTableProb data = this[row];
            switch (col)
            {
				case "ID": return data.ID;
				case "C_Weight": return data.C_Weight;
				case "B_Weight": return data.B_Weight;
				case "A_Weight": return data.A_Weight;
				case "S_Weight": return data.S_Weight;


                default: return null;
            }
        }
    }


	 public object this[int row, int col]
    {
        get
        {
            SummonTableProb data = Datas[row];

            switch (col)
            {
				case 0: return data.ID;
				case 1: return data.C_Weight;
				case 2: return data.B_Weight;
				case 3: return data.A_Weight;
				case 4: return data.S_Weight;

                default: return null;
            }
        }
    }

    public SummonTableProb this[string name]
    {
        get
        {
            return _DicDatas[name];
        }
    }


    public SummonTableProb this[int index]
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
public class SummonTableProb : GameDataProb
{
		[JsonProperty] public readonly string ID;
	[JsonProperty] public readonly int C_Weight;
	[JsonProperty] public readonly int B_Weight;
	[JsonProperty] public readonly int A_Weight;
	[JsonProperty] public readonly int S_Weight;

}
