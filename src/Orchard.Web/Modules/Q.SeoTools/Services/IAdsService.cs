using Orchard;
using Q.SeoTools.Models;
using System;
using System.Collections.Generic;

namespace Q.SeoTools.Services
{

    public interface IAdsService : IDependency
    {
        AdsFileRecord Get();
        Tuple<bool, IEnumerable<string>> Save(string text);
    }
}