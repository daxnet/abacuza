using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Common.Utilities
{
    public static class Utils
    {
        public static TimeSpan ParseTimeSpanExpression(string expr, TimeSpan? defaultValue = null)
            => string.IsNullOrEmpty(expr) ? (defaultValue ?? TimeSpan.Zero) : expr[^1] switch
            {
                'm' => TimeSpan.FromMinutes(Convert.ToDouble(expr[0..^1])),
                's' => TimeSpan.FromSeconds(Convert.ToDouble(expr[0..^1])),
                'h' => TimeSpan.FromHours(Convert.ToDouble(expr[0..^1])),
                'f' => TimeSpan.FromMilliseconds(Convert.ToDouble(expr[0..^1])),
                _ => TimeSpan.FromMilliseconds(Convert.ToDouble(expr))
            };

    }
}

