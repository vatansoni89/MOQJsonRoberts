using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Loans.Domain.Applications;
using Moq;
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

            var mockIdentityVerifier = new Mock<IIdentityVerifier>();
            var mockCreditScorer = new Mock<ICreditScorer>();
            var lap = new LoanApplicationProcessor(mockIdentityVerifier.Object, mockCreditScorer.Object);
            lap.Process(loanApplication);

            //It fails as LoanApplicationProcessor dont accept Null arguments. 
            Assert.That(loanApplication.GetIsAccepted(),Is.False);
        }

        [Test]
        public void Accept()
        {
            
            var loanProduct = new LoanProduct(1,"Loan",8.35m);
            var loanAmount = new LoanAmount("inr",3500000);
            var loanApplication = new LoanApplication(1, loanProduct,loanAmount, "Vatan",35,"Ekta Nagar",100000 );

            var mockIdentityVerifier = new Mock<IIdentityVerifier>();
            //mockIdentityVerifier.Setup(x => x.Validate("Vatan", 35, "Ekta Nagar")).Returns(true);

            //Setup for any type matching args
            //mockIdentityVerifier.Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>())).Returns(true);

            //setup for output var
            bool isValidOutValue = true;
            mockIdentityVerifier.Setup(x => x.Validate(It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                out isValidOutValue));

            var mockCreditScorer = new Mock<ICreditScorer>();
            //mockCreditScorer.Setup(x => x.CalculateScore("Vatan", "Ekta Nagar")); //It returns null so could not be used.

            var lap = new LoanApplicationProcessor(mockIdentityVerifier.Object, mockCreditScorer.Object);
            lap.Process(loanApplication);

            //It fails as LoanApplicationProcessor dont accept Null arguments. 
            Assert.That(loanApplication.GetIsAccepted(),Is.True);
        }
    }
}
