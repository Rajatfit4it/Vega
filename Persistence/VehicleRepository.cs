using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core.Models;
using vega.Core;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System;
using vega.Extensions;

namespace vega.Persistence
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VegaDbContext context;

        public VehicleRepository(VegaDbContext context)
        {
            this.context = context;
        }

        public void Add(Vehicle vehicle)
        {
            context.Vehicles.Add(vehicle);
        }

        public async Task<Vehicle> GetVehicle(int id, bool includeRelated = true)
        {
            if (!includeRelated)
                return await context.Vehicles.FindAsync(id);

            return await context.Vehicles
            .Include(v => v.Model)
            .ThenInclude(m => m.Make)
            .Include(v => v.Features)
            .ThenInclude(vf => vf.Feature)
            .SingleOrDefaultAsync(v => v.Id == id);

        }

        public async Task<QueryResult<Vehicle>> GetAllVehicles(VehicleQuery vehicleQuery)
        {
            QueryResult<Vehicle> result = new QueryResult<Vehicle>();
            var query = context.Vehicles
            .Include(v => v.Model)
            .ThenInclude(m => m.Make)
            .Include(v => v.Features)
            .ThenInclude(vf => vf.Feature).AsQueryable();

            if (vehicleQuery.MakeId.HasValue)
                query = query.Where(e => e.Model.MakeId == vehicleQuery.MakeId);
            if (vehicleQuery.ModelId.HasValue)
                query = query.Where(e => e.ModelId == vehicleQuery.ModelId);

            var columnsMap = new Dictionary<string, Expression<Func<Vehicle, object>>>()
            {
                ["make"] = v => v.Model.Make.Name,
                ["model"] = v => v.Model.Name,
                ["contactName"] = v => v.ContactName
            };
            //query = ApplyOrdering(vehicleQuery, query, columnsMap);
            query = query.ApplyOrdering(vehicleQuery, columnsMap);
            result.TotalItems = await query.CountAsync();
            query = query.ApplyPaging(vehicleQuery);
            result.Items = await query.ToListAsync();

            return result;

        }



        public void Remove(Vehicle vehicle)
        {
            context.Vehicles.Remove(vehicle);
        }
    }
}