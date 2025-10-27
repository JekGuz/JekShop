using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JekShop.Core.Dto
{
    public class ChuckJokeDto
    {
        public string? Id { get; set; }
        public string? Value { get; set; } // сама шутка
        public string? IconUrl { get; set; } // иконка
        public string? Url { get; set; } // ссылка на шутку
        public string? CreatedAt { get; set; } // дата создания
        public string? UpdatedAt { get; set; } // дата обновления
        public object[]? Categories { get; set; } // категории (чаще всего пустые)
    }
}
