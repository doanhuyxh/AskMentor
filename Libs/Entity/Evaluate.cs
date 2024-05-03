namespace Libs.Entity
{
    public class Evaluate
    {
        public int Id { get; set; }
        public string NameLeaner { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public string MyProperty { get; set; }
        public int Rating { get; set; }
        public ApplicationUser UserId { get; set; }
    }
}
