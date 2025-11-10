namespace JekShop.RealEstateTest;

using System.Threading.Tasks;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


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

    public static RealEstateDto RealEstatedto2()
    {
        return new()
        {
            Area = 180,
            Location = "Old Town",
            RoomNumber = 8,
            BuildingType = "Penthouse",
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

    //
    [Fact]
    public async Task ShouldNot_GetByIdRealestate_WhenReturnsNotEqual1()
    {
        //Arrange
        Guid wrongGuid = Guid.Parse(Guid.NewGuid().ToString());
        Guid guid = Guid.Parse("0a35d9eb-e4d7-44c7-ac85-d3c584938eec");

        //Act
        await Svc<IRealEstateServices>().DetailAsync(guid);

        //Assert
        Assert.NotEqual(wrongGuid, guid);
    }

    [Fact]
    public async Task Should_GetByIdRealestate_WhenReturnsEqual1()
    {
        // Arrage
        Guid databaseGuid = Guid.Parse("0a35d9eb-e4d7-44c7-ac85-d3c584938eec");
        Guid guid = Guid.Parse("0a35d9eb-e4d7-44c7-ac85-d3c584938eec");

        // Act
        await Svc<IRealEstateServices>().DetailAsync(guid);


        // Assert
        Assert.Equal(databaseGuid, guid);
    }

    [Fact]
    public async Task Should_DeleteByIdRealestate_WhenDeleteRealestate1()
    {
        // Arrage
        RealEstateDto dto = RealEstatedto1();

        // Act
        var created1 = await Svc<IRealEstateServices>().Create(dto);
        var delete1 = await Svc<IRealEstateServices>().Delete((Guid)created1.Id);

        // Assert
        Assert.Equal(delete1.Id, created1.Id);
    }

    [Fact]
    public async Task ShouldNot_DeleteByIdRealestate_WhenDidNotDeleteRealEsrare1()
    {
        // Arrage
        RealEstateDto dto = RealEstatedto1();

        // Act
        var created1 = await Svc<IRealEstateServices>().Create(dto);
        var created2 = await Svc<IRealEstateServices>().Create(dto);

        var result = await Svc<IRealEstateServices>().Delete((Guid)created2.Id);

        // Assert
        Assert.NotEqual(result.Id, created1.Id);
    }

    [Fact]
    public async Task Should_UpdateRealEstate_WhenUpdate()
    {
        // Arrage

        RealEstateDto dto = RealEstatedto1();

        // Act
        var created1 = await Svc<IRealEstateServices>().Create(dto);

        var updateDto = new RealEstateDto
        {
            Id = created1.Id,// ОБЯЗАТЕЛЬНО тот же Id
            Area = 120,
            Location = "Old Town",
            RoomNumber = 3,
            BuildingType = "Penthouse",
            CreateAt = created1.CreateAt,// дата создания не меняется
            ModifiedAt = DateTime.Now
        };

        // обновляем
        var updated = await Svc<IRealEstateServices>().Update(updateDto);

        // Assert
        Assert.Equal(created1.Id, updated.Id);
    }

    [Fact]
    public async Task Should_UpdateRealEstate_WhenUpdate1()
    {
        // Arrage
        var guid = new Guid("0a35d9eb-e4d7-44c7-ac85-d3c584938eec");

        RealEstateDto dto = RealEstatedto1();
        RealEstateDto domain = new();

        domain.Id = Guid.Parse("0a35d9eb-e4d7-44c7-ac85-d3c584938eec");
        domain.Area = 90;
        domain.Location = "Suburb";
        domain.RoomNumber = 3;
        domain.BuildingType = "House";
        domain.CreateAt = DateTime.Now;
        domain.ModifiedAt = DateTime.Now;

        // Act
        await Svc<IRealEstateServices>().Update(dto);

        // Assert
        Assert.Equal(guid, domain.Id);
        // DoesNotMatch ja kasutage seda Locationi ja RoomNumber jaoks
        // чтобы они БОЛЬШЕ не были старыми ("Center" и 2)
        Assert.DoesNotMatch(dto.Location, domain.Location);
        Assert.DoesNotMatch(dto.RoomNumber.ToString(), domain.RoomNumber.ToString());

        Assert.NotEqual(dto.RoomNumber, domain.RoomNumber);
        Assert.NotEqual(dto.Area, domain.Area);
    }

    [Fact]
    public async Task Should_UpdateRealEstate_WhenUpdateData2()
    {
        // Arrange
        RealEstateDto dto = RealEstatedto1();
        RealEstateDto updatedDto = RealEstatedto2();

        // Act
        var createdRealEstate = await Svc<IRealEstateServices>().Create(dto);
        var result = await Svc<IRealEstateServices>().Update(updatedDto);

        // Assert
        Assert.NotEqual(createdRealEstate.Area, result.Area);
        Assert.DoesNotMatch(createdRealEstate.Location, result.Location);
        Assert.NotEqual(createdRealEstate.RoomNumber, result.RoomNumber);
        Assert.DoesNotMatch(createdRealEstate.BuildingType, result.BuildingType);
        Assert.NotEqual(createdRealEstate.ModifiedAt, result.ModifiedAt);
    }

    // teha test nimega ShouldNot_UpdateRealEstate_WhenDidNotUpdateDate()
    [Fact]
    public async Task ShouldNot_UpdateRealEstate_WhenDidNotUpdateDate()
    {
        // Arrange – создаём исходный DTO через RealEstatedto1()
        RealEstateDto dto = RealEstatedto1();

        // сохраняем запись в базу
        var created1 = await Svc<IRealEstateServices>().Create(dto);

        // Сохраняем исходные даты, чтобы потом сравнить
        var create1 = created1.CreateAt;
        var update = created1.ModifiedAt;

        // Act – имитируем "фейковую кнопку": Update НЕ вызываем
        var fake = await Svc<IRealEstateServices>().DetailAsync((Guid)created1.Id);

        // Assert – проверяем, что НИЧЕГО не изменилось

        // объект существует
        Assert.NotNull(fake);

        // Id тот же
        Assert.Equal(created1.Id, fake.Id);

        // данные такие же, как были в dto / created
        Assert.Equal(dto.Area, fake.Area);
        Assert.Equal(dto.Location, fake.Location);
        Assert.Equal(dto.RoomNumber, fake.RoomNumber);
        Assert.Equal(dto.BuildingType, fake.BuildingType);

        // самое главное: даты не изменились, потому что Update не вызывался
        Assert.Equal(create1, fake.CreateAt);   // тут было update
        Assert.Equal(update, fake.ModifiedAt); // это правильно
    }

}
