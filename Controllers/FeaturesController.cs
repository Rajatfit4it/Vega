using vega.Persistence;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using vega.Controllers.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace vega.Controllers
{
    public class FeaturesController : Controller
    {
        private readonly IMapper mapper;
        private readonly VegaDbContext context;
        public FeaturesController(VegaDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }

        [HttpGet("/api/features")]
        public async Task<IEnumerable<KeyValuePairResource>> Get()
        {
            var features = await this.context.Features.ToListAsync();
            return this.mapper.Map<List<KeyValuePairResource>>(features);
        }


    }
}