using System;
using Microsoft.AspNetCore.Http;

namespace Tada.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            //response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
        }

        public static int CalculateAge(this DateTime date)
        {
            var age = DateTime.Today.Year - date.Year;
            if(date.AddYears(age) > DateTime.Today)
                age--;
            
            return age;
        }
    }
}