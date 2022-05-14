using Application.Dto;
using Application.Models;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Respository
{
    public interface IOnboarding : IRepository<tbl_Onboarding>
    {
        Task<OnboardingDto> SignIn(string Email, string Password);
        Task<bool> VerifyPhone(string Phone);
        Task<bool> VerifyEmail(string Email);
        Task<bool> Verifyotp(string PhoneorEmail, string input);
        Task<OnboardingDto> AddOnboarding(OnbardingRequest request);
        Task<List<OnboardingResponse>> GetAllOnboarding();
        Task<OnboardingResponse> GetOnboardingByEmail(string Email);
        Task<OnboardingResponse> GetOnboardingByPhone(string Phone);
        Task<string> GetToken(string Token);
    }
}
