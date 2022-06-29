using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OasisInstagram.Classes.Entity
{
    [System.Serializable]
    public class DataPatern
    {
        [JsonProperty("tableName")] public string _tableName;
        [JsonProperty("users")] public List<User> _usersList;
        public DataPatern[] deneme()
        {
            DataPatern dataPatern = new DataPatern();
            dataPatern._tableName = this._tableName;
            dataPatern._usersList = this._usersList;

            DataPatern[] dataPaternList = { dataPatern };
            return dataPaternList;
           /* Array<DataPatern> dataPaternList = new List<DataPatern>();
            dataPaternList.Add(dataPatern);
            return dataPaternList;*/
        }
        
    }
}
