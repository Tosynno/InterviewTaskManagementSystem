using Application.Dto;
using BC = BCrypt.Net.BCrypt;
using Application.Models;
using Application.Respository;
using Application.Validations;
using AutoMapper;
using Domain;
using Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class OnboardingService : BaseRepository<tbl_Onboarding>, IOnboarding
    {
        private IRedisCache _redis;
        private readonly IMapper _mapper;
        public OnboardingService(AppDbContext dbContext, IRedisCache redis, IMapper mapper) : base(dbContext)
        {
            _redis = redis;
            _mapper = mapper;
        }

        public async Task<OnboardingDto> AddOnboarding(OnbardingRequest request)
        {
           
            tbl_Onboarding sa = new tbl_Onboarding();
            OnboardingDto result = new OnboardingDto();
           
            var checkotp = await Verifyotp(request.Email, request.Otp);
            if (checkotp == true)
            {
                 
                sa.Email = request.Email;
                sa.PhoneNumber = request.PhoneNumber;
                sa.Password = BC.HashPassword(request.Password, "1234567890poiuytrewqasdfghjklmnbvcxz");

                await AddAsync(sa);
                // BC.Verify(model.Password, account.PasswordHash, "1234567890poiuytrewqasdfghjklmnbvcxz")
                //using JWT to generate token 
                result.Token = "85e87dghubcjndkjnfdkjkdcijjfdhjjhdjhvjhdjhvdjhfgedysfeycdtucdw";
                result.ResponseCode = "00";
                result.Message = "Onboarding was Succesful";
                
            }
            else
            {
                var cheotp = await Verifyotp(request.PhoneNumber, request.Otp);
                if (cheotp == true)
                {
                    sa.Email = request.Email;
                    sa.PhoneNumber = request.PhoneNumber;
                    sa.Password = BC.HashPassword(request.Password, "1234567890poiuytrewqasdfghjklmnbvcxz");

                    await AddAsync(sa);
                    result.Token = "85e87dghubcjndkjnfdkjkdcijjfdhjjhdjhvjhdjhvdjhfgedysfeycdtucdw";
                    result.ResponseCode = "00";
                    result.Message = "Onboarding was Succesful";
                }
                else
                {
                    result.ResponseCode = "99";
                    result.Message = "Fail to Onboarding Customer at this time, Kindly Contact the support person";
                }
              
            }
           
            
            return result;
        }

        public async Task<List<OnboardingResponse>> GetAllOnboarding()
        {
            var result = await GetAllAsync();
            var res = _mapper.Map<List<OnboardingResponse>>(result);
            return res;
        }

        public async Task<OnboardingResponse> GetOnboardingByEmail(string Email)
        {
            var result = await GetAllAsync();
            var getres = result.FirstOrDefault(c => c.Email == Email);
            var res = _mapper.Map<OnboardingResponse>(getres);
            return res;
        }

        public async Task<OnboardingResponse> GetOnboardingByPhone(string Phone)
        {
            var result = await GetAllAsync();
            var getres = result.FirstOrDefault(c => c.PhoneNumber == Phone);
            var res = _mapper.Map<OnboardingResponse>(getres);
            return res;
        }

        public Task<string> GetToken(string Token)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> VerifyEmail(string Email)
        {
            Random d = new Random();
            var otpinput = d.Next(100000, 999999).ToString();

            //save otp in redis or any other cache
            var saveotp = await _redis.PostOTP(Email, otpinput);
            if (saveotp == true)
                //send otp using sendgrip, sendblue, twillo, infobiz, mountimobi etc, this processor 
                //help pass customer infomation and also help organisation pass info to their customer by email or mobile device
                return true;
            return false;
        }

        public async Task<bool> Verifyotp(string PhoneorEmail, string input)
        {
            var saveotp = await _redis.GetOTP(PhoneorEmail);
            if (saveotp == input)
                return true;
            return false;
        }

        public async Task<bool> VerifyPhone(string Phone)
        {
            Random d = new Random();
            var otpinput = d.Next(100000, 999999).ToString();

            var saveotp = await _redis.PostOTP(Phone, otpinput);
            if (saveotp == true)
            return true;
            return false;
        }

        public async Task<OnboardingDto> SignIn(string Email, string Password)
        {
            OnboardingDto res = new OnboardingDto();
            var result = await DbContext.tbl_Onboardings.FirstOrDefaultAsync(c => c.Email == Email);
            if (result != null)
            {
               var passres = BC.Verify(Password, result.Password + "1234567890poiuytrewqasdfghjklmnbvcxz");

                if (passres == true)
                {
                    //using JWT to generate token 
                    res.Token = "85e87dghubcjndkjnfdkjkdcijjfdhjjhdjhvjhdjhvdjhfgedysfeycdtucdw";
                    res.ResponseCode = "00";
                    res.Message = "Login was Succesful";
                    return res;
                }
                else
                {
                    //using JWT to generate token 
                    res.Token = "";
                    res.ResponseCode = "99";
                    res.Message = "Invalid Credential!!!";
                    return res;
                }
               
            }
            else
            {
                res.Token = "";
                res.ResponseCode = "099";
                res.Message = "Access Denied!!!";
                return res;
            }
        }
    }
}
