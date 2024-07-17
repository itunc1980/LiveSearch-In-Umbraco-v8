using Searching.Site.Services;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Searching.Site.Composing
{
    public class RegisterServiceComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register<ISearchService, SearchService>(Lifetime.Singleton);            
        }
    }
}
