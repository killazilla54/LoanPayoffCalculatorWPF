using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanPaymentCalculator.ViewModels
{
    /// <summary>
    ///  View Model for a Loan entry row
    /// </summary>
    public class Loan 
    {
        public double principal { get; set; }
        public double interest { get; set; }
        public double rate { get; set; }
        public int id { get; set; }

        public Loan(int id)
        {
            principal = 0;
            interest = 0;
            rate = 0.00;
            this.id = id;
        }

        public override string ToString()
        {
            return "Principal: " + principal + " - Interest: " + interest + " - Rate: " + rate;
        }
    }
}
