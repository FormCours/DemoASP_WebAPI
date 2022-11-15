using Demo_WebAPI_01.BLL.Interfaces;
using Demo_WebAPI_01.BLL.Services;
using Demo_WebAPI_01.Models;
using Demo_WebAPI_01.Models.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Demo_WebAPI_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonnageController : ControllerBase
    {
        private IPersonnageService _PersonnageService;
        private SingletonService _SingletonService;
        private ScopedService _ScopedService1;
        private ScopedService _ScopedService2;
        private TransientService _TransientService1;
        private TransientService _TransientService2;

        public PersonnageController(IPersonnageService personnageService, SingletonService singletonService, ScopedService scopedService1, ScopedService scopedService2, TransientService transientService1, TransientService transientService2)
        {
            // Le « PersonnageService » va etre injecté 
            _PersonnageService = personnageService;

            _SingletonService = singletonService;
            _ScopedService1 = scopedService1;
            _ScopedService2 = scopedService2;
            _TransientService1 = transientService1;
            _TransientService2 = transientService2;
        }

        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json", "text/xml")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PersonnageDTO>))]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<PersonnageDTO>))]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<PersonnageDTO> personnages1 = _PersonnageService.Get().Select(p => p.toDTO());
            // IEnumerable<PersonnageDTO> personnages2 = _PersonnageService.Get().Select(PersonnageMapper.toDTO);

            Console.WriteLine($"Singleton: {_SingletonService.GetHashCode()}");
            Console.WriteLine($"Scoped 1: {_ScopedService1.GetHashCode()}");
            Console.WriteLine($"Scoped 2: {_ScopedService2.GetHashCode()}");
            Console.WriteLine($"Transient 1: {_TransientService1.GetHashCode()}");
            Console.WriteLine($"Transient 2: {_TransientService2.GetHashCode()}");

            return Ok(personnages1);
        }

        [HttpGet("{id:int}")]
        [Produces("application/json", "text/xml")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonnageDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            PersonnageDTO? personnage = _PersonnageService.GetById(id)?.toDTO();

            if (personnage is null)
            {
                return Problem(statusCode: 404, detail: "Pas trouvé (╯°□°）╯︵ ┻━┻");
                // return NotFound();
            }

            return Ok(personnage);
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PersonnageDTO))]
        public async Task<IActionResult> Add(PersonnageDataDTO dataDTO)
        {
            PersonnageDTO personnage = _PersonnageService.Insert(dataDTO.toModel()).toDTO();

            return CreatedAtAction(nameof(GetById), new { id = personnage.Id }, personnage);
            //return CreatedAtAction(nameof(GetAll), personnage );
        }

        [HttpPut("{id:int}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public IActionResult Update(int id, PersonnageDataDTO dataDTO)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        // https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-6.0
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        [Produces("application/json", "text/xml")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            bool isOk = _PersonnageService.Delete(id);

            if (!isOk)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpPatch("{id}")]
        [Consumes("application/json-patch+json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PartialUpdate([FromRoute]int id, [FromBody] JsonPatchDocument<PersonnageDTO> data)
        {
            PersonnageDTO? target = _PersonnageService.GetById(id)?.toDTO();

            if(target is null) 
                return NotFound();

            // Modification des données via le patch-json
            data.ApplyTo(target);

            _PersonnageService.Update(id, target.toModel());

            return NoContent();
        }
        /* Exemple de body pour le patch 
        
        [
          {
            "path": "/phoneNumber",
            "op": "remove"
          },

          {
            "path": "/firstname",
            "op": "replace",
            "value" : "Test"
          }
        ]
        */




        // Exemple de commande pour Swagger CodeGen
        /*
        java -jar swagger-codegen-cli.jar generate -i http://localhost:5118/swagger/v1/swagger.json -l csharp -o /tmp/test
        */
    }
}
