using System.Linq;
using AutoMapper;
using vega.Controllers.Resources;
using vega.Core.Models;
using System.Collections.Generic;

namespace vega.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Domain to API Resource
            CreateMap(typeof (QueryResult<>), typeof (QueryResultResource<>));
            CreateMap<Photo, PhotoResource>();
            CreateMap<VehicleQueryResource, VehicleQuery>();
            CreateMap<Make, MakeResource>();
            CreateMap<Make, KeyValuePairResource>();
            CreateMap<Model, KeyValuePairResource>();
            CreateMap<Feature, KeyValuePairResource>();
            CreateMap<Vehicle, SaveVehicleResource>()
            .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(e => e.FeatureId)))
            .ForMember(vr => vr.Contact,
             opt => opt.MapFrom(v => new ContactResource()
             {
                 Name = v.ContactName,
                 Email = v.ContactEmail,
                 Phone = v.ContactPhone
             }));

             CreateMap<Vehicle, VehicleResource>()
            .ForMember(vr => vr.Features, 
            opt => opt.MapFrom(v => 
            v.Features.Select(e => new KeyValuePairResource{ Id = e.Feature.Id, Name = e.Feature.Name})))
            .ForMember(vr => vr.Contact,
             opt => opt.MapFrom(v => new ContactResource()
             {
                 Name = v.ContactName,
                 Email = v.ContactEmail,
                 Phone = v.ContactPhone
             }))
             .ForMember(vr => vr.Make,
              opt=> opt.MapFrom(v=> v.Model.Make));

            //API Resource to Domain
            CreateMap<SaveVehicleResource, Vehicle>()
            .ForMember(v => v.Id, op => op.Ignore())
            .ForMember(v => v.ContactName, op => op.MapFrom(vr => vr.Contact.Name))
            .ForMember(v => v.ContactEmail, op => op.MapFrom(vr => vr.Contact.Email))
            .ForMember(v => v.ContactPhone, op => op.MapFrom(vr => vr.Contact.Phone))
            // .ForMember(v => v.Features,
            // op => op.MapFrom(vr =>
            // vr.Features.Select(e => new VehicleFeature { FeatureId = e })))
            .ForMember(v => v.Features, opt => opt.Ignore())
            .AfterMap((vr, v) =>
            {
                var removeItems = new List<VehicleFeature>();
                foreach (var f in v.Features)
                {
                    if (!vr.Features.Contains(f.FeatureId))
                    {
                        removeItems.Add(f);
                    }
                }
                foreach(var r in removeItems)
                {
                    v.Features.Remove(r);
                }
                foreach(var id in vr.Features)
                {
                    if(!v.Features.Any(i => i.FeatureId == id))
                    v.Features.Add(new VehicleFeature{ FeatureId = id});
                }
            })
            ;

        }
    }
}