using System;
using System.Linq;

namespace Fluctuation.Extensions {
    public static class Extensions {
        public static string GetParameterValue(this string[] args, string parameterName, char parameterPrefix = '-') {
            return string.Join(":", args)
                .Split(parameterPrefix).Select(x => x.Split(':'))
                .Where(x => x.Length > 1)
                .Where(x => x.First().Equals(parameterName, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Skip(1).First())
                .FirstOrDefault();
        }
    }
}
