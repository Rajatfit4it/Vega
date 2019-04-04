using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Core;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace vega.Controllers
{
    [Route("/api/vehicles/")]
    public class VehiclesController : Controller
    {
        private readonly IMapper mapper;
        private readonly IVehicleRepository repository;
        private readonly IUnifOfWork unitOfWork;

        public VehiclesController(IMapper mapper, IVehicleRepository repository, IUnifOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateVehicle([FromBody] SaveVehicleResource vehicleResource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // if (!await IsModelExists(vehicleResource.ModelId))
            // {
            //     ModelState.AddModelError("ModelId", "Invalid ModelId");
            //     return BadRequest(ModelState);
            // }

            // if (vehicleResource.Features != null)
            // {
            //     foreach (var featureid in vehicleResource.Features)
            //     {
            //         if (!await IsFeatureExists(featureid))
            //         {
            //             ModelState.AddModelError("FeatureId", featureid + " is Invalid");
            //             return BadRequest(ModelState);
            //         }
            //     }
            // }

            var vehicle = mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource);
            vehicle.LastUpdate = DateTime.Now;
            repository.Add(vehicle);
            await unitOfWork.CompleteAsync();

            vehicle = await repository.GetVehicle(vehicle.Id);

            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] SaveVehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var vehicle = await repository.GetVehicle(id);

            if (vehicle == null)
                return NotFound();


            // if (!await IsModelExists(vehicleResource.ModelId))
            // {
            //     ModelState.AddModelError("ModelId", "Invalid ModelId");
            //     return BadRequest(ModelState);
            // }

            // if (vehicleResource.Features != null)
            // {
            //     foreach (var featureid in vehicleResource.Features)
            //     {
            //         if (!await IsFeatureExists(featureid))
            //         {
            //             ModelState.AddModelError("FeatureId", featureid + " is Invalid");
            //             return BadRequest(ModelState);
            //         }
            //     }
            // }


            mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource, vehicle);
            vehicle.LastUpdate = DateTime.Now;
            await unitOfWork.CompleteAsync();
            vehicle = await repository.GetVehicle(vehicle.Id);
            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(result);
        }

        // private async Task<bool> IsModelExists(int modelId)
        // {
        //     var model = await context.Models.FindAsync(modelId);
        //     if (model != null)
        //         return true;
        //     return false;
        // }

        // private async Task<bool> IsFeatureExists(int featureid)
        // {
        //     var feature = await context.Features.FindAsync(featureid);
        //     if (feature != null)
        //         return true;
        //     return false;
        // }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await repository.GetVehicle(id, includeRelated: false);
            if (vehicle == null)
                return NotFound();

            repository.Remove(vehicle);
            await unitOfWork.CompleteAsync();
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            var vehicle = await repository.GetVehicle(id);
            if (vehicle == null)
                return NotFound();

            var vehicleResource = mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(vehicleResource);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVehicles(VehicleQueryResource filterResource)
        {
            var filter = mapper.Map<VehicleQueryResource, VehicleQuery>(filterResource);
            var queryResult = await repository.GetAllVehicles(filter);

            var queryResultResource = 
            mapper.Map<QueryResult<Vehicle>, QueryResultResource<VehicleResource>>(queryResult);
            return Ok(queryResultResource);
        }
    }
}