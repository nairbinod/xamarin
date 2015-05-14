using System;
using System.Linq;
using System.Collections.Generic;

namespace MyPayLib
{
    public static class HRA
    {
        #region HRA Calculation

		public static double EligibleHRA(double ActualHRAReceived, double RentPaidPerMonth, double IncomeBasic)
        {
			double rentPaid = RentPaidPerMonth - IncomeBasic/12 * 0.1;

			List<double> list = new List<double>{ ActualHRAReceived, rentPaid, IncomeBasic/12 };

			double minValue = list.Where(i=> i > 0).Min();

            return minValue * 12;
        }

        #endregion

    }
}
