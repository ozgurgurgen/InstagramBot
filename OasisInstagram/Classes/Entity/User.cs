using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OasisInstagram.Classes.Entity
{
    [System.Serializable]
    public class User
    {
        [JsonProperty("userName")] public string _userName;
        [JsonProperty("followDate")] public string _followDate;

    }
}
