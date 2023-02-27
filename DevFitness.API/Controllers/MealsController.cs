using AutoMapper;
using DevFitness.API.Core.Entities;
using DevFitness.API.Models.InputModels;
using DevFitness.API.Models.ViewModels;
using DevFitness.API.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace DevFitness.API.Controllers
{
    [Route("api/users/{userId}/meals")]
    public class MealsController : ControllerBase
    {
        private readonly DevFitnessDbContext _dbContext;
        private readonly IMapper _mapper;

        public MealsController(DevFitnessDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        ///     Retorna Todas as Alimentações de um Usuário
        /// </summary>
        /// <param name="userId"> - Identificador do Usuário.</param>
        /// <returns> Objeto de Detalhes das Alimentações de um Usuário</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet]
        public IActionResult GetAll(int userId)
        {
            var allMeals = _dbContext.Meals.Where(x => x.UserId == userId && x.Active);

            IEnumerable<MealViewModel> allMealsViewModel = allMeals
                                        .Select(x => new MealViewModel(x.Id, x.Description, x.Calories, x.Date));
            return Ok(allMealsViewModel);
        }

        /// <summary>
        ///     Retorna uma Alimentação Especifica de um Usuário
        /// </summary>
        /// <param name="userId"> - Identificador do Usuário</param>
        /// <param name="mealId"> - Identificador da Alimentação</param>
        /// <returns> Objeto com os Detalhes de uma Alimentação Especifica de um Usuário</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet("{mealId}")]
        public IActionResult Get(int userId, int mealId)
        {
            var meal = _dbContext.Meals.SingleOrDefault(x => x.Id == mealId && x.UserId == userId);
            
            var mealViewModel = _mapper.Map<MealViewModel>(meal);
            return Ok(mealViewModel);
        }

        /// <summary>
        ///     Cadastra uma Alimentação para um Usuário
        /// </summary>
        /// <param name="userId"> - Identificador do Usuario</param>
        /// <param name="inputModel"> - Objeto com dados para Cadastro de uma Alimentação</param>
        /// <returns> Objeto Recém-criado</returns>
        /// <response code="201">Objeto Criado com Sucesso</response>
        /// <response code="400">Dados Inválidos</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post(int userId, [FromBody] CreateMealInputModel inputModel)
        {
            var meal = _mapper.Map<Meal>(inputModel);
            meal.SetUserId(userId);

            _dbContext.Meals.Add(meal);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(Get), new { userId = userId, mealId = meal.Id }, inputModel);
        }

        /// <summary>
        ///     Atualiza Dados de uma Alimentação de um Usuario
        /// </summary>
        /// <param name="userId"> - Identificador de um Usuario</param>
        /// <param name="mealId"> - Identificador de uma Alimentação</param>
        /// <param name="inputModel"> - Objeto com dados para Atualização de uma Alimentação</param>
        /// <returns> Objeto Atualizado</returns>
        /// <response code="204">Objeto Atualizado com Sucesso</response>
        /// <response code="404">Alimentação e/ou Usuário não encontrado(s)</response>
        [HttpPut("{mealId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(int userId, int mealId, [FromBody] UpdateMealInputModel inputModel)
        {
            var meal = _dbContext.Meals.SingleOrDefault(x => x.UserId == userId && x.Id == mealId);

            meal.Update(inputModel.Description, inputModel.Calories, inputModel.Date);
            _dbContext.SaveChanges();

            return NoContent();
        }

        /// <summary>
        ///     Remove uma Alimentação de um Usuário
        /// </summary>
        /// <param name="userId"> - Identificador de um Usuário</param>
        /// <param name="mealId"> - Identificador de uma Alimentação</param>
        /// <returns> Objeto Removido</returns>
        /// <response code="204">Alimentação Removida com Sucesso</response>
        /// <response code="404">Alimentação e/ou Usuário não encontrado(s)</response>
        [HttpDelete("{mealId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int userId, int mealId)
        {
            var meal = _dbContext.Meals.SingleOrDefault(x => x.UserId == userId && x.Id == mealId);

            if (meal == null)
                return NotFound();

            meal.Deactive();
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}

