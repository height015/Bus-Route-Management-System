using System;
using AutoMapper;
using BRMSAPI.Domain;
using Models;

namespace Api.AutoMapper;

	public class Mapping : Profile
	{
		public Mapping()
		{
        CreateMap<UserRegistrationVM, Passengers>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.SurnName, opt => opt.MapFrom(src => src.Surname))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailAddress))
             .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
       
             .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.HomeAddress))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.DateOfBirth.ToString("MM/dd/yyyy")))
             .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.DatedJoined, opt => opt.MapFrom(src => DateTime.UtcNow.ToString("MM/dd/yyyy")))

            ;
        //CreateMap<AppUser, UserRegistrationVM>()
        //    .ForMember(dest => dest.GivenName, opt => opt.MapFrom(src => src.FirstName))
        //    .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.LastName))
        //    .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.Email))
        //    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
        //    .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
        //    .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.StateOfOrigin))
        //    .ForMember(dest => dest.HomeAddress, opt => opt.MapFrom(src => src.Address))
        //    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
        //    //.ForMember(dest => dest, opt => opt.MapFrom(src => src.PasswordHash))
        //    .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.BirthDate));




        CreateMap<UserVM, Passengers>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
           .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
           .ForMember(dest => dest.SurnName, opt => opt.MapFrom(src => src.Surname))
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailAddress))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
           .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.HomeAddress))
           .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.DateOfBirth))
           .ForMember(dest => dest.DatedJoined, opt => opt.MapFrom(src => DateTime.UtcNow.ToString("yyyy/MM/dd")));

        CreateMap<Passengers, UserVM>()
           .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
           .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.SurnName))
           .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.Email))
           .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
           .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
           .ForMember(dest => dest.HomeAddress, opt => opt.MapFrom(src => src.Address))
           //.ForMember(dest => dest, opt => opt.MapFrom(src => src.PasswordHash))
           .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.BirthDate));

        CreateMap<EditUserVM, Passengers>()
         //.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
         .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.GivenName))
         .ForMember(dest => dest.SurnName, opt => opt.MapFrom(src => src.Surname))
         //.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailAddress))
         // .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
         // .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
         //.ForMember(dest => dest.StateOfOrigin, opt => opt.MapFrom(src => src.State))
         //.ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
         .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.HomeAddress))
         .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.DateOfBirth.ToString("yyyy/MM/dd")))
         .ForMember(dest => dest.DatedJoined, opt => opt.MapFrom(src => DateTime.UtcNow.ToString("yyyy/MM/dd")));

        CreateMap<Passengers, EditUserVM>()
           //.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.GivenName, opt => opt.MapFrom(src => src.FirstName))
           .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.SurnName))
           //.ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.Email))
           //.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
           //.ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
           .ForMember(dest => dest.HomeAddress, opt => opt.MapFrom(src => src.Address))
           //.ForMember(dest => dest, opt => opt.MapFrom(src => src.PasswordHash))
           .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.BirthDate));
    }
}

