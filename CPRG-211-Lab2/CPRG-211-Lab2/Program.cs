using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CPRG_211_Lab2
{
	class Employee
	{
		private string id;
		public string Id
		{
			get { return id; }
			set{id = value;}
		}
		private string name;
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		private string address;
		public string Address
		{
			get { return address; }
			set { address = value; }
		}
		private string phone;
		public string Phone
		{
			get { return phone; }
			set { phone = value; }
		}

		public Employee() { }
		public Employee(string id, string name, string address, string phone)
		{
			this.id = id;
			this.name = name;
			this.address = address;
			this.phone = phone;
		}
	}

	class Salaried : Employee 
	{
		private double salary;
		public double Salary
		{
			get { return salary; }
			set { salary = value; }
		}
		public Salaried() { }
		public Salaried(string id, string name, string address, string phone, double salary)
		{
			Id = id;
			Name = name;
			Address = address;
			Phone = phone;
			this.salary = salary; 
		}
			
	}
	class PartTime : Employee 
	{
		private double rate;
		public double Rate
		{
			get { return rate; } 
		}
		private int hours;
		public int Hours
		{
			get { return hours; }
		}

		public PartTime() { }
		public PartTime(string id, string name, string address, string phone, double rate, int hours)
		{
			Id = id;
			Name = name;
			Address = address;
			Phone = phone;
			this.rate = rate;
			this.hours = hours;
		}
	}
	class Wages : Employee 
	{
		private double rate;
		public double Rate
		{
			get { return rate; }
		}
		private int hours;
		public int Hours
		{
			get { return hours; }
		}
		public Wages() { }
		public Wages(string id, string name, string address, string phone, double rate, int hours)
		{
			Id = id;
			Name = name;
			Address = address;
			Phone = phone;
			this.rate = rate;
			this.hours = hours;
		}
	}

internal class Program
	{
		static void Main(string[] args)
		{
			List<Employee> employeeList = new List<Employee>();
			List<Char> salaryID = new List<Char> { '0', '1', '2', '3', '4' };
			List<Char> wageID = new List<Char> { '5', '6', '7'};
			List<Char> partTimeID = new List<Char> { '8', '9' };

			StreamReader streamReader = new StreamReader("employees.txt");
			string line = streamReader.ReadLine();
			while (line != null) 
			{
				string[] words = line.Split(new char[] {':'});

				if (salaryID.Contains((words[0])[0]))
				{ 
					employeeList.Add(new Salaried(words[0], words[1], words[2], words[3], Convert.ToDouble(words[7])));
				}
				else if (partTimeID.Contains((words[0])[0]))
				{
					employeeList.Add(new PartTime(words[0], words[1], words[2], words[3], Convert.ToDouble(words[7]), Convert.ToInt32(words[8])));
				}
				else if (wageID.Contains((words[0])[0]))
				{
					employeeList.Add(new Wages(words[0], words[1], words[2], words[3], Convert.ToDouble(words[7]), Convert.ToInt32(words[8])));
				}
				else 
				{
					Console.WriteLine("This data isn't valid.");
				}

				line = streamReader.ReadLine();
			}
			streamReader.Close();
			foreach (var employee in employeeList)
			{
				double weeklyPay;
				if (employee.GetType().Name == "Salaried")
				{
					Salaried salaried = (Salaried)employee;
					weeklyPay = salaried.Salary;
				}
				else if (employee.GetType().Name == "PartTime")
				{
					PartTime partTime = (PartTime)employee;
					weeklyPay = partTime.Rate * partTime.Hours;
				}
				else
				{
					Wages wages = (Wages)employee;
					if (wages.Hours > 40)
					{
						weeklyPay = (wages.Rate * 40) + (wages.Rate * 1.5 * (wages.Hours - 40));
					}
					else 
					{ 
						weeklyPay = wages.Rate * wages.Hours; 
					}
				}
				Console.WriteLine($"{employee.Name} makes {String.Format("${0:0.00}", weeklyPay)} per week");
			}

			double highestWage = 0;
			string highestWageName = "Null";

			foreach (var employee in employeeList) 
			{
				if (employee.GetType().Name =="Wages")
				{
					double weeklyPay;
					Wages wages = (Wages)employee;
					if (wages.Hours > 40)
					{
						weeklyPay = (wages.Rate * 40) + (wages.Rate * 1.5 * (wages.Hours - 40));
					}
					else
					{
						weeklyPay = wages.Rate * wages.Hours;
					}

					if (weeklyPay > highestWage)
					{
						highestWage = weeklyPay;
						highestWageName = wages.Name;
					}
				}
			}

			Console.WriteLine($"\nThe wage employee with the highest pay is {highestWageName}, with a weekly pay of {String.Format("${0:0.00}", highestWage)}");

			double lowestSalary = 999999999;
			string lowestSalaryName = "Null";

			foreach (var employee in employeeList) 
			{
				
				if (employee.GetType().Name == "Salaried")
				{
					double weeklyPay;
					Salaried salaried = (Salaried)employee;
					weeklyPay = salaried.Salary;

					if (lowestSalary > weeklyPay)
					{
						lowestSalary = weeklyPay;
						lowestSalaryName = salaried.Name;
					}
				}

			}

			Console.WriteLine($"\nThe salary employee with the lowest pay is {lowestSalaryName}, with a weekly pay of {String.Format("${0:0.00}", lowestSalary)}");

			double salaryNum = 0;
			double wagesNum = 0;
			double partTimeNum = 0;
			double employeeNum = employeeList.Count;

			foreach (var employee in employeeList) 
			{
				if (employee.GetType().Name == "Salaried")
				{
					salaryNum += 1;
				}
				else if (employee.GetType().Name == "Wages")
				{
					wagesNum += 1;
				}
				else if (employee.GetType().Name == "PartTime")
				{
					partTimeNum += 1;
				}
			}

			Console.WriteLine($"\nSalary Employees: {String.Format("{0:0.00}%", salaryNum / employeeNum * 100 )}\nWage Employees: {String.Format("{0:0.00}%", wagesNum / employeeNum * 100)}\nPart Time Employees: {String.Format("{0:0.00}%", partTimeNum / employeeNum * 100)}");

			Console.ReadLine();
		}
	}
}
