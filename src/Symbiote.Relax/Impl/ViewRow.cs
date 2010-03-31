using Newtonsoft.Json;

namespace Symbiote.Relax.Impl
{
    public class ViewRow<TModel>
        where TModel : class, ICouchDocument
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "doc")]
        public TModel Model { get; set; }
    }
}