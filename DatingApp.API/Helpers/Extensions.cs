using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static int CalculateAge(this DateTime theDateTime)
        {
            var age = DateTime.Today.Year - theDateTime.Year;
            if (theDateTime.AddYears(age) > DateTime.Today)
                age--;

            return age;
        }

        public static string ToDisplay(this List<string> list, string separator = ", ")
        {
            if (list.Count == 0)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append(list[0]);
            for (int i = 1; i < list.Count; i++)
            {
                sb.Append(string.Format("{0}{1}", separator, list[i]));
            }
            return sb.ToString();
        }

        public static string ToDisplay(this List<string> list, string exclude, string separator = ", ")
        {
            List<string> dump = new List<string>();
            foreach (var item in list)
            {
                if (item == exclude) continue;
                dump.Add(item.ToString());
            }
            return dump.ToDisplay();
        }

        public static string ToPercentString(this object item)
        {
            return item.ToString() + " %";
        }
    }
}