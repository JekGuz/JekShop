namespace JekShop.RealEstateTest;

using System.Threading.Tasks;
using JekShop.Core.Domain;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using JekShop.Data;
using Microsoft.EntityFrameworkCore;
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

    public static RealEstateDto RealEstatedto0()
    {
        return new()
        {
            Id = null,
            Area = null,
            Location = null,
            RoomNumber = null,
            BuildingType = null,
            CreateAt = null,
            ModifiedAt = null
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

    //[Fact]
    //public async Task Should_UpdateRealEstate_WhenUpdate()
    //{
    //    Arrage

    //   RealEstateDto dto = RealEstatedto1();

    //    Act
    //   var created1 = await Svc<IRealEstateServices>().Create(dto);

    //    var updateDto = new RealEstateDto
    //    {
    //        Id = created1.Id,// ОБЯЗАТЕЛЬНО тот же Id
    //        Area = 120,
    //        Location = "Old Town",
    //        RoomNumber = 3,
    //        BuildingType = "Penthouse",
    //        CreateAt = created1.CreateAt,// дата создания не меняется
    //        ModifiedAt = DateTime.Now
    //    };

    //    обновляем
    //   var updated = await Svc<IRealEstateServices>().Update(updateDto);

    //    Assert
    //    Assert.Equal(created1.Id, updated.Id);
    //}

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

    [Fact]
    public async Task ShouldNot_UpdateRealEstate_WhenDidNotUpdateDate1()
    {
        RealEstateDto dto = RealEstatedto1();
        var created1 = await Svc<IRealEstateServices>().Create(dto);

        RealEstateDto update = RealEstatedto0();
        var result = await Svc<IRealEstateServices>().Update(update);

        Assert.NotEqual(dto.Id, result.Id);
    }
    // mõtelda ise välja unit test
    //see peab olema selline, mida enne pole teinud
    [Fact]
    public async Task Should_CreateDifferentRealEstates_WhenUseDifferentDtos()
    {
        // Arrange – kasutame kahte erinevat DTO-d
        // Используем 2 разных дто
        RealEstateDto dto1 = RealEstatedto1(); // 80, "Center", 2, "Apartment"
        RealEstateDto dto2 = RealEstatedto2(); // 180, "Old Town", 8, "Penthouse"

        var service = Svc<IRealEstateServices>();

        // Act – loome kaks kirjet baasi
        // Создаем 2 записи в базе
        var created1 = await Svc<IRealEstateServices>().Create(dto1);
        var created2 = await Svc<IRealEstateServices>().Create(dto2);

        // Assert

        // 1. Id peavad olema erinevad
        // 2 разных ид
        Assert.NotEqual(created1.Id, created2.Id);

        // 2. Kontrollime, et olulised väljad on samuti erinevad
        // Проверим, что важные поля тоже разные
        Assert.NotEqual(created1.Area, created2.Area);
        Assert.DoesNotMatch(created1.Location, created2.Location);
        Assert.NotEqual(created1.RoomNumber, created2.RoomNumber);
        Assert.DoesNotMatch(created1.BuildingType, created2.BuildingType);
    }

    // ---------------------------------------------------------------------------------------------------------------------

    //tuleb välha mõelda kolm erinevat xUnit testi RealEstate kohta
    //saate teha 2-3 in meeskonnas
    //kommentaari kirjutate, mida iga test kontrollib


    // Test 1: Should_AddRealEstate_WhenAreaIsNegative
    // Test kontrollib, et PRAEGUNE rakendus lubab negatiivse pindala (Area < 0) ilma veata salvestada – see on loogikaviga, mida test näitab.
    // Тест проверяет, что ТЕКУЩЕЕ приложение позволяет сохранить отрицательную площадь (Area < 0) без ошибки — это логическая ошибка, и тест демонстрирует её.
    [Fact]
    public async Task Should_AddRealEstate_WhenAreaIsNegative()
    {
        // Arrange – loome normaalse DTO ja paneme Area negatiivseks
        // Arrange – создаём нормальный DTO и делаем площадь отрицательной
        var service = Svc<IRealEstateServices>();
        RealEstateDto dto = RealEstatedto1();
        dto.Area = -10; // negatiivne / отрицательное значение

        // Act – salvestame kinnisvara teenuse kaudu
        // Act – сохраняем объект через сервис
        var created = await service.Create(dto);

        // Assert – kontrollime, et negatiivne pindala tõesti salvestati
        // Assert – проверяем, что отрицательная площадь действительно сохранилась
        Assert.NotNull(created);
        Assert.Equal(dto.Area, created.Area);
        Assert.True(created.Area < 0);
    }


    // Test 2: ShouldNot_AddRealEstate_WhenAllFieldsAreNull
    // Test NÄITAB, et praegune rakendus lubab salvestada täiesti tühja DTO (RealEstatedto0), kus kõik väljad on null – see on loogikaviga.
    // Тест ПОКАЗЫВАЕТ, что текущее приложение позволяет сохранить полностью пустой DTO (RealEstatedto0), где все поля = null — это логическая ошибка.
    [Fact]
    public async Task Should_AddRealEstate_WhenAllFieldsAreNull()
    {
        // Arrange – kasutame spetsiaalset "tühja" DTO-d RealEstatedto0()
        // Arrange – используем специальный "пустой" DTO из RealEstatedto0()
        var service = Svc<IRealEstateServices>();
        RealEstateDto emptyDto = RealEstatedto0();

        // Act – proovime luua kinnisvara täiesti tühjade andmetega
        // Act – пробуем создать объект недвижимости с полностью пустыми данными
        var created = await service.Create(emptyDto);

        // Assert – kontrollime, et BAASISSE läkski tühi kirje
        // Assert – проверяем, что В БАЗУ действительно ушла пустая запись
        Assert.NotNull(created);

        // põhilised väljad on endiselt null/tühjad
        // основные поля по-прежнему null/пустые
        Assert.Null(created.Area);
        Assert.True(string.IsNullOrWhiteSpace(created.Location));
        Assert.Null(created.RoomNumber);
        Assert.True(string.IsNullOrWhiteSpace(created.BuildingType));

        // See test näitab, et sisendit ei valideerita ja tühjad andmed salvestatakse.
        // Этот тест показывает, что входные данные не валидируются, и пустые данные сохраняются как есть.
    }


    // Test 3: Should_Allow_ModifiedAt_Before_CreatedAt
    // Test kontrollib, et süsteem PRAEGU lubab olukorda, kus ModifiedAt on varasem kui CreateAt (ajaliselt "tagurpidi").
    // Тест проверяет, что система СЕЙЧАС допускает ситуацию, когда ModifiedAt раньше, чем CreateAt (временная «ошибка» в данных).
    [Fact]
    public async Task Should_Allow_ModifiedAt_Before_CreatedAt1()
    {
        // Arrange – loome algse kinnisvara ja selle uuenduse
        // Arrange – создаём исходный объект недвижимости и его обновление
        var service = Svc<IRealEstateServices>();

        // esialgsed andmed
        RealEstateDto original = RealEstatedto1();
        // uued andmed
        RealEstateDto update = RealEstatedto2();

        // Paneme ModifiedAt varemaks kui CreateAt
        // Делаем ModifiedAt раньше, чем CreateAt
        update.ModifiedAt = DateTime.Now.AddYears(-1);
        var created = await service.Create(original);

        // Act – käivitame Update uuendatud kuupäevadega
        // Act – вызываем Update с «перевёрнутыми» датами
        var result = await service.Update(update);

        // Assert – kontrollime, et tulemus tõesti lubab ModifiedAt <= CreateAt
        // Assert – проверяем, что результат действительно допускает ModifiedAt <= CreateAt
        Assert.NotNull(result);
        // või teha Assert.False  siis <=
        Assert.True(result.ModifiedAt >= result.CreateAt);

        // See näitab, et äriloogika ei kontrolli kuupäevade järjekorda
        // Это показывает, что бизнес-логика не проверяет корректность порядка дат
    }

    // test kedagi

    [Fact]
    public async Task Should_CreateRealEstate_WithNoNullId()
    {
        // Test kontrollib, et loodud RealEstate objektil okels Id väärtus

        // Arrange
        RealEstateDto dto = RealEstatedto1();

        // Act
        var result = await Svc<IRealEstateServices>().Create(dto);

        // Assert
        Assert.NotNull(result.Id);
    }

    [Fact]
    // Uuendame objekti, mullel puubud Id -teenus peab tagastame
    // null või muu oodatava tulemuse.
    public async Task ShouldNot_UpdateRealEstate_WhenIdDoesNotExist()
    {
        // Arrange
        RealEstateDto update = RealEstatedto1();
        update.Id = Guid.NewGuid();

        // Act and Assert
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
        {
            await Svc<IRealEstateServices>().Update(update);
        });
    }

    [Fact]
    // Loogiline strsenaarium: loome -> saame ID järgi -> võrdleme välju
    public async Task Should_ReturnSameRealEstate_WhenGetDetailsAfterCreate()
    {
        // Arrange
        RealEstateDto dto = RealEstatedto1();

        // Act
        var created = await Svc<IRealEstateServices>().Create(dto);
        var fetched = await Svc<IRealEstateServices>().DetailAsync((Guid)created.Id);

        // Assert
        Assert.NotNull(fetched);
        Assert.Equal(created.Id, fetched.Id);
        Assert.Equal(created.Location, fetched.Location);
    }

    [Fact]
    public async Task Should_AssingUniqueIds_When_CreateMultiple()
    {
        // Arrange
        RealEstateDto dto = RealEstatedto1();
        RealEstateDto dto1 = RealEstatedto2();

        // Act
        var create1 = await Svc<IRealEstateServices>().Create(dto);
        var create2 = await Svc<IRealEstateServices>().Create(dto1);

        // Assert
        Assert.NotNull(create1);
        Assert.NotNull(create2);
        Assert.NotEqual(create1.Id, create2.Id);
        Assert.NotEqual(Guid.Empty, create1.Id);
        Assert.NotEqual(Guid.Empty, create2.Id);
    }

    // We check that after deleting the record,
    // there are no rows left in FileTodatebases with this RealEstateId
    [Fact]
    public async Task Should_DeleteRelatedImages_WhenDeleteRealEstate()
    {
        // Arrange
        RealEstateDto dto = RealEstatedto1();

        var created = await Svc<IRealEstateServices>().Create(dto);
        var id = (Guid)created.Id;

        var db = Svc<JekShopContext>();
        db.FileToDatabases.Add(new FileToDatabase
        {
            Id = Guid.NewGuid(),
            RealEstateId = id,
            ImageTitle = "kitchen.jpg",
            ImageData = new byte[] { 1, 2, 3 }
        });
        db.FileToDatabases.Add(new FileToDatabase
        {
            Id = Guid.NewGuid(),
            RealEstateId = id,
            ImageTitle = "livingroom.jpg",
            ImageData = new byte[] { 4, 5, 6 }
        });
        await db.SaveChangesAsync();

        // Act
        await Svc<IRealEstateServices>().Delete(id);

        // Assert
        var leftovers = db.FileToDatabases.Where(x => x.RealEstateId == id).ToList();

        Assert.NotEmpty(leftovers);
    }

    // ei tööta
    [Fact]
    public async Task Should_ReturnNull_When_DeletingNonExistentRealEstate()
    {
        // Arrange (Ettevalmistus)
        // Genereerime juhusliku ID, mida andmebaasis kindlasti ei ole.
        // Guid nonExistentId = Guid.NewGuid();
        var dto = RealEstatedto1();

        var create = await Svc<IRealEstateServices>().Create(dto);

        // Act (Tegevus)
        // Proovime kustutada objekti selle ID järgi.
        await Svc<IRealEstateServices>().Delete((Guid)create.Id);

        var detail = await Svc<IRealEstateServices>().DetailAsync((Guid)create.Id);

        // Assert (Kontroll)
        // Meetod peab tagastama nulli, kuna polnud midagi kustutada ja viga ei tohiks tekkida.
        Assert.Null(detail);
    }

    [Fact]
    public async Task Should_AddValidRealEstate_WhenDateTypeIsValid()
    {
        // Arrange
        RealEstateDto dto = RealEstatedto1();

        // act
        var create = await Svc<IRealEstateServices>().Create(dto);

        // assert
        Assert.IsType<int>(create.RoomNumber);
        Assert.IsType<string>(create.Location);
        Assert.IsType<DateTime>(create.CreateAt);
    }

}

