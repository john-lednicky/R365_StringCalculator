using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculator.Services
{
    public class Messages
    {
        private static Dictionary<string, string> messages = new Dictionary<string, string>();

        static Messages()
        {
            messages.Add("MoreThanTwoNumbers", "Input can only contain two numbers.");
        }

        public string this[string key]
        {
            get
            {
                if (messages.TryGetValue(key, out string? value))
                    return value;
                else
                    throw new KeyNotFoundException($"Key '{key}' not found in Messages dictionary.");
            }
        }
    }
}
