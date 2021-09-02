using System;
using System.IO;
using System.Reflection;

namespace EmployeesHierarchy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string path = @"C:\Users\Ian.Mutuma\Desktop\TBL\EmployeesHierarchy-main\EmployeesHierarchy\EmployeeInfo.csv";
            string EmployeeData = File.ReadAllText(path);

            new Employee(EmployeeData);

            Console.ReadLine();
        }
    }
}
