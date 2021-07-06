using AutoMapper;
using DevFitness.API.Core.Entities;
using DevFitness.API.Models.InputModels;
using DevFitness.API.Models.ViewModels;
using DevFitness.API.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DevFitness.API.Controllers
{
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly DevFitnessDbContext _dbContext;
        private readonly IMapper _mapper;

        public UsersController(DevFitnessDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /* Coloca o comando /// (3 barras) para colocar uma parte do codigo abaixo */
        /// <summary>
        ///     Retornar Detalhes de Usuário
        /// </summary>
        /// <param name="id"> - Identificador de Usuário</param>
        /// <returns> Objeto de Detalhes de Usuário</returns>
        /// <response code="200">Usuário Encontrado com Sucesso</response>
        /// <response code="404">Usuário não Encontrado</response>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _dbContext.Users.SingleOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound();

            var userViewModel = _mapper.Map<UserViewModel>(user);

            return Ok(userViewModel);
        }

        /* Coloca o comando /// (3 barras) para colocar uma parte do codigo abaixo */
        /// <summary>
        ///     Cadastrar um Usúario
        /// </summary>
        /// <remarks>
        ///     Requisição de exemplo:
        ///     {
        ///         "fullName": "Luiz Dev",
        ///         "height": 1.72,
        ///         "weight": 70,
        ///         "birthDate": "1992-01-01 00:00:00"
        ///     }
        /// </remarks>
        /// <param name="inputModel"> - Objeto com Dados para Cadastro de um Usuário</param>
        /// <returns> Objeto Recém-criado</returns>
        /// <response code="201">Objeto Criado com Sucesso</response>
        /// <response code="400">Dados Inválidos</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] CreateUserInputModel inputModel)
        {
            var user = _mapper.Map<User>(inputModel);

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = 10 }, inputModel);
        }

        /// <summary>
        ///     Atualizar um Usuário
        /// </summary>
        /// <param name="id"> - Identificador do Usuário</param>
        /// <param name="inputModel"> - Objeto com Dados para Atualizar um Usuário</param>
        /// <returns> Objeto Atualizado</returns>
        /// <response code="204">Objeto Atualizado com Sucesso</response>
        /// <response code="404">Usuário não Encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(int id, [FromBody] UpdateUserInputModel inputModel)
        {
            var user = _dbContext.Users.SingleOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound();

            user.Update(inputModel.Height, inputModel.Weight);

            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
