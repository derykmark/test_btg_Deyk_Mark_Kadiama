using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankKRT.Application.DTOs;
using BankKRT.Application.Interfaces;
using BankKRT.Domain.Entities;
using BankKRT.Domain.Enums;
using BankKRT.Domain.Events;
using BankKRT.Domain.Interfaces;
using BankKRT.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace BankKRT.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMediator _mediator;
    private readonly IMemoryCache _cache;

    public AccountService(
        IAccountRepository accountRepository,
        IMediator mediator,
        IMemoryCache cache)
    {
        _accountRepository = accountRepository;
        _mediator = mediator;
        _cache = cache;
    }

    public async Task<AccountResponse> CreateAsync(CreateAccountRequest request)
    {
        var cpf = CPF.Create(request.Cpf);
        
        var existingAccount = await _accountRepository.GetByCpfAsync(cpf);
        if (existingAccount != null)
        {
            throw new InvalidOperationException("CPF already in use.");
        }

        var account = Account.Create(request.HolderName, cpf);
        
        await _accountRepository.AddAsync(account);

        await _mediator.Publish(new AccountCreatedEvent(
            account.Id,
            account.HolderName,
            account.Cpf,
            account.CreatedAt));

        return MapToResponse(account);
    }

    public async Task<AccountResponse?> GetByIdAsync(int id)
    {
        var cacheKey = $"account:{id}";
        
        if (!_cache.TryGetValue(cacheKey, out AccountResponse? response))
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null) return null;

            response = MapToResponse(account);
            
            var today = DateTime.Today;
            var endOfDay = today.AddDays(1).AddTicks(-1);
            
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(endOfDay);

            _cache.Set(cacheKey, response, cacheEntryOptions);
        }

        return response;
    }

    public async Task<IEnumerable<AccountResponse>> GetAllAsync()
    {
        var accounts = await _accountRepository.GetAllAsync();
        return accounts.Select(MapToResponse);
    }

    public async Task<AccountResponse> UpdateAsync(int id, UpdateAccountRequest request)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        if (account == null)
            throw new KeyNotFoundException($"Account {id} not found.");

        if (!string.IsNullOrWhiteSpace(request.HolderName))
        {
            account.Update(request.HolderName);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (Enum.TryParse<AccountStatus>(request.Status, true, out var status))
            {
                if (status == AccountStatus.Active)
                    account.Activate();
                else if (status == AccountStatus.Inactive)
                    account.Deactivate();
            }
        }

        await _accountRepository.UpdateAsync(account);

        _cache.Remove($"account:{id}");
        
        await _mediator.Publish(new AccountUpdatedEvent(
            account.Id,
            account.HolderName,
            account.Status,
            account.Status,
            account.UpdatedAt ?? DateTime.UtcNow));

        return MapToResponse(account);
    }

    public async Task DeleteAsync(int id)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        if (account == null)
            throw new KeyNotFoundException($"Account {id} not found.");

        await _accountRepository.DeleteAsync(account);

        _cache.Remove($"account:{id}");

        await _mediator.Publish(new AccountDeletedEvent(
            account.Id,
            account.HolderName,
            account.Cpf,
            DateTime.UtcNow));
    }

    private static AccountResponse MapToResponse(Account account)
    {
        return new AccountResponse(
            account.Id,
            account.HolderName,
            account.Cpf, // Uses implicit conversion from CPF to string
            account.Status.ToString(),
            account.CreatedAt,
            account.UpdatedAt
        );
    }
}
