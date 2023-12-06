using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAppSCD
{
    public partial class Form1 : Form
    {
        EmployeeService employeeService;
        List<Employee> employeeList;
        DepartmentService departmentService;
        List<Department> departmentList;
        private object client;

        public Form1()
        {
            InitializeComponent();
            employeeService = new EmployeeService();
            employeeService.createConnection();
            departmentService = new DepartmentService();
            departmentService.CreateConnection();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var employeeList = employeeService.GetEmployees();

            if (employeeList != null)
            {
                // Set the DataSource property of dataGridView1 to display the employee data
                dataGridView1.DataSource = employeeList;
            }
            else
            {
                MessageBox.Show("Eroare la obținerea datelor. Verificați consola pentru detalii.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var departmentList = departmentService.GetDepartments();
            if (departmentList != null)
            {
                dataGridView1.DataSource = departmentList;
            }
            else
            {
                MessageBox.Show("Eroare la obținerea datelor. Verificați consola pentru detalii.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonAddDepartment_Click(object sender, EventArgs e)
        {
            
            string description = textBoxDescription.Text;
            int parentID = int.Parse(textBoxParentID.Text);
            int managerID = int.Parse(textBoxManagerID.Text);

            Department newDepartment = new Department
            { 
                description = description,
                parentID = parentID,
                managerID = managerID
            };

            // Utilizați instanța globală a departmentService și furnizați adresa de bază
            await departmentService.AddDepartment(newDepartment);
        }
        //pentru rename
        private async void button3_Click(object sender, EventArgs e)
        {
            int departmentId = int.Parse(textBoxDepartmentID.Text);
            string newDescription = textBoxDescription.Text;
            int newParentID = int.Parse(textBoxParentID.Text);
            int newManagerID = int.Parse(textBoxManagerID.Text);
            // Utilize the global instance of departmentService and provide the base address
            await departmentService.RenameDepartment(departmentId, newDescription,newParentID,newManagerID);
        }
        //pentru delete
        private async void button4_Click(object sender, EventArgs e)
        {
            int departmentId = int.Parse(textBoxDepartmentID.Text);

            // Utilize the global instance of departmentService and provide the base address
            await departmentService.DeleteDepartment(departmentId);
        }

        private async void buttonAdd_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text;
            long managerID = long.Parse(textBoxManager_ID.Text);
            string email = textBoxEmail.Text;

            Employee newEmployee = new Employee
            {
                name = name,
                managerID = managerID,
                email = email
            };

            await employeeService.AddEmployee(newEmployee);
        }

        private async void buttonUpdate_Click(object sender, EventArgs e)
        {
            int employeeId = int.Parse(textBoxID.Text);
            string newName = textBoxName.Text;
            long newManagerID = long.Parse(textBoxManager_ID.Text);
            string newEmail = textBoxEmail.Text;

            await employeeService.UpdateEmployee(employeeId, newName, newManagerID, newEmail);
        }

        private async void buttonDelete_Click(object sender, EventArgs e)
        {
            int employeeId = int.Parse(textBoxID.Text);

            await employeeService.DeleteEmployee(employeeId);
        }
    }
}
