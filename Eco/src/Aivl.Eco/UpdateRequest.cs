namespace Eco
{
    using System.Text.Json.Serialization;

    public abstract class UpdateRequest
    {
        [JsonIgnore]
        public string ModifiedBy
        {
            get;
            set;
        }
    }
}
