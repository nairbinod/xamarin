using System;
using System.Collections.Generic;
using System.Linq;

namespace MyPayLib
{
	public enum DeductionType
	{
		Flex = 1,
		Expense = 2,
		Investment = 3
	}

	public class MyIncome
	{
		public MyIncome()
		{
		}

		public double CTC { get; set; }

		//private double _basicPct = .4;

		public double BasicPercentage { get; set; }

		public double Basic
		{
			get
			{
				return this.CTC * this.BasicPercentage;
			}
		}

		private double _pfPct = .12;

		public double PfPercentage
		{
			get
			{
				return _pfPct;
			}
			set
			{
				_pfPct = value;
			}
		}

		public double PfAmount
		{
			get
			{
				return this.Basic * this.PfPercentage;
			}
		}

		public double EmployerPfAmount
		{
			get
			{
				return this.Basic * this.PfPercentage;
			}
		}

		private double _vrPct = .12;

		public double VariablePercentage
		{

			get
			{
				return _vrPct;
			}
			set
			{
				_vrPct = value;
			}
		}

		public double VariableAmount
		{
			get
			{
				return this.CTC * this.VariablePercentage;
			}
		}

		public double ShiftAllowance { get; set; }
	}

	public class MyDeduction
	{
		public MyDeduction()
		{
		}

		public string DeductionName { get; set; }

		public double DeductionAmount { get; set; }

		public DeductionType DeductionType { get; set; }
		public bool IsCalcField { get; set; }
	}

	public class PayCheck
	{
		public PayCheck()
		{
			Income = new MyIncome();
			Deductions = new List<MyDeduction>();
			Init();
			Calculate();
		}

		public MyIncome Income { get; set; }

		public List<MyDeduction> Deductions { get; set; }

		public double TotalIncomeValue { get; set; }

		public double TotalDeductionsValue { get; set; }

		private bool _IsMetroResident;
		public bool IsMetroResident
		{
			get { return _IsMetroResident; }
			set
			{
				if (value == true)
					Income.BasicPercentage = .5;
				else
					Income.BasicPercentage = .4;
				_IsMetroResident = value;
			}
		}

		void Init()
		{
			this.Income.CTC = 1000000;
			this.Deductions.Add(new MyDeduction
			{
				DeductionAmount = 150000,
				DeductionName = "80C",
				DeductionType = DeductionType.Investment
			});
			this.Deductions.Add(new MyDeduction
			{
				DeductionAmount = 0,
				DeductionName = "NPS",
				DeductionType = DeductionType.Investment
			});
			this.Deductions.Add(new MyDeduction
			{
				DeductionAmount = 0,
				DeductionName = "80CG",
				DeductionType = DeductionType.Investment
			});
			this.Deductions.Add(new MyDeduction
			{
				DeductionAmount = 0,
				DeductionName = "Home Loan Interest",
				DeductionType = DeductionType.Investment
			});
			this.Deductions.Add(new MyDeduction
			{
				DeductionAmount = 15000,
				DeductionName = "HRA",
				DeductionType = DeductionType.Expense,
				IsCalcField = true
			});
			this.Deductions.Add(new MyDeduction
				{
					DeductionAmount = 0,
					DeductionName = "Rent Paid",
					DeductionType = DeductionType.Expense,
					IsCalcField = true
				});
			
			this.Deductions.Add(new MyDeduction
			{
				DeductionAmount = 1600,
				DeductionName = "Conveyance Allowance",
				DeductionType = DeductionType.Expense
			});
			this.Deductions.Add(new MyDeduction
			{
				DeductionAmount = 1250,
				DeductionName = "Medical Reimbursement",
				DeductionType = DeductionType.Flex
			});
			this.Deductions.Add(new MyDeduction
			{
				DeductionAmount = 0,
				DeductionName = "LTA",
				DeductionType = DeductionType.Flex
			});
			this.Deductions.Add(new MyDeduction
			{
				DeductionAmount = 1250,
				DeductionName = "Professional Development",
				DeductionType = DeductionType.Flex
			});
			this.Deductions.Add(new MyDeduction
			{
				DeductionAmount = 1100,
				DeductionName = "Food Coupons",
				DeductionType = DeductionType.Flex
			});

			this.Deductions.Add(new MyDeduction
			{
				DeductionAmount = 200,
				DeductionName = "Professional Tax",
				DeductionType = DeductionType.Flex
			});

			this.Deductions.Add(new MyDeduction
			{
				DeductionAmount = 0,
				DeductionName = "Medical Insurance",
				DeductionType = DeductionType.Investment
			});
			//PayCheck.Deductions.Add(new MyDeduction { DeductionAmount = 0, DeductionName = "Housing Loan", DeductionType = false });
			this.Deductions.Add(new MyDeduction
			{
				DeductionAmount = 0,
				DeductionName = "Other(Misc)",
				DeductionType = DeductionType.Flex
			});

			this.Deductions.Add(new MyDeduction
			{
				DeductionAmount = 0,
				DeductionName = "Car Transport Allowance",
				DeductionType = DeductionType.Flex
			});
		}

		public void Calculate()
		{
			TotalIncome();
			TotalDeductions();
		}

		private void TotalIncome()
		{
			TotalIncomeValue = Income.CTC - Income.EmployerPfAmount;// Income.Basic + Income.CTC + Income.PfAmount + Income.VariableAmount + Income.ShiftAllowance;
		}

		private void TotalDeductions()
		{
			TotalDeductionsValue = 0;
			foreach (MyDeduction ded in Deductions)
			{
				if (ded.DeductionType == DeductionType.Flex || ded.DeductionType == DeductionType.Expense) {
					if (!ded.IsCalcField)
						TotalDeductionsValue += ded.DeductionAmount * 12;
				}
				else
					TotalDeductionsValue += ded.DeductionAmount;
			}

			MyDeduction hra = (MyDeduction)Deductions.Where (d => d.DeductionName == "HRA").FirstOrDefault ();
			MyDeduction rent = (MyDeduction)Deductions.Where (d => d.DeductionName == "Rent Paid").FirstOrDefault ();
			TotalDeductionsValue += HRA.EligibleHRA (hra.DeductionAmount, rent.DeductionAmount, Income.Basic);
		}

		public double TaxableIncomeValue
		{
			get { return TotalIncomeValue - TotalDeductionsValue + Income.ShiftAllowance; }
		}

		public double TaxAmountValue
		{
			get
			{
				const double govtExemption = 250000;

				const double Tax10percentage = 500000;
				const double Tax20percentage = 1000000;
				//const double Tax30percentage = 2000000;
				double taxAmount = 0;

				//=ROUND(IF(AND(C21>B23), ((C21-B23)*20/100),0),0)

				double tax10Value = TaxableIncomeValue < govtExemption ? 0 :
							(TaxableIncomeValue > Tax10percentage ? govtExemption * 0.1 : (TaxableIncomeValue - govtExemption) * 0.1);

				//6 Lakhs
				//12 Lkahs
				double tax20value = TaxableIncomeValue < Tax10percentage ? 0 :
					(TaxableIncomeValue < Tax20percentage ? (TaxableIncomeValue - Tax10percentage) * .2 : Tax10percentage * 0.2);


				double tax30value = TaxableIncomeValue > Tax20percentage ? (TaxableIncomeValue - Tax20percentage) * 0.3 : 0;

				//=ROUND(IF(AND(C21>B24,C21<=B25), ((C21-B24)*30/100),0),0)

				//Console.WriteLine(string.Format("10% value: {0}", tax20value));
				//Console.WriteLine(string.Format("20% value: {0}", tax30value));


				taxAmount = tax10Value + tax20value + tax30value;

				return taxAmount;
			}

			//=(B1-C27-E2-C18-C13-C11-C12)/12 + F10/12
		}

		public double TakeHomeValue
		{
			get
			{

				double _deductions = 0;
				foreach (MyDeduction ded in Deductions)
				{
					if (ded.DeductionType == DeductionType.Flex)
						_deductions += ded.DeductionAmount * 12;
				}                

				return (Income.CTC - Income.PfAmount - Income.EmployerPfAmount - TaxAmountValue - _deductions) / 12 + Income.ShiftAllowance / 12;
			}
		}
	}
}

