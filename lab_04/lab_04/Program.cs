using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace lab_04
{
    class Currency
    {
        public string Code { get; set; }
        public double Rate { get; set; }
    }
    class Vacancy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Salary { get; set; }
        public List<string> KeySkills = new List<string>();
    }
    class Program
    {
        static void Main(string[] args)
        {
            PrintVacancies();
            Console.ReadLine();
        }


        static void PrintVacancy(Vacancy vacancy)
        {
            Console.WriteLine("\tНазвание: " + vacancy.Name);
            Console.WriteLine("\t\t\tЗарплата: " + vacancy.Salary);
            SetKeySkills(vacancy);
            Console.WriteLine("\t\t\tКлючевые навыки: ");
            for (int i = 0; i < vacancy.KeySkills.Count; i++)
            {
                Console.WriteLine("\t\t\t\t" + i + ": " + vacancy.KeySkills[i]);
            }

        }
        static void PrintVacancies()
        {
            List<Vacancy> vacancies = GetVacancies();


            //1.	Названия профессий в вакансиях, объявленная зарплата которых привышает либо равна 120000 руб.
            //2.	Названия ключевых навыков в вакансиях, объявленная зарплата которых привышает либо равна 120000 руб.
            Console.WriteLine("Зарплата >= 120000 рублей");

            foreach (var vacancy in vacancies)
            {
                if (vacancy.Salary >= 120000)
                {
                    PrintVacancy(vacancy);
                }
            }
            Console.WriteLine();

            //3.	Названия профессий в вакансиях, объявленная зарплата которых менее 15000 руб.
            //4.	Названия ключевых навыков в вакансиях, объявленная зарплата которых менее 15000 руб.
            Console.WriteLine("Зарплата < 15000 рублей");
            foreach (var vacancy in vacancies)
            {
                if (vacancy.Salary < 15000)
                {
                    PrintVacancy(vacancy);
                }
            }
        }

        static void SetKeySkills(Vacancy vacancy)
        {
            string vacResponse = SendRequest("https://api.hh.ru/", "vacancies/" + vacancy.Id).Result;

            dynamic vacResults = JsonConvert.DeserializeObject<dynamic>(vacResponse);
            if (vacResults.key_skills != null)
            {
                foreach (var key_skill in vacResults.key_skills)
                {
                    vacancy.KeySkills.Add((string)key_skill.name);
                }
            }
        }

        static async Task<string> SendRequest(string address, string requestUri)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(address);
            client.Timeout = TimeSpan.FromSeconds(10);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", "api-test-agent");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string responseForm = "";

            if (response.IsSuccessStatusCode)
            {
                responseForm = response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                Console.WriteLine("Произошла ошибка!");
            }
            
            return responseForm;
        }

        static List<Vacancy> GetVacancies()
        {
            Console.WriteLine("Сбор данных...");
            List<Currency> currencies = new List<Currency>();

            string currentResponse = SendRequest("https://api.hh.ru/", "dictionaries").Result;
            dynamic currentResults = JsonConvert.DeserializeObject<dynamic>(currentResponse);

            if (currentResults.currency != null)
            {
                for (int i = 0; i <= (currentResults.currency).Count-1; i++) {
                    Currency currency = new Currency();
                    if (currentResults.currency[i].code != null) currency.Code = (string)currentResults.currency[i].code;
                    if (currentResults.currency[i].rate != null) currency.Rate = (double)currentResults.currency[i].rate;
                    currencies.Add(currency);
                }

            }

            List<Vacancy> vacancies = new List<Vacancy>();

            Console.WriteLine("Получение вакансий...");
            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine("Запрос 100 элементов с " + i + " страницы");
                string responseForm = SendRequest("https://api.hh.ru/", "vacancies?per_page=100&page=" + i).Result;

                dynamic results = JsonConvert.DeserializeObject<dynamic>(responseForm);
                if (results.items != null)
                {
                    foreach (var item in results.items)
                    {
                        if (item.salary != null)
                        {
                            Vacancy vacancy = new Vacancy();
                            if (item.id != null) vacancy.Id = (int)item.id;
                            if (item.name != null) vacancy.Name = (string)item.name;

                            if (item.salary.from != null && item.salary.to != null)   vacancy.Salary = ((int)item.salary.from + (int)item.salary.to) / 2;
                            else if (item.salary.from != null)  vacancy.Salary = (int)item.salary.from;
                            else if (item.salary.to != null)    vacancy.Salary = (int)item.salary.to;
                      
                            if (item.salary.currency != null)
                                vacancy.Salary = (int)(vacancy.Salary * currencies.Find(c => c.Code.Equals((string)item.salary.currency)).Rate);
                            vacancies.Add(vacancy);
                        }
                    }
                }
            }

            return vacancies;
        }
    }
}