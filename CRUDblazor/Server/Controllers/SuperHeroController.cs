using CRUDblazor.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUDblazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext _context;
        public SuperHeroController(DataContext context)
        {
            _context = context;
        }


        public static List<Comic> Comics = new List<Comic> {
            new Comic{ Id = 1, Name = "Marvel" },
            new Comic{ Id = 2, Name = "DC" },
        };
        public static List<SuperHero> SuperHeroes = new List<SuperHero>
        {
            new SuperHero
            {
                Id = 1,
                FirstName = "Peter",
                LastName = "Parker",
                HeroName = "Spiderman",
                Comic = Comics[0],
                ComicId = 1,

            },
            new SuperHero
            {
                Id = 2,
                FirstName = "Bruce",
                LastName = "Wayne",
                HeroName = "Batman",
                Comic = Comics[1],
                ComicId = 2,

            }
        };

        [HttpGet("comics")]

        public async Task<ActionResult<List<Comic>>> GetComics()
        {
            var comics = await _context.Comics.ToListAsync();
            return Ok(Comics);
        }

        [HttpGet]

        public async Task<ActionResult<List<SuperHero>>> GetSuperHeroes()
        {
            var heroes = await _context.SuperHeroes.ToListAsync();
            return Ok(SuperHeroes);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<List<SuperHero>>> GetSingleHero(int id)
        {
            var hero = await _context.SuperHeroes.Include(x => x.Comic).FirstOrDefaultAsync(x => x.Id == id);
            if (hero == null)
            {
                return NotFound();
            }
            return Ok(hero);
        }

        [HttpPost]

        public async Task<ActionResult<List<SuperHero>>> CreateSuperHero(SuperHero hero)
        {
            hero.Comic = null;

            _context.SuperHeroes.Add(hero);
            await _context.SaveChangesAsync();
            return Ok(hero);
        }

        private async Task<List<SuperHero>> GetDbHeroes()
        {
            return await _context.SuperHeroes.Include(sh => sh.Comic).ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<SuperHero>>> UpdateSuperHero(SuperHero hero, int id)
        {
            var dbHero = await _context.SuperHeroes.Include(x => x.Comic).FirstOrDefaultAsync(x => x.Id == id);
            if(dbHero == null)
            {
                return NotFound("Sorry no hero for you");
            }
            
            dbHero.FirstName = hero.FirstName;
            dbHero.LastName = hero.LastName;
            dbHero.HeroName = hero.HeroName;
            dbHero.ComicId = hero.ComicId;

            await _context.SaveChangesAsync();

            return Ok(await GetDbHeroes());
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> DeleteSuperHero(int id)
        {
            var dbHero = await _context.SuperHeroes.Include(x => x.Comic).FirstOrDefaultAsync(x => x.Id == id);
            if (dbHero == null)
            {
                return NotFound("Sorry no hero for you");
            }
            _context.SuperHeroes.Remove(dbHero);

            await _context.SaveChangesAsync();

            return Ok(await GetDbHeroes());
        }
    }
}
