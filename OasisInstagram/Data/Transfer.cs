using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OasisInstagram.Data
{
    public class Transfer
    {
        public void ToFile(string strings)
        {
            var path = Environment.CurrentDirectory + @"/console.data";
            if (File.Exists(path))
            {
                var datas = File.ReadAllLines(path, Encoding.UTF8).ToList<string>();
                
                datas.Add(strings +"  |  Zaman => "+ DateTime.Now.ToString());
                File.WriteAllLines(path, datas);
            }
            else
            {
                File.WriteAllText(path, strings);
            }
        }
        public List<string> ToConsole()
        {
            var path = Environment.CurrentDirectory + @"/console.data";
            if (File.Exists(path))
            {   var datas = File.ReadAllLines(path, Encoding.UTF8).ToList<string>();
                if (datas.Count == 0)
                {                    
                    return new List<string>() { "İşlem Başlatılıyor..." };
                }
                else
                {
                    return File.ReadAllLines(path, Encoding.UTF8).ToList<string>();
                }
            }
            else
            {
                return new List<string>() { "Yeni Konsol Dosyası Oluşturuluyor..." };
            }
        }
    }
}
