using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;

namespace WhiteSoftTest
{
    class Program
    {
        HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            Program program = new Program();
            await program.GetData();
        }

        private async Task GetData()
        {
            string response = await client.GetStringAsync("https://raw.githubusercontent.com/thewhitesoft/student-2022-assignment/main/data.json");
            List<string> data = JsonConvert.DeserializeObject<List<string>>(response);

            string strReplacement = File.ReadAllText(@"replacement.json");

            List<Replacement> replacement = JsonConvert.DeserializeObject<List<Replacement>>(strReplacement);

            //оставляем только последнее изменение для одинаковых строк
            for (int i = 0; i < replacement.Count; i++)
            {
                for (int j = 1; j < replacement.Count; j++)
                {
                    if (replacement[i].replacement == replacement[j].replacement && i != j)
                    {
                        replacement.RemoveAt(i);
                    }
                }
            }

            //если какая-то изменённая строка является частью другой изменённой строки, то обеспечиваем сначала изменение бОльшей строки
            for (int i = 0; i < replacement.Count; i++)
            {
                for (int j = 1; j < replacement.Count; j++)
                {
                    if (replacement[i].replacement.Contains(replacement[j].replacement) && j < i)
                    {
                        Replacement temp = replacement[i];
                        replacement[i] = replacement[j];
                        replacement[j] = temp;
                    }
                }
            }

            //делаем замены
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < replacement.Count; j++)
                {
                    if (data[i].Contains(replacement[j].replacement))
                    {
                        data[i] = data[i].Replace(replacement[j].replacement, replacement[j].source);
                    }
                }
            }

            //удаляем пустые строки
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] == "")
                {
                    data.RemoveAt(i);
                    i--;
                }
            }


            string strRes = JsonConvert.SerializeObject(data, Formatting.Indented);

            
            File.WriteAllText(@"result.JSON", strRes);
            Console.WriteLine("Загрузка в файл прошла успешно!");
        }
    }
}
