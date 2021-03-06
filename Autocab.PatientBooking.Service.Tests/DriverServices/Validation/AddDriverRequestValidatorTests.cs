using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Autocab.PassengerBooking.Data;
using Autocab.PassengerBooking.Data.Models;
using Autocab.PassengerBooking.Service.DriverServices.Requests;
using Autocab.PassengerBooking.Service.DriverServices.Validation;
using System;

namespace Autocab.PassengerBooking.Service.Tests.DriverServices.Validation
{
    [TestFixture]
    public class AddDriverRequestValidatorTests
    {
        private IFixture _fixture;

        private PassengerBookingContext _context;

        private AddDriverRequestValidator _addDriverRequestValidator;

        [SetUp]
        public void SetUp()
        {
            // Boilerplate
            _fixture = new Fixture();

            //Prevent fixture from generating from entity circular references 
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

            // Mock setup
            _context = new PassengerBookingContext(new DbContextOptionsBuilder<PassengerBookingContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

            // Mock default
            SetupMockDefaults();

            // Sut instantiation
            _addDriverRequestValidator = new AddDriverRequestValidator(
                _context
            );
        }

        private void SetupMockDefaults()
        {

        }

        [Test]
        public void ValidateRequest_AllChecksPass_ReturnsPassedValidationResult()
        {
            //arrange
            var request = GetValidRequest();

            //act
            var res = _addDriverRequestValidator.ValidateRequest(request);

            //assert
            res.PassedValidation.Should().BeTrue();
        }

        [TestCase("")]
        [TestCase(null)]
        public void ValidateRequest_FirstNameNullOrEmpty_ReturnsFailedValidationResult(string firstName)
        {
            //arrange
            var request = GetValidRequest();
            request.FirstName = firstName;

            //act
            var res = _addDriverRequestValidator.ValidateRequest(request);

            //assert
            res.PassedValidation.Should().BeFalse();
            res.Errors.Should().Contain("FirstName must be populated");
        }

        [TestCase("")]
        [TestCase(null)]
        public void ValidateRequest_LastNameNullOrEmpty_ReturnsFailedValidationResult(string lastName)
        {
            //arrange
            var request = GetValidRequest();
            request.LastName = lastName;

            //act
            var res = _addDriverRequestValidator.ValidateRequest(request);

            //assert
            res.PassedValidation.Should().BeFalse();
            res.Errors.Should().Contain("LastName must be populated");
        }

        [TestCase("")]
        [TestCase(null)]
        public void ValidateRequest_EmailNullOrEmpty_ReturnsFailedValidationResult(string email)
        {
            //arrange
            var request = GetValidRequest();
            request.Email = email;

            //act
            var res = _addDriverRequestValidator.ValidateRequest(request);

            //assert
            res.PassedValidation.Should().BeFalse();
            res.Errors.Should().Contain("Email must be populated");
        }

        [TestCase("user@")]
        [TestCase("@")]
        [TestCase("user")]
        [TestCase(null)]
        [TestCase("")]
        public void ValidateRequest_InvalidEmail_ReturnsFailedValidationResult(string email)
        {
            //arrange
            var request = GetValidRequest();
            request.Email = email;

            //act
            var res = _addDriverRequestValidator.ValidateRequest(request);

            string[] expectedErrors = { "Email must be populated", "Email must be a valid email address" };

            //assert
            res.PassedValidation.Should().BeFalse();
            res.Errors.Should().IntersectWith(expectedErrors);
        }

        [TestCase("user@domain.com")]
        [TestCase("user@domain-domain.com")]
        [TestCase("user@domain.net")]
        [TestCase("user@1.net")]
        [TestCase("user@domain.co.uk")]
        [TestCase("user.name@domain.com")]
        [TestCase("user.name@domain-domain.com")]
        [TestCase("user.name@domain.net")]
        [TestCase("user.name@1.net")]
        [TestCase("user.name@domain.co.uk")]
        [TestCase("user+100@domain.com")]
        [TestCase("user+100@domain-domain.com")]
        [TestCase("user+100@domain.net")]
        [TestCase("user+100@1.net")]
        [TestCase("user+100@domain.co.uk")]
        public void ValidateRequest_ValidEmail_ReturnsPassedValidationResult(string email)
        {
            //arrange
            var request = GetValidRequest();
            request.Email = email;

            //act
            var res = _addDriverRequestValidator.ValidateRequest(request);

            //assert
            res.PassedValidation.Should().BeTrue();
        }

        [Test]
        public void ValidateRequest_DriverWithEmailAddressAlreadyExists_ReturnsFailedValidationResult()
        {
            //arrange
            var request = GetValidRequest();

            var existingDriver = _fixture
                .Build<Driver>()
                .With(x => x.Email, request.Email)
                .Create();

            _context.Add(existingDriver);
            _context.SaveChanges();

            //act
            var res = _addDriverRequestValidator.ValidateRequest(request);

            //assert
            res.PassedValidation.Should().BeFalse();
            res.Errors.Should().Contain("A Driver with that email address already exists");
        }

        private AddDriverRequest GetValidRequest()
        {
            var request = _fixture.Build<AddDriverRequest>()
                .With(x => x.Email, "user@domain.com")
                .Create();
            return request;
        }
    }
}
