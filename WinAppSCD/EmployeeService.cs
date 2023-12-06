using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAppSCD
{
    internal class EmployeeService
    {
        private static HttpClient client = new HttpClient();

        public void createConnection()
        {
            // Actualizează adresa bazei pentru a reflecta adresa serverului backend.
            client.BaseAddress = new Uri("http://localhost:8080/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public List<Employee> GetEmployees()
        {
            List<Employee> employees = null;
            HttpResponseMessage response = client.GetAsync("api/employee/getAll").Result;
            if (response.IsSuccessStatusCode)
            {
                string resultString = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Lista de angajați: " + resultString);
                employees = JsonSerializer.Deserialize<List<Employee>>(resultString);
                return employees;
            }
            else
            {
                Console.WriteLine("Eroare la solicitarea datelor: " + response.StatusCode);
            }
            return null;
        }

        public async Task AddEmployee(Employee newEmployee)
        {
            string requestUri = "api/employee/create";
            string employeeJson = JsonSerializer.Serialize(newEmployee);
            HttpContent content = new StringContent(employeeJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(requestUri, content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Angajat adăugat cu succes.");
            }
            else
            {
                MessageBox.Show("Eroare la adăugarea angajatului: " + response.StatusCode);
            }
        }

        public async Task UpdateEmployee(int employeeId, string newName, long newManagerID, string newEmail)
        {
            string requestUri = $"api/employee/update/{employeeId}";

            var updateData = new
            {
                name = newName,
                managerID = newManagerID,
                email = newEmail
            };

            string employeeJson = JsonSerializer.Serialize(updateData);
            Console.WriteLine($"Request JSON: {employeeJson}");

            HttpContent content = new StringContent(employeeJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(requestUri, content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Angajat actualizat cu succes.");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                MessageBox.Show("Angajatul nu a fost găsit. Verificați ID-ul angajatului.");
            }
            else
            {
                MessageBox.Show("Eroare la actualizarea angajatului: " + response.StatusCode);
            }
        }

        public async Task DeleteEmployee(int employeeId)
        {
            string requestUri = $"api/employee/delete/{employeeId}";

            HttpResponseMessage response = await client.DeleteAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Angajat șters cu succes.");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                MessageBox.Show("Angajatul nu a fost găsit. Verificați ID-ul angajatului.");
            }
            else
            {
                MessageBox.Show("Eroare la ștergerea angajatului: " + response.StatusCode);
            }
        }

    }
}


