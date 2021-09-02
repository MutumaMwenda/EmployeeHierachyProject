using EmployeesHierarchy;
using System;
using System.Collections.Generic;
using Xunit;

namespace EmployeesHierarchyTest
{
    public class EmployeeTest
    {
        [Theory]
        [InlineData("-1")]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("100")]
        public void TestEmployeeSalary(string Value)
        {
            Employee employee=new Employee();
            int result =employee.ValidateSalary(Value);

            Assert.True(result != 0, Value+" should be an integer.");
        }

        [Theory]
        [InlineData("1,,1000\n2, 1, 800\n3, 1, 500\n4, 2, 500\n6, 2, 500\n5, 1, 500\n11, 2, 300\n12, 11, 500")]
        [InlineData("1,,1000\n2, 1, 800\n3, 1, 500\n4, 2, 500\n6, 2, 500\n5, 1, 500\n11,, 300")]
        [InlineData("1,,10002, 1, 800\n3, 1, 500\n4, 2, 500\n6, 2, 500\n5, 1, 500\n10, 11, 400")]
        [InlineData("1,,10002, 1, 800\n3, 1, 500\n4, 2, 500\n6, 2, 500\n5, 1, 500\n6, 5, 300")]
        [InlineData("1,,1000\n2, 1, 800\n3, 1, 500\n4, 2, 500\n6, 2, 500\n5, 1, 500\n11, 12, 300\n12, 11, 500")]
        public void TestValidation(string EmployeeCSV)
        {
            Action testCode = () => { new Employee(EmployeeCSV); };

            var ex = Record.Exception(testCode);

            Assert.NotNull(ex);
            Assert.IsType<EmployeeException>(ex);
        }

        [Theory]
        [InlineData("1,,1000\n2, 1, 800\n3, 1, 500\n4, 2, 500\n6, 2, 500\n5, 1, 500\n11, 2, 300\n12, 11, 500")]
        [InlineData("1,,1000\n2, 1, 800\n3, 1, 500\n4, 2, 500\n6, 2, 500\n5, 1, 500\n11,, 300")]
        public void TestNumberOfCEOs(string EmployeeCSV)
        {
            Employee employee=new Employee(EmployeeCSV);
            bool result = employee.ValidateNumberOfCeos(employee.Employees);

            Assert.True(result, "CEOs can be only one.");
        }

        [Theory]
        [InlineData("1,,1000\n2, 1, 800\n3, 1, 500\n4, 2, 500\n6, 2, 500\n5, 1, 500\n11, 2, 300\n12, 11, 500")]
        [InlineData("1,,10002, 1, 800\n3, 1, 500\n4, 2, 500\n6, 2, 500\n5, 1, 500\n10, 11, 400")]
        public void TestManagersAreEmployees(string EmployeeCSV)
        {
            Employee employee=new Employee(EmployeeCSV);
            bool result = employee.ValidateManagersAreEmployees(employee.Employees);

            Assert.True(result, "Managers must be employees.");
        }

        [Theory]
        [InlineData("1,,1000\n2, 1, 800\n3, 1, 500\n4, 2, 500\n6, 2, 500\n5, 1, 500\n11, 2, 300\n12, 11, 500")]
        [InlineData("1,,10002, 1, 800\n3, 1, 500\n4, 2, 500\n6, 2, 500\n5, 1, 500\n6, 5, 300")]
        public void TestNumberOfEmployeeManageer(string EmployeeCSV)
        {
            Employee employee=new Employee(EmployeeCSV);
            bool result = employee.ValidateManagersAreEmployees(employee.Employees);

            Assert.True(result, "Employee can have only one manager.");
        }


        [Theory]
        [InlineData("1,,1000\n2, 1, 800\n3, 1, 500\n4, 2, 500\n6, 2, 500\n5, 1, 500\n11, 12, 300\n12, 11, 500")]
        public void TestCircularReferencingException(string EmployeeCSV)
        {
            Action testCode = () => { new Employee(EmployeeCSV); };

            var ex = Record.Exception(testCode);

            Assert.NotNull(ex);
            Assert.IsType<EmployeeException>(ex);
        }

        [Theory]
        [InlineData("1", "1,,1000\n2, 1, 800\n3, 1, 500\n4, 2, 500\n6, 2, 500\n5, 1, 500\n11, 2, 300\n12, 11, 500")]
        [InlineData("2", "1,,1000\n2, 1, 800\n3, 1, 500\n4, 2, 500\n6, 2, 500\n5, 1, 500\n11, 2, 300\n12, 11, 500")]
        [InlineData("3", "1,,1000\n2, 1, 800\n3, 1, 500\n4, 2, 500\n6, 2, 500\n5, 1, 500\n11, 2, 300\n12, 11, 500")]
        public void TestSalaryBudget(string ManagerId, string EmployeeCSV)
        {
            Employee employee = new Employee(EmployeeCSV);
            var result = employee.SalaryBudget(ManagerId);

            Assert.NotNull(result);
        }
    }
}
