namespace Libs.Entity
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FieldId { get; set; }
        public Field Field { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public List<Skill>? Skills { get; set; }
    }
}
