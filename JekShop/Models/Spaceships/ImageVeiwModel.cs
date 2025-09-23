namespace JekShop.Models.Spaceships
{
    public class ImageVeiwModel
    {
        public Guid ImageId { get; set; }
        public string? FilePath {  get; set; }
        public Guid? SpaceshipId { get; set; }

    }
}
