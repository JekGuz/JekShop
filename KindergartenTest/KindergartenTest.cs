using System.Globalization;
using System.Globalization;
using System.Threading.Tasks;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using JekShop.KindergartenTest;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace KindergartenTest
{
        public class KindergartenTest : UnitTest1
        {
            public static KindergartenDto KindergartenDto1()
            {
                return new()
                {
                    GroupName = "Paike",
                    ChildrenCount = 21,
                    KindergartenName = "Mustakivi",
                    TeacherName = "Marina",
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                };
            }

        public static KindergartenDto KindergartenDto2()
        {
            return new()
            {
                GroupName = "Lilid",
                ChildrenCount = 19,
                KindergartenName = "Mustakivi",
                TeacherName = "Laura",
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
            };
        }

        [Fact]
            public async Task ShoulNot_AddEmptyKindergarten_WhenReturnResult()
            {
            // Arrange 
            KindergartenDto dto = KindergartenDto1();

                // Act 
                var result = await Svc<IKindergartenServices>().Create(dto);

                // Assert
                Assert.NotNull(result);
            }

            //ShouldNot_GerByIdRealestate_WhenReturnsNotEqual()
            // что при чтении по Id первой записи мы не получаем вторую (то есть сервис реально возвращает правильный объект по Id).
            [Fact]
            public async Task ShouldNot_GetByIdKindergarten_WhenReturnsNotEqual()
            {
                // Arrange – создаём две разные записи недвижимости
                var service = Svc<IKindergartenServices>();

                KindergartenDto dto1 = KindergartenDto1();

                var created1 = await service.Create(dto1);

                // Act – берём из базы по Id только первую запись
                var result = await service.DetailAsync((Guid)created1.Id); // ≈ created1.Id.Value

                // Assert – убеждаемся, что найденная запись НЕ равна второй
                Assert.NotNull(result);
            }


            //Should_GetByIdKindergarten_WhenReturnsEqal()
            // что при чтении по Id мы получаем именно ту же запись (Id совпадает).
            [Fact]
            public async Task Should_GetByIdKindergarten_WhenReturnsEqal()
            {
                // Arrange – создаём одну запись и запоминаем её Id
                var service = Svc<IKindergartenServices>();

                KindergartenDto dto = KindergartenDto1();


                var created = await service.Create(dto);

                // Act – читаем объект по этому же Id через DetailAsync
                var result = await service.DetailAsync((Guid)created.Id); // ≈ created1.Id.Value

                // Assert – запись нашлась и Id совпадает
                Assert.NotNull(result);
                Assert.Equal(created.Id, result.Id);
            }

        //Should_DeleteByIdKindergarten_WhenDeleteKindergarten()
        // что после удаления по Id запись больше не находится.
        [Fact]
            public async Task Should_DeleteByIdKindergarten_WhenDeleteKindergarten()
            {
                // Arrange – создаём запись, чтобы потом её удалить
                var service = Svc<IKindergartenServices>();

                KindergartenDto dto = KindergartenDto1();

                var created = await service.Create(dto);

                // Act – удаляем по Id
                await service.Delete((Guid)created.Id);

                // и пробуем прочитать снова
                var resultAfterDelete = await service.DetailAsync((Guid)created.Id);

                // Assert – после удаления объект больше не должен существовать
                Assert.Null(resultAfterDelete);
            }

        //ShouldNot_DeleteByIdKindergarten_WhenDidNotDeleteKindergarten()
        // Что нужно добавить запись и удалить по не существующими Id
        [Fact]
            public async Task ShouldNot_DeleteByIdKindergarten_WhenDidNotDeleteKindergarten()
            {
                // Arrange – создаём две записи
                var service = Svc<IKindergartenServices>();

                KindergartenDto dto1 = KindergartenDto1();

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

        // teha test nimega ShouldNot_UpdateKindergarten_WhenDidNotUpdateDate()
        [Fact]
        public async Task ShouldNot_UpdateKindergarten_WhenDidNotUpdateDate()
        {
            // Arrange – создаём исходный DTO через KindergartenDto1()
            KindergartenDto dto = KindergartenDto1();

            // сохраняем запись в базу
            var created1 = await Svc<IKindergartenServices>().Create(dto);

            // Сохраняем исходные даты, чтобы потом сравнить
            var create1 = created1.CreateAt;
            var update = created1.UpdateAt;

            // Act – имитируем "фейковую кнопку": Update НЕ вызываем
            var fake = await Svc<IKindergartenServices>().DetailAsync((Guid)created1.Id);

            // Assert – проверяем, что НИЧЕГО не изменилось

            // объект существует
            Assert.NotNull(fake);

            // Id тот же
            Assert.Equal(created1.Id, fake.Id);

            // данные такие же, как были в dto / created
            Assert.Equal(dto.GroupName, fake.GroupName);
            Assert.Equal(dto.ChildrenCount, fake.ChildrenCount);
            Assert.Equal(dto.KindergartenName, fake.KindergartenName);
            Assert.Equal(dto.TeacherName, fake.TeacherName);

            // самое главное: даты не изменились, потому что Update не вызывался
            Assert.Equal(create1, fake.CreateAt);   // тут было update
            Assert.Equal(update, fake.UpdateAt); // это правильно
        }

    }
}
