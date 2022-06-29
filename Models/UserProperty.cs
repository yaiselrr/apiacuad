namespace API.Models
{
    public class UserProperty
    {
        public string GeneralDataUserId { get; set; }
        public string PropertyId { get; set; }
        public GeneralDataUser GeneralDataUser { get; set; }
        public Property Property { get; set; }

    }
}
