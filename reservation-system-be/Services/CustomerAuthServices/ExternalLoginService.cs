using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Oauth2.v2.Data;
using Google.Apis.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Facebook;
using System.Threading.Tasks;
using reservation_system_be.Services.CustomerServices;
using reservation_system_be.Models;
using static System.Net.WebRequestMethods;
using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Services.EmailServices;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Google;

namespace reservation_system_be.Services.CustomerAuthServices

{
    public interface IExternalLoginService
    {
        Task<Customer> ValidateGoogleLogin(string token);
        Task<Customer> ValidateFacebookLogin(string token);

        Task<Customer> ValidateGoogleSignup(string token);
        
    }

    public class ExternalLoginService : IExternalLoginService
    {
        private readonly ICustomerService _customerService;

        public ExternalLoginService(ICustomerService customerService)
        {

            _customerService = customerService;
        }



        public async Task<Customer> ValidateGoogleSignup(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    throw new ArgumentException("Google access token is missing or empty.");
                }
                // Validate and parse the token

                GoogleCredential credential = GoogleCredential.FromAccessToken(token);


                // Create the OAuth2 Service
                var service = new Oauth2Service(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "VehicleHub" // Replace with your application name
                });

                // Fetch user info
                var userinfo = await service.Userinfo.Get().ExecuteAsync();

                // Extract email and other necessary details
                string email = userinfo.Email;

                if (string.IsNullOrEmpty(email))
                {
                    throw new InvalidOperationException("Failed to retrieve email from Google user info.");
                }

                // Here you can create or update your Customer object in your system

                Customer customer = new Customer
                {
                    Email = email
                    // Add other necessary fields
                };

                return customer;
            }
            catch (ArgumentException ex)
            {
                // Handle exceptions according to your application's requirements
                throw new ArgumentException("Invalid Google access token.", ex);
            }

        
            catch (Exception ex)
            {
                // Handle other unexpected errors
                throw new InvalidOperationException("Failed to validate Google signup.", ex);
            }
        }   
      

        public async Task<Customer> ValidateGoogleLogin(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    throw new ArgumentException("Google access token is missing or empty.");
                }

                GoogleCredential credential = GoogleCredential.FromAccessToken(token);


                var service = new Oauth2Service(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "VehicleHub" // Replace with your application name
                });

                Userinfo userinfo = await service.Userinfo.Get().ExecuteAsync();


                // Extract email and other necessary details
                string email = userinfo.Email;

                if (string.IsNullOrEmpty(email))
                {
                    throw new InvalidOperationException("Failed to retrieve email from Google user info.");
                }
                Customer customer = await _customerService.GetCustomerByEmail(email);

                if (customer == null)
                {
                    throw new InvalidOperationException("Customer not found.");
                }


                return customer;
            }

            catch (ArgumentException ex)
            {
                throw new ArgumentException("Invalid Google access token.", ex);
            }

            catch (GoogleApiException gEx)
            {
                // Log or handle Google API specific errors
                throw new InvalidOperationException("Google API error while validating login.", gEx);
            }
            catch (Exception ex)
            {
                // Handle other unexpected errors
                throw new InvalidOperationException("Failed to validate Google login.", ex);
            }
        }

        public async Task<Customer> ValidateFacebookLogin(string token)
        {
            var fbClient = new FacebookClient(token);
            dynamic fbUser = await fbClient.GetTaskAsync("me?fields=email");

            // Extract email from the dynamic response safely
            string email = fbUser?.email;
            if (string.IsNullOrEmpty(email))
            {
                throw new InvalidOperationException("Failed to retrieve email from Facebook.");
            }

            return new Customer { Email = email };
        }


    }
}
