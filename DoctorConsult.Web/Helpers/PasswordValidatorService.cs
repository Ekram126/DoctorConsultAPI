using DoctorConsult.Models;
using Microsoft.AspNetCore.Identity;

namespace DoctorConsult.Web.Helpers
{
    public class PasswordValidatorService : IPasswordValidator<ApplicationUser>
    {
        private readonly int _minimumLength;
        private readonly bool _requireUppercase;
        private readonly bool _requireLowercase;
        private readonly bool _requireDigit;
        private readonly bool _requireNonAlphanumeric;

        public PasswordValidatorService(IConfiguration configuration)
       {
            _minimumLength = configuration.GetValue<int>("Password:MinimumLength", 6);
            _requireUppercase = configuration.GetValue<bool>("Password:RequireUppercase", true);
            _requireLowercase = configuration.GetValue<bool>("Password:RequireLowercase", true);
            _requireDigit = configuration.GetValue<bool>("Password:RequireDigit", true);
            _requireNonAlphanumeric = configuration.GetValue<bool>("Password:RequireNonAlphanumeric", true);
        }

        public async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> userManager, ApplicationUser user, string password)
        {
            var errors = new List<IdentityError>();

            if (password.Length < _minimumLength)
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordTooShort",
                    Description = $"Password must be at least {_minimumLength} characters long."
                });
            }

            if (_requireUppercase && !password.Any(char.IsUpper))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordMissingUppercase",
                    Description = "Password must contain at least one uppercase letter."
                });
            }

            if (_requireLowercase && !password.Any(char.IsLower))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordMissingLowercase",
                    Description = "Password must contain at least one lowercase letter."
                });
            }

            if (_requireDigit && !password.Any(char.IsDigit))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordMissingDigit",
                    Description = "Password must contain at least one digit."
                });
            }

            if (_requireNonAlphanumeric && !password.Any(char.IsPunctuation))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordMissingNonAlphanumeric",
                    Description = "Password must contain at least one non-alphanumeric character."
                });
            }

            if (password.Equals(user.UserName, StringComparison.OrdinalIgnoreCase))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordMatchesUsername",
                    Description = "Password cannot match the username."
                });
            }

            if (user.UserName.Contains(password, StringComparison.OrdinalIgnoreCase))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordContainsUsername",
                    Description = "Password cannot contain a substring of the username."
                });
            }

            if (!errors.Any())
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(errors.ToArray());
        }
   
    
    
    
    }
}
