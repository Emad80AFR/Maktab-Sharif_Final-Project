﻿using AM._Application.Contracts.Account;
using AM._Application.Contracts.Account.DTO_s;
using AM._Domain.AccountAgg;
using AM._Domain.RollAgg;
using FrameWork.Application;
using FrameWork.Application.Authentication;
using FrameWork.Application.Authentication.PasswordHashing;
using FrameWork.Application.Messages;
using Microsoft.Extensions.Logging;
using System.Threading;
using FrameWork.Application.FileUpload;
using Microsoft.AspNetCore.WebUtilities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Reflection;
using FrameWork.Infrastructure.ConfigurationModel;

namespace AM._Application.Implementation
{
    public class AccountApplication:IAccountApplication
    {
        private readonly ILogger<AccountApplication> _logger;
        private readonly IFileUploader _fileUploader;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAccountRepository _accountRepository;
        private readonly IAuthHelper _authHelper;
        private readonly IRoleRepository _roleRepository;
        private readonly AppSettingsOption.Domainsettings _appOptions;


    public AccountApplication(ILogger<AccountApplication> logger, IFileUploader fileUploader, IPasswordHasher passwordHasher, IAccountRepository accountRepository, IAuthHelper authHelper, IRoleRepository roleRepository, AppSettingsOption.Domainsettings appOptions)
        {
            _logger = logger;
            _fileUploader = fileUploader;
            _passwordHasher = passwordHasher;
            _accountRepository = accountRepository;
            _authHelper = authHelper;
            _roleRepository = roleRepository;
            _appOptions = appOptions;
        }

        public async Task<AccountViewModel> GetAccountBy(long id,CancellationToken cancellationToken)
        {
            try
            {
                var account = await _accountRepository.Get(id, cancellationToken);
                if (account == null)
                {
                    _logger.LogWarning("Account not found for ID: {AccountId}", id);
                    return null!; 
                }

                _logger.LogInformation("Account details retrieved successfully for ID: {AccountId}", id);
                return new AccountViewModel()
                {
                    Fullname = account.Fullname,
                    Mobile = account.Mobile
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving account details for ID: {AccountId}", id);
                return null!; 
            }
        }

        public async Task<OperationResult> Register(RegisterAccount command,CancellationToken cancellationToken)
        {
            var operation = new OperationResult();
            try
            {
                if (await _accountRepository.Exist(x => x.Username == command.Username || x.Mobile == command.Mobile, cancellationToken))
                {
                    _logger.LogWarning(ApplicationMessages.DuplicatedRecord);
                    return operation.Failed(ApplicationMessages.DuplicatedRecord);
                }

                var password = _passwordHasher.Hash(command.Password);
                const string path = "profilePhotos";
                var picturePath = await _fileUploader.Upload(command.ProfilePhoto, path, cancellationToken);
                var account = new Account(command.Fullname, command.Username, password, command.Mobile, command.RoleId, picturePath);
                await _accountRepository.Create(account, cancellationToken);
                await _accountRepository.SaveChanges(cancellationToken);

                _logger.LogInformation("Account created successfully.");
                return operation.Succeeded();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the account.");
                return operation.Failed("An error occurred while creating the account.");
            }
        }

        public async Task<OperationResult> Edit(EditAccount command, CancellationToken cancellationToken)
        {
            var operation = new OperationResult();
            try
            {
                var account = await _accountRepository.Get(command.Id, cancellationToken);
                if (account == null)
                {
                    _logger.LogWarning(ApplicationMessages.RecordNotFound);
                    return operation.Failed(ApplicationMessages.RecordNotFound);
                }

                if (await _accountRepository.Exist(x =>
                        (x.Username == command.Username || x.Mobile == command.Mobile) && x.Id != command.Id, cancellationToken))
                {
                    _logger.LogWarning(ApplicationMessages.DuplicatedRecord);
                    return operation.Failed(ApplicationMessages.DuplicatedRecord);
                }

                var path = "profilePhotos";
                var picturePath = await _fileUploader.Upload(command.ProfilePhoto, path, cancellationToken);
                account.Edit(command.Fullname, command.Username, command.Mobile, command.RoleId, picturePath);
                await _accountRepository.SaveChanges(cancellationToken);

                _logger.LogInformation("Account edited successfully.");
                return operation.Succeeded();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the account.");
                return operation.Failed("An error occurred while editing the account.");
            }
        }

        public async Task<OperationResult> EditSeller(EditAccount command, CancellationToken cancellationToken)
        {
            var operation = new OperationResult();
            try
            {
                var account = await _accountRepository.Get(command.Id, cancellationToken);
                if (account == null)
                {
                    _logger.LogWarning(ApplicationMessages.RecordNotFound);
                    return operation.Failed(ApplicationMessages.RecordNotFound);
                }

                if (await _accountRepository.Exist(x =>
                        (x.Username == command.Username || x.Mobile == command.Mobile) && x.Id != command.Id, cancellationToken))
                {
                    _logger.LogWarning(ApplicationMessages.DuplicatedRecord);
                    return operation.Failed(ApplicationMessages.DuplicatedRecord);
                }

                var profilePhotoPath = "profilePhotos";
                var picturePath = await _fileUploader.Upload(command.ProfilePhoto, profilePhotoPath, cancellationToken);
                var shopPhotoPath = "shopPhotos";
                var shopPath = await _fileUploader.Upload(command.ShopPicture, shopPhotoPath, cancellationToken);
                account.EditSeller( command.Fullname, command.ShopName!, command.Username,command.Mobile,picturePath, shopPath);
                await _accountRepository.SaveChanges(cancellationToken);

                _logger.LogInformation("Account edited successfully.");
                return operation.Succeeded();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the account.");
                return operation.Failed("An error occurred while editing the account.");
            }
        }

        public async Task<OperationResult> Activate(long id, CancellationToken cancellationToken)
        {
            var operation = new OperationResult();
            try
            {
                var account = await _accountRepository.Get(id, cancellationToken);
                if (account == null)
                {
                    _logger.LogWarning(ApplicationMessages.RecordNotFound);
                    return operation.Failed(ApplicationMessages.RecordNotFound);
                }
                
                account.Activate();
                await _accountRepository.SaveChanges(cancellationToken);
                _logger.LogInformation("Account Activate successfully.");
                return operation.Succeeded();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Activating the account.");
                return operation.Failed("An error occurred while editing the account.");
            }
        }

        public async Task<OperationResult> DeActive(long id, CancellationToken cancellationToken)
        {
            var operation = new OperationResult();
            try
            {
                var account = await _accountRepository.Get(id, cancellationToken);
                if (account == null)
                {
                    _logger.LogWarning(ApplicationMessages.RecordNotFound);
                    return operation.Failed(ApplicationMessages.RecordNotFound);
                }

                account.Deactivate();
                await _accountRepository.SaveChanges(cancellationToken);
                _logger.LogInformation("Account DeActivate successfully.");
                return operation.Succeeded();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while DeActivating the account.");
                return operation.Failed("An error occurred while editing the account.");
            }
        }

        public async Task<OperationResult> ChangePassword(ChangePassword command,CancellationToken cancellationToken)
        {
            var operation = new OperationResult();
            try
            {
                var account = await _accountRepository.Get(command.Id, cancellationToken);
                if (account == null)
                {
                    _logger.LogWarning(ApplicationMessages.RecordNotFound);
                    return operation.Failed(ApplicationMessages.RecordNotFound);
                }

                if (command.Password != command.RePassword)
                {
                    _logger.LogWarning("An error occurred,password Not Match");
                }

                var password = _passwordHasher.Hash(command.Password);
                account.ChangePassword(password);
                await _accountRepository.SaveChanges(cancellationToken);

                _logger.LogInformation("Account password changed successfully.");
                return operation.Succeeded();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while changing the account password.");
                return operation.Failed("An error occurred while changing the account password.");
            }
        }

        public async Task<OperationResult> Login(Login command, CancellationToken cancellationToken)
        {
            var operation = new OperationResult();
            try
            {
                var account = await _accountRepository.GetBy(command.Username, cancellationToken);
                if (account == null)
                {
                    _logger.LogWarning("An error occurred ; password or username is wrong!");
                    return operation.Failed(ApplicationMessages.WrongUserPass);
                }

                if (!account.IsActive)
                    return operation.Failed(ApplicationMessages.UnActiveAccount);

                var result = _passwordHasher.Check(account.Password, command.Password);
                if (!result.Verified)
                {
                    _logger.LogWarning("An error occurred ; password or username is wrong!");
                    return operation.Failed(ApplicationMessages.WrongUserPass);
                }

                var role = await _roleRepository.Get(account.RoleId, cancellationToken);
                var permissions=role!.Permissions
                    .Select(x => x.Code)
                    .ToList();

                var authViewModel = new AuthViewModel(account.Id, account.RoleId, account.Fullname
                    , account.Username, account.Mobile,account.ProfilePhoto,permissions);

                _authHelper.Signin(authViewModel);

                _logger.LogInformation("User signed in successfully.");
                return operation.Succeeded();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while signing in.");
                return operation.Failed("An error occurred while signing in.");
            }
        }

        public async Task<EditAccount> GetDetails(long id,CancellationToken cancellationToken)
        {
            try
            {
                var accountDetails = await _accountRepository.GetDetails(id, cancellationToken);
                _logger.LogInformation("Account details retrieved successfully.");
                return accountDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving account details.");
                throw; 
            }
        }

        public async Task<List<AccountViewModel>> Search(AccountSearchModel searchModel, CancellationToken cancellationToken)
        {
            try
            {
                var searchResults = await _accountRepository.Search(searchModel, cancellationToken);
                _logger.LogInformation("Account search completed successfully.");
                return searchResults;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for accounts.");
                throw; 
            }
        }

        public Task Logout(CancellationToken cancellationToken)
        {
            try
            {
                 _authHelper.SignOut();
                _logger.LogInformation("User signed out successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while signing out.");
                throw;
            }

            return Task.CompletedTask;
        }

        public async Task<List<AccountViewModel>> GetAccounts(CancellationToken cancellationToken)
        {
            try
            {
                var accounts = await _accountRepository.GetAccounts(cancellationToken);
                _logger.LogInformation("Accounts retrieved successfully.");
                return accounts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving accounts.");
                throw; 
            }
        }

        public async Task<FinancialModel> GetFinancialInfo(long id, CancellationToken cancellationToken)
        {
            return await _accountRepository.GetFinancialInfo(id, cancellationToken);
        }

        public async Task<bool> UpdateFinancialInfo(long id, double amount, CancellationToken cancellationToken)
        {
            var account=await _accountRepository.Get(id,cancellationToken);
            if (account == null)
                return false;

            account!.UpdateSaleAmount(amount);

            if (account.SalesAmount >= _appOptions.SaleAmount.GoldSale)
                account.Medal = _appOptions.Medals.Gold;
            else if (account.SalesAmount >= _appOptions.SaleAmount.SilverSale)
                account.Medal = _appOptions.Medals.Silver;
            else if (account.SalesAmount >= _appOptions.SaleAmount.BronzeSale)
                account.Medal = _appOptions.Medals.Bronze;
            await _accountRepository.SaveChanges(cancellationToken);
            return true;
        }

        public async Task<bool> UpdateSaleAmount(double amount, CancellationToken cancellationToken)
        {
            var account=await _accountRepository.GetManagerAccount(cancellationToken);
            if (account == null)
                return false;
            account.UpdateSaleAmount(amount);
            await _accountRepository.SaveChanges(cancellationToken);
            return true;
        }

        public async Task<bool> AssignModel(long id, CancellationToken cancellationToken)
        {
            var account=await _accountRepository.Get(id, cancellationToken);
            if (account == null)
                return false;

            if (account.SalesAmount >= _appOptions.SaleAmount.GoldSale)
                account.Medal = _appOptions.Medals.Gold;
            else if(account.SalesAmount>=_appOptions.SaleAmount.SilverSale)
                account.Medal= _appOptions.Medals.Silver;
            else if(account.SalesAmount>=_appOptions.SaleAmount.BronzeSale)
                account.Medal=_appOptions.Medals.Bronze;
            await _accountRepository.SaveChanges(cancellationToken);
            return true;
        }

        public async Task<string> GetAccountName(long id, CancellationToken cancellationToken)
        {

            return await _accountRepository.GetAccountName(id,cancellationToken);
        }
    }
}