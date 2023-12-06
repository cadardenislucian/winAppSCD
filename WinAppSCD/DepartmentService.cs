using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Windows.Forms;

namespace WinAppSCD
{
    internal class DepartmentService
    {
        private static HttpClient client = new HttpClient();

        public void CreateConnection()
        {
            // Set up the connection to the backend server for department-related operations
            client.BaseAddress = new Uri("http://localhost:8080/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public List<Department> GetDepartments()
        {
            List<Department> departments = null;

            // Use a relative URI without a leading slash
            string requestUri = "api/department/getAll";
            HttpResponseMessage response = client.GetAsync(requestUri).Result;

            if (response.IsSuccessStatusCode)
            {
                string resultString = response.Content.ReadAsStringAsync().Result;
                departments = JsonSerializer.Deserialize<List<Department>>(resultString);
                return departments;
            }
            else
            {
                Console.WriteLine("Error retrieving departments: " + response.StatusCode);
            }

            return null;
        }

        public async Task AddDepartment(Department newDepartment)
{
    string requestUri = "api/department/create";
    string departmentJson = JsonSerializer.Serialize(newDepartment);
    HttpContent content = new StringContent(departmentJson, Encoding.UTF8, "application/json");

    HttpResponseMessage response = client.PostAsync(requestUri, content).Result;

    if (response.IsSuccessStatusCode)
    {
        MessageBox.Show("Departament adăugat cu succes.");
    }
    else
    {
        MessageBox.Show("Eroare la adăugarea departamentului: " + response.StatusCode);
    }
    }
        public async Task RenameDepartment(int departmentId, string newDescription, int newParentID, int newManagerID)
        {
            string requestUri = $"api/department/update/{departmentId}";

            // Create an anonymous object with the fields you want to update
            var updateData = new
            {
                description = newDescription,
                parentID = newParentID,
                managerID = newManagerID
            };

            string departmentJson = JsonSerializer.Serialize(updateData);
            Console.WriteLine($"Request JSON: {departmentJson}"); // Log the JSON payload

            HttpContent content = new StringContent(departmentJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(requestUri, content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Departament redenumit cu succes.");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                MessageBox.Show("Departamentul nu a fost găsit. Verificați ID-ul departamentului.");
            }
            else
            {
                MessageBox.Show("Eroare la redenumirea departamentului: " + response.StatusCode);
            }
        }

        public async Task DeleteDepartment(int departmentId)
        {
            string requestUri = $"api/department/delete/{departmentId}";

            HttpResponseMessage response = await client.DeleteAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Departament șters cu succes.");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                MessageBox.Show("Departamentul nu a fost găsit. Verificați ID-ul departamentului.");
            }
            else
            {
                MessageBox.Show("Eroare la ștergerea departamentului: " + response.StatusCode);
            }
        }

    }
}
