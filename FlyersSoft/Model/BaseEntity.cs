namespace FlyersSoft.Model
{
    public class BaseEntity
    {
        public BaseEntity()
        {
        }

        public bool isActive { get; set; }
        public int createdBy { get; set; }
        public DateTime createdOn { get; set; }
        public int updatedBy { get; set; }
        public DateTime updatedOn { get; set; }
    }
}
