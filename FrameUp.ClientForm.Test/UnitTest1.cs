using FrameUp.ClientForm.Application.Models;
using FrameUp.ClientForm.Application.UsesCases;
using FrameUp.ClientForm.Domain.Entities;
using FrameUp.ClientForm.Domain.Interfaces;
using FrameUp.ClientForm.Domain.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace FrameUp.ClientForm.Test
{
    public class AccountUseCaseTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IPasswordHasher> _mockPasswordHasher;
        private Mock<ITokenService> _mockTokenService;
        private AccountUseCase _accountUseCase;

        [SetUp]
        public void Setup()
        {
            // Configura os mocks
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _mockTokenService = new Mock<ITokenService>();

            // Instancia o UseCase
            _accountUseCase = new AccountUseCase(
                _mockUserRepository.Object,
                _mockPasswordHasher.Object,
                _mockTokenService.Object
            );
        }

        [Test]
        public async Task RegisterAsync_UserAlreadyExists_ReturnsErrorResponse()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "password123"
            };

            _mockUserRepository.Setup(repo => repo.UserExistsAsync(request.Email))
                               .ReturnsAsync(true); // Simula que o usuário já existe

            // Act
            var response = await _accountUseCase.RegisterAsync(request);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual("User already exists.", response.Message);
        }

        [Test]
        public async Task RegisterAsync_UserDoesNotExist_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "password123"
            };

            _mockUserRepository.Setup(repo => repo.UserExistsAsync(request.Email))
                               .ReturnsAsync(false); // Simula que o usuário não existe
            _mockPasswordHasher.Setup(ph => ph.HashPassword(request.Password))
                               .Returns("hashedPassword"); // Simula o hash da senha

            _mockUserRepository.Setup(repo => repo.AddUserAsync(It.IsAny<User>()))
                               .Returns(Task.CompletedTask); // Simula a adição do usuário

            // Act
            var response = await _accountUseCase.RegisterAsync(request);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual("User registered successfully.", response.Message);
        }

        [Test]
        public async Task LoginAsync_InvalidCredentials_ReturnsErrorResponse()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "john.doe@example.com",
                Password = "wrongPassword"
            };

            _mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(request.Email))
                               .ReturnsAsync(new User("John Doe", request.Email, "hashedPassword"));
            _mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword("hashedPassword", request.Password))
                               .Returns(Microsoft.AspNet.Identity.PasswordVerificationResult.Failed); // Simula senha inválida

            // Act
            var response = await _accountUseCase.LoginAsync(request);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual("Invalid credentials.", response.Message);
        }

        [Test]
        public async Task LoginAsync_ValidCredentials_ReturnsSuccessResponseWithToken()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "john.doe@example.com",
                Password = "password123"
            };

            var user = new User("John Doe", request.Email, "hashedPassword");
            _mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(request.Email))
                               .ReturnsAsync(user);
            _mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword("hashedPassword", request.Password))
                               .Returns(Microsoft.AspNet.Identity.PasswordVerificationResult.Success); // Simula senha válida
            _mockTokenService.Setup(ts => ts.GenerateToken(user))
                             .Returns("validToken"); // Simula a geração do token

            // Act
            var response = await _accountUseCase.LoginAsync(request);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual("Login successful.", response.Message);
            Assert.AreEqual("validToken", response.Token);
        }

        [Test]
        public async Task UserExistsAsync_UserDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var email = "nonexistent.user@example.com";
            _mockUserRepository.Setup(repo => repo.UserExistsAsync(email))
                               .ReturnsAsync(false); // Simula que o usuário não existe

            // Act
            var exists = await _accountUseCase.UserExistsAsync(email);

            // Assert
            Assert.IsFalse(exists);
        }

        [Test]
        public async Task ValidateAccountAsync_AccountExists_ReturnsTrue()
        {
            // Arrange
            var email = "john.doe@example.com";
            _mockUserRepository.Setup(repo => repo.UserExistsAsync(email))
                               .ReturnsAsync(true); // Simula que o usuário existe

            // Act
            var isValid = await _accountUseCase.ValidateAccountAsync(email);

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public async Task ValidateAccountAsync_AccountDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var email = "nonexistent.user@example.com";
            _mockUserRepository.Setup(repo => repo.UserExistsAsync(email))
                               .ReturnsAsync(false); // Simula que o usuário não existe

            // Act
            var isValid = await _accountUseCase.ValidateAccountAsync(email);

            // Assert
            Assert.IsFalse(isValid);
        }
    }
}