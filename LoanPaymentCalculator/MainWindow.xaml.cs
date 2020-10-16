using LoanPaymentCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;


namespace LoanPaymentCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Loan> Loans { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.Loans = new ObservableCollection<Loan>();
            //Init 1st row of loan fields
            this.Loans.Add(new Loan(1));
            icLoansList.ItemsSource = this.Loans;
        }

        /*
         * Adds a new empty row of loan input fields
         */
        private void AddRowBtn_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Add Button Clicked");
            this.Loans.Add(new Loan(Loans.Count+1));
        }

        private void CalcBtn_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(" Calc Button Clicked");
            ResultsText.Content = "";
            Dictionary<int, double> totalLoans = new Dictionary<int, double>();

            List<Loan> payoffLoans = new List<Loan>();
            foreach(Loan l in Loans)
            {
                Loan tempLoan = new Loan(l.id);
                tempLoan.principal = l.principal;
                tempLoan.interest = l.interest;
                tempLoan.rate = l.rate;
                payoffLoans.Add(tempLoan);
            }

            double paymentAmt = 0;
            try
            {
                paymentAmt = Convert.ToDouble(PaymentAmt.Text);
            } catch (FormatException ex)
            {
                ResultsText.Content = "Payoff Amount missing";
                return;
            }
            bool paidOff = false;
            int months = 0;
            while(paidOff == false)
            {
                
                PayMonth(paymentAmt, ref payoffLoans);
                months++;
                for(int i = 0; i < payoffLoans.Count; i++)
                {
                    Debug.WriteLine("Loan " + i + " principal: " + payoffLoans[i].principal);
                    if(payoffLoans[i].principal == 0)
                    {
                        payoffLoans.RemoveAt(i);
                    }
                }
                if(payoffLoans.Count == 0)
                {
                    paidOff = true;
                    ResultsText.Content = "You will payoff your loans in " + months / 12 + " years and " + months % 12 + " months.";
                }
            }
        }

        private void PayMonth(double payment, ref List<Loan> currentLoans)
        {
            
            for (int i = 0; i < currentLoans.Count; i++)
            {
                //TODO do math to make sure payment - interest is not negative.  if negative, only apply what can be applied.
                if(payment > currentLoans[i].interest)
                {
                    payment -= currentLoans[i].interest;
                    currentLoans[i].interest = 0;
                } else
                {
                    //Apply whats left
                    currentLoans[i].interest -= payment;
                    payment = 0;

                    break;
                }
                
            }
            //divide remaining payment evenly across principals
            double splitPaymentEvenly = payment / currentLoans.Count;
            for (int i = 0; i < currentLoans.Count; i++)
            {
                currentLoans[i].principal -= splitPaymentEvenly;
                if(currentLoans[i].principal <= 0)
                {
                    currentLoans[i].principal = 0;
                    continue; // don't calculate interest, loan is done.
                }
                currentLoans[i].interest = currentLoans[i].principal * (currentLoans[i].rate / 1200);
            }
        }

        /*
         * Resets the input fields to their default state.
         */
        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            Loans.Clear();
            this.Loans.Add(new Loan(1));

            PaymentAmt.Text = "";
            ResultsText.Content = "";
        }
    }
}
