namespace School.Data.Entities
{
    public class BaseEntity
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)] // If You Need To Stop Auto Identity Increment
        public int Id { get; set; }
    }
}
