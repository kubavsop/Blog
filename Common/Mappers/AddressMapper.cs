using Blog.API.Common.Enums;
using Blog.API.Controllers.Dto.Responses;
using Blog.API.Entities.Database;

namespace Blog.API.Common.Mappers;

internal static class  AddressMapper
{

    public static IEnumerable<SearchAddressDto> AddressesToAddressesDto(IEnumerable<AddressElement> addressElements)
    {
        return addressElements.Select(AddressToAddressDto);
    }
    
    private static SearchAddressDto AddressToAddressDto(AddressElement address)
    {
        return new SearchAddressDto
        {
            ObjectId = address.ObjectId,
            ObjectGuid = address.ObjectGuid,
            Text = address.Text,
            ObjectLevel = address.ObjectLevel,
            ObjectLevelText = ObjectLevelToString(address.ObjectLevel)
        };
    }

    private static string ObjectLevelToString(GarAddressLevel expression)
    {
        return expression switch
        {
            GarAddressLevel.Region => "Субъект РФ",
            GarAddressLevel.AdministrativeArea => "Административный район",
            GarAddressLevel.MunicipalArea => "Муниципальный район",
            GarAddressLevel.RuralUrbanSettlement => "Сельское/городское поселение",
            GarAddressLevel.City => "Город",
            GarAddressLevel.Locality => "Населенный пункт",
            GarAddressLevel.ElementOfPlanningStructure => "Элемент планировочной структуры",
            GarAddressLevel.ElementOfRoadNetwork => "Элемент улично-дорожной сети",
            GarAddressLevel.Land => "Земельный участок",
            GarAddressLevel.Building => "Здание (сооружение)",
            GarAddressLevel.Room => "Помещение",
            GarAddressLevel.RoomInRooms => "Помещения в пределах помещения",
            GarAddressLevel.AutonomousRegionLevel => "Уровень автономного округа (устаревшее)",
            GarAddressLevel.IntracityLevel => "Уровень внутригородской территории (устаревшее)",
            GarAddressLevel.AdditionalTerritoriesLevel => "Уровень дополнительных территорий (устаревшее)",
            GarAddressLevel.LevelOfObjectsInAdditionalTerritories => "Уровень объектов на дополнительных территориях (устаревшее)",
            GarAddressLevel.CarPlace => "Машино-место",
            _ => throw new ArgumentOutOfRangeException(nameof(expression), expression, "Invalid address level.")
        };
    }
}