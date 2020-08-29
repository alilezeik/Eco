namespace Eco
{
    using System.Text.Json.Serialization;

    public abstract class CreateRequest
    {
        [JsonIgnore]
        public string CreatedBy
        {
            get;
            set;
        }
    }
}
