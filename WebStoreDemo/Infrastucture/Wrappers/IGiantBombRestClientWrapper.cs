using GiantBomb.Api;

namespace WebStoreDemo.Infrastucture.Wrappers
{
    public interface IGiantBombRestClientWrapper
    {
        IGiantBombRestClient GetClient();
    }
}