using AutoMapper;
using BRMSAPI.Domain;
using BRMSAPI.Model;
using System.Globalization;

namespace Backend.Utility;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {

        CreateMap<LocationVM, Location>()

            .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.Id))

            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))

            .ForMember(dest => dest.LCDA, opt => opt.MapFrom(src => src.LocalCoucilArea))

            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));




        //  CreateMap<RegLocationObj, LocationObj>();


        //  //Propaply for listing but will check...
        //  CreateMap<LocationObj, EditLocationObj>()
        //      .ForMember(dest => dest.RegLocationObjId, opt => opt.MapFrom(src => src.Id));

        //  //Used in a nested Mapping
        //  CreateMap<LocationVM, LocationObj>()
        //   .ForMember(dest => dest.VisibleLandmark, opt => opt.MapFrom(src => src.Landmark.Trim()))
        //      .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.LocationDescription.Trim()));




        //  CreateMap<LocationVM, EditLocationObj>()
        //      .ForMember(dest => dest.RegLocationObjId, opt => opt.MapFrom(src => src.Id))
        //      .ForMember(dest => dest.LocationTitle, opt => opt.MapFrom(src => src.LocationTitle.Trim()))
        //      .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.LocationDescription.Trim()))
        //      .ForMember(dest => dest.VisibleLandmark, opt => opt.MapFrom(src => src.Landmark.Trim()));


        //  CreateMap<EditLocationObj, LocationObj>()
        //.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RegLocationObjId))
        //;


        //  //PickPoint
        //  CreateMap<PickPointVM, RegPickPointObj>()
        //      .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusVal))
        //      ;



        //  CreateMap<PickPointVM, RegPickPointObj>()
        //      .AfterMap((src, dest) =>
        //      {
        //          // Apply ConvertFirstLetterToUpper() to each string property of RegPickPointObj
        //          foreach (var propertyInfo in typeof(RegPickPointObj).GetProperties())
        //          {
        //              if (propertyInfo.PropertyType == typeof(string))
        //              {
        //                  string value = (string)propertyInfo.GetValue(dest);
        //                  if (!string.IsNullOrEmpty(value))
        //                  {
        //                      propertyInfo.SetValue(dest, ConvertFirstLetterToUpper(value));
        //                  }
        //              }
        //          }
        //      });





        //  CreateMap<RegPickPointObj, PickPointObj>();


        //  CreateMap<EditPickPointObj, PickPointObj>()
        //       .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.Location.Id))
        //       .ForMember(dest => dest.RegPickPointObjId, opt => opt.MapFrom(src => src.RegPickPointObjId))
        //       .ForMember(dest => dest.RegCode, opt => opt.MapFrom(src => src.Code));


        //  CreateMap<PickPointObj, EditPickPointObj>()
        //  .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.Location.Id))
        //  .ForMember(dest => dest.RegPickPointObjId, opt => opt.MapFrom(src => src.RegPickPointObjId))
        //   .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.RegCode));


        //  CreateMap<RegRouteObj, RouteObj>()
        //      .ForMember(dest => dest.StartPoint, opt => opt.MapFrom(src => src.Start))
        //      .ForMember(dest => dest.EndPoint, opt => opt.MapFrom(src => src.Stop))
        //       //.ForMember(dest => dest.LCDA, opt => opt.MapFrom(src => src.LcdaId))
        //       ;

        //  CreateMap<EditRouteObj, RouteObj>()
        //    .ForMember(dest => dest.StartPoint, opt => opt.MapFrom(src => src.Start))
        //    .ForMember(dest => dest.EndPoint, opt => opt.MapFrom(src => src.Stop))
        //    .ForMember(dest => dest.RouteId, opt => opt.MapFrom(src => src.RegRouteId));


        //  CreateMap<RouteObj, EditRouteObj>()
        //    .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.StartPoint))
        //    .ForMember(dest => dest.Stop, opt => opt.MapFrom(src => src.EndPoint))
        //    .ForMember(dest => dest.RegRouteId, opt => opt.MapFrom(src => src.RouteId))
        //    .ForMember(dest => dest.PickPointIds, opt => opt.MapFrom(src => src.PickPointIds));
        //  //.ForMember(dest => dest.PickPoints, opt => opt.MapFrom(src => src.PickPoints));


        //  CreateMap<RegScheduleObj, ScheduleObj>()
        //  .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src => src.ArrivalTime))
        //  .ForMember(dest => dest.DepertureTime, opt => opt.MapFrom(src => src.DepertureTime))
        //  .ForMember(dest => dest.PickPoint, opt => opt.MapFrom(src => src.PickPoint))
        //  .ForMember(dest => dest.Route, opt => opt.MapFrom(src => src.Route))
        //  .ForMember(dest => dest.PickPointId, opt => opt.MapFrom(src => src.PickPointId));


        //  CreateMap<EditScheduleObj, ScheduleObj>()
        //   .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src => src.ArrivalTime))
        //   .ForMember(dest => dest.DepertureTime, opt => opt.MapFrom(src => src.DepertureTime))
        //   .ForMember(dest => dest.PickPoint, opt => opt.MapFrom(src => src.PickPoint))
        //   .ForMember(dest => dest.Route, opt => opt.MapFrom(src => src.Route))
        //   .ForMember(dest => dest.PickPointId, opt => opt.MapFrom(src => src.PickPointId));


        //  CreateMap<RegPassengerObj, PassengerObj>()
        //  .ForMember(dest => dest.PickUp, opt => opt.MapFrom(src => src.PickUpId))
        //   .ForMember(dest => dest.Route, opt => opt.MapFrom(src => src.RouteId))
        //    .ForMember(dest => dest.PassengerType, opt => opt.MapFrom(src => src.PassengerType));

        //  CreateMap<EditPassengerObj, PassengerObj>()
        //       .ForMember(dest => dest.PassengerType, opt => opt.MapFrom(src => src.PassengerTypeId))
        //      ;


        //  CreateMap<RegBusObj, BusObj>();

        //  CreateMap<BusObj, EditBusObj>();

        //  CreateMap<EditBusObj, BusObj>();

        //  CreateMap<RegAssignBusRouteObj, AssignBusRouteObj>()
        //        .ForMember(dest => dest.Route, opt => opt.MapFrom(src => src.RouteId))
        //         .ForMember(dest => dest.ServiceType, opt => opt.MapFrom(src => src.ServiceType))
        //         .ForMember(dest => dest.BussAssigned, opt => opt.MapFrom(src => src.BussAssigned));



        //  CreateMap<EditAssignBusRouteObj, AssignBusRouteObj>()
        //       .ForMember(dest => dest.Route, opt => opt.MapFrom(src => src.RouteId))
        //        .ForMember(dest => dest.ServiceType, opt => opt.MapFrom(src => src.ServiceType))
        //        .ForMember(dest => dest.BussAssigned, opt => opt.MapFrom(src => src.BussAssigned));


        //  CreateMap<RegRouteCordinatorObj, RouteCordinatorObj>();

        //  CreateMap<EditRouteCordinatorObj, RouteCordinatorObj>()
        //       .ForMember(dest => dest.RouteCordinatorId, opt => opt.MapFrom(src => src.RegRouteCordinatorId))
        //       .ForMember(dest => dest.Route, opt => opt.MapFrom(src => src.RouteObj));


        //  CreateMap<RegBusReportObj, BusReportObj>();





        //  CreateMap<BusReportObj, RegBusReportObj>();

    }

}
