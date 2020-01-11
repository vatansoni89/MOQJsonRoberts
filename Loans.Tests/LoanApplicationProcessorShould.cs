using System;
using System.Collections.Generic;
using System.Text;
using Loans.Domain.Applications;
using NUnit.Framework;

namespace Loans.Tests
{
    [TestFixture]
    public class LoanApplicationProcessorShould
    {
        [Test]
        public void DeclineLowSalary()
        {
            
            var loanProduct = new LoanProduct(1,"Loan",8.35m);
            var loanAmount = new LoanAmount("inr",3500000);
            var loanApplication = new LoanApplication(1, loanProduct,loanAmount, "Vatan",35,"Ekta Nagar",100000 );

            var lap = new LoanApplicationProcessor(null, null);
            lap.Process(loanApplication);

            //It fails as LoanApplicationProcessor dont accept Null arguments. 
            Assert.That(loanApplication.GetIsAccepted(),Is.False);
        }
    }
}
