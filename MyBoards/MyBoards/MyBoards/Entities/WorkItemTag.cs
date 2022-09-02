namespace MyBoards.Entities
{
    public class WorkItemTag
    {
        public Tag Tag { get; set; }
        public int TagId { get; set; }
        public WorkItem WorkItem { get; set; }
        public int WorkItemId { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
