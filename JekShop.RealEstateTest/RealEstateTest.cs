namespace JekShop.RealEstateTest;

using System.Threading.Tasks;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;


public class RealEstateTest : TestBase
    {
        [Fact]
        public async Task Test1()
        {
            // Arrange
            RealEstateDto dto = new();

        dto.Area = 120.5;
        dto.Location = "Downtown";
        dto.RoomNumber = 3;
        dto.BuildingType = "Apartament";
        dto.CreateAt = DateTime.Now;
        dto.ModifiedAt = DateTime.Now;

        // Act 
        var result = await Svc<IRealEstateServices>().Create(dto);

        // Assert
        Assert.NotNull(result);

        }
    }
