using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using vega.Controllers.Resources;
using vega.Core;
using vega.Core.Models;

namespace vega.Controllers
{
    [Route("/api/vehicle/{vehicleId}/photos")]
    public class PhotosController : Controller
    {
        private readonly IHostingEnvironment host;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IUnifOfWork unifOfWork;
        private readonly IMapper mapper;
        private readonly IPhotoRepository photoRepository;
        private readonly PhotosSettings photosSettings;

        public PhotosController(IHostingEnvironment host, 
        IVehicleRepository vehicleRepository,
        IUnifOfWork unifOfWork, IMapper mapper,
        IOptionsSnapshot<PhotosSettings> options,
        IPhotoRepository photoRepository)
        {
            this.host = host;
            this.vehicleRepository = vehicleRepository;
            this.unifOfWork = unifOfWork;
            this.mapper = mapper;
            this.photoRepository = photoRepository;
            this.photosSettings = options.Value;
        }
        public async Task<IEnumerable<PhotoResource>> GetPhotos(int vehicleId)
        {
            var photos = await photoRepository.GetPhotos(vehicleId);
            return mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(int vehicleId, IFormFile file)
        {
            if(file == null) return BadRequest("Please add file");
            if(file.Length == 0) return BadRequest("Empty file");
            if(file.Length> photosSettings.MaxBytes) return BadRequest("Exceed max length");
            if(!photosSettings.IsValidFormat(file.FileName)) return BadRequest("Invalid format");
            
            var vehicle = await vehicleRepository.GetVehicle(vehicleId, includeRelated:false);
            if(vehicle == null)
            return NotFound();

            var folderPath = Path.Combine(host.WebRootPath, "upload");
            if(!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var photo = new Photo(){FileName = fileName};
            vehicle.Photos.Add(photo);
            await unifOfWork.CompleteAsync();
            return Ok (mapper.Map<Photo, PhotoResource>(photo));
        }

    }
}