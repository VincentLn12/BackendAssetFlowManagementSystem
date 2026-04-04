namespace Core.Entities
{
    public class CartStorage
    {
        public required string Id { get; set; }
        public required string JsonData { get; set; }  // บันทึก JSON เป็น string
    }
}
