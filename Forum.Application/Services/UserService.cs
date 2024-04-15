using Forum.Application.Dto;
using Forum.Application.Repositories;
using Forum.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Application.Services
{
    public class AuthenticationService
    {
        private readonly PasswordService _passwordService;
        private readonly IUserRepository _userRepository;

        public AuthenticationService(
            PasswordService passwordService,
            IUserRepository userRepository)
        {
            _passwordService = passwordService;
            _userRepository = userRepository;
        }

        public async Task<(UserDetailsDto? User, string? Error)> AuthenticateAsync(
            string? username,
            string? password)
        {
            if (string.IsNullOrEmpty(username))
            {
                return (null, "Invalid username");
            }

            if (string.IsNullOrEmpty(password))
            {
                return (null, "Invalid password");
            }

            var user = await _userRepository.GetByUsernameAsync(username);

            if (user is null)
            {
                return (null, $"User with username {username} does not exist");
            }

            var passwordHash = _passwordService.GetHash(password);
            if (!passwordHash.Equals(user.Password))
            {
                return (null, $"Invalid user password");
            }

            var userDto = new UserDetailsDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                Username = user.Username,
                Role = user.Role,
            };

            return (userDto, null);
        }
    }
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordService _passwordService;

        public UserService(
            PasswordService passwordService,
            IUserRepository userRepository)
        {
            _passwordService = passwordService;
            _userRepository = userRepository;
        }

        public async Task<User?> CreateUserAsync(
            CreateUserDto user, 
            Role role = Role.User)
        {
            if (!user.ConfirmPassword!.Equals(user.Password))
            {
                return null;
            }

            var exist = await _userRepository
                .IsExistingUsernameAsync(user.Username!);

            if (exist)
            {
                return null;
            }

            exist = await _userRepository
               .IsExistingEmailAsync(user.Username!);

            if (exist)
            {
                return null;
            }

            var dbUser = new User
            {
                Email = user.Email!.ToLower(),
                CreateDate = DateTime.Now,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Role = role,
                Password = _passwordService.GetHash(user.Password!),
                UpdateDate = DateTime.Now
            };

            await _userRepository.SaveAsync(dbUser);
            return dbUser;
        }

        public async Task<(long? Id, string Error)> UpdatedUserAsync(
            long? id,
            UpdateUserDto user)
        {
            var exist = await _userRepository
                .IsExistingUsernameAsync(id!.Value, user.Username!);

            if (exist)
            {
                return (null, "Username already exists!");
            }

            exist = await _userRepository
               .IsExistingEmailAsync(id!.Value, user.Username!);

            if (exist)
            {
                return (null, "Email already exists!");
            }

            var dbUser = await _userRepository.GetByIdAsync(id!.Value);

            if (dbUser is null)
            {
                return (null, "Invalid user id!");
            }

            dbUser.Email = user.Email!.ToLower();
            dbUser.UpdateDate = DateTime.Now;
            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.Username = user.Username;

            await _userRepository.UpdateAsync(dbUser);

            return (dbUser.Id, string.Empty);
        }

        public async Task<(UserDetailsDto? User, string Error)> DeleteUserAsync(
            long? id)
        {
            var user = await _userRepository.GetByIdAsync(id!.Value);



            if (user is null)
            {
                return (null, "Invalid user id!");
            }
            await _userRepository.DeleteAsync(user);            

            user.IsDeleted = true;

            await _userRepository.UpdateAsync(user);

            var dtoUser = new UserDetailsDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                Username = user.Username,
            };

            return (dtoUser, string.Empty);
        }

        public async Task<(IEnumerable<UserShortDto> Users, string Error)> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            var dtUsers = users.Select(e => new UserShortDto
            {
                Id = e.Id,
                FullName = string.Format("{0} {1}", e.FirstName, e.LastName),
                Username = e.Username
            })
            .ToArray();

            return (dtUsers, string.Empty);
        }

        public async Task<(UserDetailsDto? User, string Error)> GetUsersAsync(long? userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                return (null, $"User with id {userId} not found");
            }

            var dtoUser = new UserDetailsDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                Username = user.Username,
            };

            return (dtoUser, string.Empty);
        }

    }
        public class PasswordService
        {
            public string GetHash(string text)
            {
                using (var sh256 = SHA256.Create())
                {
                    var bytes = Encoding.UTF8.GetBytes(text);
                    var hashBytes = sh256.ComputeHash(bytes);
                    return BitConverter.ToString(hashBytes);
                }
            }
        }
}
