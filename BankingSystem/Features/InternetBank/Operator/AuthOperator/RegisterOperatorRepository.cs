//using Azure.Core;
//using BankingSystem.DB;
//using BankingSystem.DB.Entities;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//namespace BankingSystem.Features.InternetBank.Operator.AuthOperator
//{
//    public interface IOperatorRepository
//    {
//        public Task AddOperatorAsync(OperatorEntity entity);
//        public Task<OperatorEntity> GetOperatorByPersonalNumberAsync(LoginOperatorRequest request);
//        public Task<OperatorEntity> GetOperatorByPasswordAsync(LoginOperatorRequest request);
//        public Task SaveChangesAsync();
//        public Task<bool> OperatorExists(string personalNumber);
//    }
//    public class RegisterOperatorRepository : IOperatorRepository
//    {
//        private readonly AppDbContext _db;
//       private readonly UserManager<UserEntity> _userManager;

//        public RegisterOperatorRepository(AppDbContext db)
//        {
//            _db = db;
//        }

//        public async Task AddOperatorAsync(OperatorEntity entity)
//        {
//            await _db.Operators.AddAsync(entity);
//        }

//        public async Task<OperatorEntity> GetOperatorByPersonalNumberAsync(LoginOperatorRequest request)
//        {
//            var response = await _db.Operators.Where(o => o.PersonalNumber == request.PersonalNumber).FirstOrDefaultAsync();
//            return response;
//        }

//        public async Task<OperatorEntity> GetOperatorByPasswordAsync(LoginOperatorRequest request)
//        {
//            var response = await _db.Operators.Where(o => o.Password == request.Password).FirstOrDefaultAsync();
//            return response;
//        }
//        public async Task SaveChangesAsync()
//        {
//            await _db.SaveChangesAsync();
//        }

//        public async Task<bool> OperatorExists(string personalNumber)
//        {
//            return _db.Operators.Any(o => o.PersonalNumber == personalNumber);
//        }
//    }
//}
