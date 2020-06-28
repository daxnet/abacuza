using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Common.Utilities
{
    public static class Extensions
    {
        public static string Pluralize(this string word, bool inputIsKnownToBeSingular = true)
        {
            return Vocabularies.Default.Pluralize(word, inputIsKnownToBeSingular);
        }
    }
}
