using Orchard;
using Q.SeoTools.Models;
using System;
using System.Collections.Generic;

namespace Q.SeoTools.Services
{

    public interface IRobotsService : IDependency
    {
        RobotsFileRecord Get();
        Tuple<bool, IEnumerable<string>> Save(string text);
    }
}