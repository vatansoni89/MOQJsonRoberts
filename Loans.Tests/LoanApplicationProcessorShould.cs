﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Loans.Domain.Applications;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
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
            var loanApplication = new LoanApplication(1, loanProduct,loanAmount, "Vatan",35,"Ekta Nagar",50000 );

            var mockIdentityVerifier = new Mock<IIdentityVerifier>();
            var mockCreditScorer = new Mock<ICreditScorer>();
            var lap = new LoanApplicationProcessor(mockIdentityVerifier.Object, mockCreditScorer.Object);
            lap.Process(loanApplication);

            //It fails as LoanApplicationProcessor dont accept Null arguments. 
            Assert.That(loanApplication.GetIsAccepted(),Is.False);
        }

        delegate void ValidateCallback(string applicantName, 
            int applicantAge, 
            string applicantAddress, 
            ref IdentityVerificationStatus status);

        [Test]
        public void Accept()
        {
            
            var loanProduct = new LoanProduct(1,"Loan",8.35m);
            var loanAmount = new LoanAmount("inr",3500000);
            var loanApplication = new LoanApplication(1, loanProduct,loanAmount, "Vatan",35,"Ekta Nagar",100000 );

            var mockIdentityVerifier = new Mock<IIdentityVerifier>();
            //mockIdentityVerifier.Setup(x => x.Validate("Vatan", 35, "Ekta Nagar")).Returns(true);

            //Setup for any type matching args
            mockIdentityVerifier.Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>())).Returns(true);

            //set below property hierarchy Autometically, Score is int
            //_creditScorer.ScoreResult.ScoreValue.Score

            //Below code set default values to hierarchy like 0 to Score
            var mockCreditScorer = new Mock<ICreditScorer>() {DefaultValue = DefaultValue.Mock};

            //below code set specific value
            mockCreditScorer.Setup(x => x.ScoreResult.ScoreValue.Score).Returns(300);

            var lap = new LoanApplicationProcessor(mockIdentityVerifier.Object, mockCreditScorer.Object);
            lap.Process(loanApplication);

            //It fails as LoanApplicationProcessor dont accept Null arguments. 
            Assert.That(loanApplication.GetIsAccepted(),Is.True);
        }

        [Test]
        public void NullReturnExample()
        { 
            var mock = new Mock<INullExample>();

            mock.Setup(x=>x.SomeMethod()).Returns<string>(null);

            string mockReturnvalue = mock.Object.SomeMethod();

            Assert.That(mockReturnvalue, Is.Null);
        }
    }

    public interface INullExample
    { 
        string SomeMethod();
    }
}
