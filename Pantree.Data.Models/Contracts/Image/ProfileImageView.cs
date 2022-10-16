namespace Pantree.Data.Models.Contracts
{
    public class ProfileImageView
    {
        public int ProfileImageID { get; set; }
        public byte[] ProfileImage { get; set; }
        public string AlternativeText { get; set; }
    }
}