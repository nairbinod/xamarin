using System;
using System.Collections.Generic;

namespace MyPayLib
{
	public class CabInfo
	{
		public CabInfo ()
		{
		}

		public string CabNumber { get; set; }
		public string DriverName { get; set; }
		public string PhoneNumber { get; set; }
		public string AlternateNumber { get; set; }
		public string ArrivalTime { get; set; }
		public List<EmployeeInfo> Employees {get;set;}
	}

	public class EmployeeInfo
	{
		public EmployeeInfo ()
		{
		}

		public string EmployeeID { get; set; }
		public string EmployeeName { get; set; }
		public string Process { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public string PickupLocation { get; set; }
		public string PickupTime { get; set; }
		public string Notes {get;set;}

		public CabInfo MyCab {get;set;}
	}

	public class EmployeeProperty
	{
		public EmployeeProperty ()
		{
		}

		public string PropertyName { get; set; }
		public string PropertyValue { get; set; }
		public string PropertyType { get; set; }
	}

}

