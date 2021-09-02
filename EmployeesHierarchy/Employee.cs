using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmployeesHierarchy
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public int ManagerId { get; set; }
        public int EmployeeSalary { get; set; }
        public List<Employee> Employees { get; set; }

        public List<Employee> AddEmployee(string EmployeeId, string ManagerId, string EmployeeSalary, List<Employee> AddedEployees)
        {
            int Salary = 0;
            int Manager = 0;

            Salary=ValidateSalary(EmployeeSalary);

            if (!int.TryParse(ManagerId, out Manager))
            {
                Manager = 0;
            }

            this.EmployeeId = int.Parse(EmployeeId);
            this.ManagerId = Manager;
            this.EmployeeSalary = Salary;

            Employees.Add(new Employee { EmployeeId=this.EmployeeId,ManagerId=this.ManagerId,EmployeeSalary=this.EmployeeSalary});

            return AddedEployees;
        }

        public int ValidateSalary(string Salary)
        {
            var e = this.Employees;
            int EmployeeSalary = 0;
            if (!int.TryParse(Salary, out EmployeeSalary))
            {
                throw new EmployeeException("Invalid Employee Salary");
            }

            if (EmployeeSalary <= 0)
            {
                throw new EmployeeException("Invalid Employee Salary");
            }

            return EmployeeSalary;
        }

        public bool ValidateNumberOfCeos(List<Employee> employees)
        {
            var e = this.Employees;
            if (employees.Where(e =>e.ManagerId == 0).ToList().Count > 1)
            {
                throw new EmployeeException("Number of CEOs is greater than one!");
            }
            else
            {
                return true;
            }
        }

        public bool ValidateNumberOfEmployeeManagers(List<Employee> employees)
        {
            var e = this.Employees;
            int Count = 0;
            foreach(int EmployeeId in employees.Select(e => e.EmployeeId).Distinct())
            {
                List<int> Managers=employees.Where(e=>e.EmployeeId==EmployeeId).Select(e=>e.ManagerId).ToList();
                if(Managers.Count > 1)
                {
                    throw new EmployeeException("Employee With ID ("+EmployeeId+") has more than one manager!");
                }
            }

            return true;
        }

        public bool ValidateManagersAreEmployees(List<Employee> employees)
        {
            var e = this.Employees;
            int Count = 0;
            foreach(int ManagerId in employees.Where(e => e.ManagerId != 0).Select(e => e.ManagerId).Distinct())
            {
                List<int> Managers=employees.Where(e=>e.EmployeeId== ManagerId).
                    Select(e=>e.ManagerId).ToList();
                if(Managers == null)
                {
                    throw new EmployeeException("Manager With ID ("+ ManagerId + ") is not an employee!");
                }
                if(Managers.Count == 0)
                {
                    throw new EmployeeException("Manager With ID ("+ ManagerId + ") is not an employee!");
                }
            }

            return true;
        }

        public bool ValidateCircularReferencing(List<Employee> employees)
        {
            var e = this.Employees;
            int Count = 0;
            foreach(Employee employee in employees)
            {
                Employee manager=employees.Where(e =>
                e.ManagerId !=0 
                & e.EmployeeId==employee.ManagerId
                & e.ManagerId==employee.EmployeeId
                
                ).FirstOrDefault();

                if (manager != null)
                    throw new EmployeeException("Circular Refering Occured");
            }

            return true;
        }

        public Employee(List<Employee> Employees)
        {
            this.Employees = Employees;
        }

        public Employee()
        {

        }

        public Employee(string EmployeeCsvString)
        {
            Employees=new List<Employee>();
            string[] EmployeeCsv = EmployeeCsvString.Split('\n');
            Employee employee=new Employee(Employees);
            foreach (string EmployeeData in EmployeeCsv)
            {
                if (EmployeeData.Equals("")) break;
                string[] Employee=EmployeeData.Split(',');

                employee.Employees = AddEmployee(Employee[0], Employee[1], Employee[2], employee.Employees);
                employee = new Employee(Employees);
            }

            ValidateNumberOfCeos(employee.Employees);

            ValidateNumberOfEmployeeManagers(employee.Employees);

            ValidateManagersAreEmployees(employee.Employees);

            ValidateCircularReferencing(employee.Employees);
        }

        public long SalaryBudget(string ManagerId)
        {
            long TotalSalary = 0;
            TotalSalary+= this.Employees.Where(e => e.EmployeeId == int.Parse(ManagerId)).FirstOrDefault().EmployeeSalary;
            foreach (Employee Employee in Employees.Where(e=>e.ManagerId == int.Parse(ManagerId)).ToList())
            {
                List<Employee> SubEmployees = Employees.Where(e => e.ManagerId == int.Parse(Employee.EmployeeId.ToString())).ToList();
                if (SubEmployees != null)
                {
                    if(SubEmployees.Count() > 0)
                        TotalSalary += SalaryBudget(Employee.EmployeeId.ToString());
                    else
                        TotalSalary += Employee.EmployeeSalary;
                }
                else
                    TotalSalary += Employee.EmployeeSalary;
            }

            return TotalSalary;
        }
    }

}
