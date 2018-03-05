using GiantBomb.Api;
using Microsoft.Extensions.Options;
using WebStoreDemo.Infrastucture.Settings;

namespace WebStoreDemo.Infrastucture.Wrappers
{
    public class GiantBombRestClientWrapper : IGiantBombRestClientWrapper
    {
        private readonly GiantBombSettings _giantBombSettings;

        public GiantBombRestClientWrapper(IOptions<GiantBombSettings> giantBombSettings)
        {
            this._giantBombSettings = giantBombSettings.Value;
        }

        public IGiantBombRestClient GetClient()
        {
            return new GiantBombRestClient(this._giantBombSettings.ApiKey);
        }
    }
}