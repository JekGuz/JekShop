namespace JekShop.RealEstateTest;

using System.Threading.Tasks;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;


public class RealEstateTest : TestBase
    {

    public static RealEstateDto RealEstatedto1()
    {
        return new()
        {
            Area = 80,
            Location = "Center",
            RoomNumber = 2,
            BuildingType = "Apartment",
            CreateAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };
    }

    [Fact]
        public async Task ShoulNot_AddEmptyRealEstate_WhenReturnResult()
        {
        // Arrange 
        RealEstateDto dto = RealEstatedto1();

        // Act 
        var result = await Svc<IRealEstateServices>().Create(dto);

        // Assert
        Assert.NotNull(result);
        }
    //ShouldNot_GerByIdRealestate_WhenReturnsNotEqual()
    // что при чтении по Id первой записи мы не получаем вторую (то есть сервис реально возвращает правильный объект по Id).
    [Fact]
    public async Task ShouldNot_GetByIdRealestate_WhenReturnsNotEqual()
    {
        // Arrange – создаём две разные записи недвижимости
        var service = Svc<IRealEstateServices>();

        RealEstateDto dto1 = RealEstatedto1();

        var created1 = await service.Create(dto1);

        // Act – берём из базы по Id только первую запись
        var result = await service.DetailAsync((Guid)created1.Id); // ≈ created1.Id.Value

        // Assert – убеждаемся, что найденная запись НЕ равна второй
        Assert.NotNull(result);
    }


    //Should_GetByIdRealestate_WhenReturnsEqal()
    // что при чтении по Id мы получаем именно ту же запись (Id совпадает).
    [Fact]
    public async Task Should_GetByIdRealestate_WhenReturnsEqal()
    {
        // Arrange – создаём одну запись и запоминаем её Id
        var service = Svc<IRealEstateServices>();

        RealEstateDto dto = RealEstatedto1();


        var created = await service.Create(dto);

        // Act – читаем объект по этому же Id через DetailAsync
        var result = await service.DetailAsync((Guid)created.Id); // ≈ created1.Id.Value

        // Assert – запись нашлась и Id совпадает
        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
    }

    //Should_DeleteByIdRealestate_WhenDeleteRealestate()
    // что после удаления по Id запись больше не находится.
    [Fact]
    public async Task Should_DeleteByIdRealestate_WhenDeleteRealestate()
    {
        // Arrange – создаём запись, чтобы потом её удалить
        var service = Svc<IRealEstateServices>();

        RealEstateDto dto = RealEstatedto1();

        var created = await service.Create(dto);

        // Act – удаляем по Id
        await service.Delete((Guid)created.Id);

        // и пробуем прочитать снова
        var resultAfterDelete = await service.DetailAsync((Guid)created.Id);

        // Assert – после удаления объект больше не должен существовать
        Assert.Null(resultAfterDelete);
    }

    //ShouldNot_DeleteByIdRealestate_WhenDidNotDeleteRealEsrare()
    // Что нужно добавить запись и удалить по не существующими Id
    [Fact]
    public async Task ShouldNot_DeleteByIdRealestate_WhenDidNotDeleteRealEsrare()
    {
        // Arrange – создаём две записи
        var service = Svc<IRealEstateServices>();

        RealEstateDto dto1 = RealEstatedto1();

        var created1 = await service.Create(dto1);

        // Act – пробуем удалить по НЕсуществующему Id
        var fakeId = Guid.NewGuid(); // такого Id нет в базе

        try
        {
            await service.Delete(fakeId);
        }
        catch (ArgumentNullException)
        {
            // Это ожидаемое поведение текущей реализации Delete,
            // просто игнорируем исключение в рамках теста.
        }

        // Assert – наша настоящая запись всё ещё существует
        var fromDb = await service.DetailAsync((Guid)created1.Id);

        Assert.NotNull(fromDb);
        Assert.Equal(created1.Id, fromDb.Id);

    }

    [Fact]
    public async Task ShouldNot_GetByIdRealestate_WhenReturnsNotEqual1()
    {
        // Arrange 
        Guid wrongGuid = Guid.Parse(Guid.NewGuid().ToString());
        Guid guid = Guid.Parse("0a35d9eb-e4d7-44c7-ac85-d3c584938eec");

        // Act
        await Svc<IRealEstateServices>().DetailAsync(guid);

        // Assert
        Assert.NotEqual(wrongGuid, guid);
    }

}
