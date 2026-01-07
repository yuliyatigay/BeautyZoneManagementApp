using BusinessLogic.Services;
using Domain.Enums;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Domain.ServicesInterfaces;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BeautyZoneTests.ServicesTests;

public class AccountServiceTest
{
    private readonly Mock<IJwtService> _jwtServiceMock = new();
    private readonly Mock<IAccountRepository> _accountRepositoryMock = new();
    private readonly Mock<IValidator<Account>> _validatorMock = new();

    private AccountService CreateService()
        => new AccountService(
            _jwtServiceMock.Object,
            _validatorMock.Object,
            _accountRepositoryMock.Object
        );

    [Fact]
    public async Task RegisterAsync_ShouldCreateAccount_WhenValid()
    {
        var account = new Account
        {
            Email = "test@test.com",
            PasswordHash = "password"
        };

        _accountRepositoryMock
            .Setup(r => r.GetByEmail(account.Email))
            .ReturnsAsync((Account?)null);

        _validatorMock
            .Setup(v => v.ValidateAsync(account, default))
            .ReturnsAsync(new ValidationResult());

        var service = CreateService();

        await service.RegisterAsync(account);

        _accountRepositoryMock.Verify(r =>
            r.CreateAccount(It.Is<Account>(a =>
                a.Email == account.Email &&
                a.Role == UserRole.user &&
                a.PasswordHash != "password"
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrow_WhenAccountAlreadyExists()
    {
        var account = new Account { Email = "test@test.com" };

        _accountRepositoryMock
            .Setup(r => r.GetByEmail(account.Email))
            .ReturnsAsync(new Account());

        var service = CreateService();

        var act = async () => await service.RegisterAsync(account);

        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Account already exists");
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrow_WhenValidationFails()
    {
        var account = new Account { Email = "test@test.com" };

        _accountRepositoryMock
            .Setup(r => r.GetByEmail(account.Email))
            .ReturnsAsync((Account?)null);

        _validatorMock
            .Setup(v => v.ValidateAsync(account, default))
            .ReturnsAsync(new ValidationResult(new[]
            {
                new ValidationFailure("Email", "Invalid email")
            }));

        var service = CreateService();

        var act = async () => await service.RegisterAsync(account);

        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("*Invalid email*");
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnJwt_WhenCredentialsValid()
    {
        var password = "password";

        var account = new Account
        {
            Email = "test@test.com",
            PasswordHash = new PasswordHasher<Account>()
                .HashPassword(null!, password)
        };

        _accountRepositoryMock
            .Setup(r => r.GetByEmail(account.Email))
            .ReturnsAsync(account);

        _jwtServiceMock
            .Setup(j => j.GenerateJwtToken(account))
            .Returns("jwt-token");

        var service = CreateService();

        var token = await service.LoginAsync(account.Email, password);

        token.Should().Be("jwt-token");
    }

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenAccountNotFound()
    {
        _accountRepositoryMock
            .Setup(r => r.GetByEmail(It.IsAny<string>()))
            .ReturnsAsync((Account?)null);

        var service = CreateService();

        var act = async () => await service.LoginAsync("test@test.com", "password");

        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Account is not registered");
    }

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenPasswordInvalid()
    {
        var account = new Account
        {
            Email = "test@test.com",
            PasswordHash = new PasswordHasher<Account>()
                .HashPassword(null!, "correct")
        };

        _accountRepositoryMock
            .Setup(r => r.GetByEmail(account.Email))
            .ReturnsAsync(account);

        var service = CreateService();

        var act = async () => await service.LoginAsync(account.Email, "wrong");

        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Invalid username or password");
    }
}