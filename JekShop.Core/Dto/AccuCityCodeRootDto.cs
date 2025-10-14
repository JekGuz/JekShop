using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JekShop.Core.Dto
{
    public class AccuCityCodeRootDto
    {
        public string? Version { get; set; }
        public string? Key { get; set; }
        public string? Type { get; set; }
        public int? Rank { get; set; }
        public string? LocalizedName { get; set; }
        public string? EnglishName { get; set; }
        public string? PrimaryPostalCode { get; set; }
        public RegionCountryDto? Region { get; set; }
        public AdministrativeAreaDto? AdministrativeArea { get; set; }
        public TimeZoneDto? TimeZone { get; set; }
        public GeoPositionDto? GeoPosition { get; set; }
        public bool? IsAlias { get; set; }
        public SupplementalAdminAreasDto[]? SupplementalAdminAreas { get; set; }
        public string[]? DataSets { get; set; }

    }

    public class RegionCountryDto
    {
        public ValueDto? Region { get; set; }
        public ValueDto? Country { get; set; }
    }

    public class ValueDto
    {
        public string? ID { get; set; }
        public string? LocalizedName { get; set; }
        public string? EnglishName { get; set; }
    }
    
    public class AdministrativeAreaDto
    {
        public string? ID { get; set; }
        public string? LocalizedName { get; set; }
        public string? EnglishName { get; set; }
        public int? Level { get; set; }
        public string? LocalizedType { get; set; }
        public string? EnglishType { get; set; }
        public int? CountryID { get; set; }
    }

    public class TimeZoneDto
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public double? GmtOffset { get; set; }
        public bool? IsDaylightSaving { get; set; }
        public string? NextOffsetChange { get; set; }
    }

    public class GeoPositionDto
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Elevation { get; set; }
        public GeoPositionValueDto? ElevationValue { get; set; }
    }

    public class GeoPositionValueDto
    {
        public MetricImerialValueDto? Metric { get; set; }
        public MetricImerialValueDto? Imperial { get; set; }
    }

    public class MetricImerialValueDto
    {
        public double? Value { get; set; }
        public string? Unit { get; set; }
        public int? UnitType { get; set; }
    }

    public class SupplementalAdminAreasDto
    {
        public int? Level { get; set; }
        public string? LocalizedName { get; set; }
        public string? EnglishName { get; set; }

    }

}
