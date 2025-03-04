
using System;
using Newtonsoft.Json;

[Serializable]
public class UserData 
{
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}
    