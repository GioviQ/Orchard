using Orchard;
using Orchard.Environment;
using Q.SeoTools.Services;

namespace Q.SeoTools.Routes
{
    public interface ICustomAliasConstraintUpdator : IDependency
    {
        void Refresh();
    }

    public class CustomAliasConstraintUpdator : ICustomAliasConstraintUpdator, IOrchardShellEvents
    {
        private readonly ICustomAliasConstraint _customAliasSlugConstraint;
        private readonly ICustomAliasService _customAliasService;

        public CustomAliasConstraintUpdator(ICustomAliasConstraint customAliasSlugConstraint,
                                              ICustomAliasService customAliasService)
        {
            _customAliasSlugConstraint = customAliasSlugConstraint;
            _customAliasService = customAliasService;
        }

        void IOrchardShellEvents.Activated()
        {
            Refresh();
        }

        void IOrchardShellEvents.Terminating()
        {
        }

        public void Refresh()
        {
            _customAliasSlugConstraint.SetAlias(_customAliasService.GetAliases());
        }
    }
}