using System.Text.Json.Serialization;

namespace Blog.API.Common.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GarAddressLevel
{
    Region = 1,
    AdministrativeArea,
    MunicipalArea,
    RuralUrbanSettlement,
    City,
    Locality,
    ElementOfPlanningStructure,
    ElementOfRoadNetwork,
    Land,
    Building,
    Room,
    RoomInRooms,
    AutonomousRegionLevel,
    IntracityLevel,
    AdditionalTerritoriesLevel,
    LevelOfObjectsInAdditionalTerritories,
    CarPlace
}